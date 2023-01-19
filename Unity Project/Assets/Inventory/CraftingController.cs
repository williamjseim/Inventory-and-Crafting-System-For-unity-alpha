using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingController : MonoBehaviour, IPointerClickHandler
{
    private CraftingDatabase craftingDatabase;
    [NonSerialized] public CraftingSlot selectedCraftingSlot;
    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject craftingSlot;
    [SerializeField] private List<CraftingSlot> craftingSlots;
    [SerializeField] private InventoryController inventory;
    [SerializeField] public GameObject craftingCard;

    public Dictionary<int, List<InventorySlot>> resourceList;

    [SerializeField] bool CreativeMode = false;

    private void Start()
    {
        if (craftingDatabase == null)
        {
            craftingDatabase = Resources.Load<CraftingDatabase>(path: "Crafting Database");//change this so it matches your database
        }
    }

    [ContextMenu("Remake Grid")]
    public void RemakeCraftGrid()//remakes the crafting grid so were sure it loads correctly
    {
        foreach (var slot in craftingSlots)
        {
            DestroyImmediate(slot.gameObject);
        }

        craftingSlots = new List<CraftingSlot>();
        if (craftingDatabase == null)
        {
            craftingDatabase = Resources.Load<CraftingDatabase>(path: "Crafting Database");
        }

        foreach (CraftingRecipeClass recipes in craftingDatabase.craftingRecipes)
        {
            CraftingSlot obj = Instantiate(craftingSlot, grid.transform).GetComponent<CraftingSlot>();
            craftingSlots.Add(obj);
            obj.NewRecipe(recipes);
            obj.CraftingController = this;
        }
    }

    [ContextMenu("Loop Through")]
    public void LoopThroughInventory()//call this only then opening then opening crafting tab
    {//loops through all slots in inventory and adds it to a list in a dictionary. one list pr item
        resourceList = new Dictionary<int, List<InventorySlot>>();
        foreach (InventorySlot slot in inventory.slots)
        {
            if (!resourceList.ContainsKey(slot.itemholder.id) && slot.itemholder.id != -1)
            {
                resourceList.Add(slot.itemholder.id, new List<InventorySlot> { slot });
            }
            else if (resourceList.ContainsKey(slot.itemholder.id) && slot.itemholder.id != -1)
            {
                resourceList.TryGetValue(slot.itemholder.id, out List<InventorySlot> list);
                list.Add(slot);
            }
        }
    }

    public int[] CalculateResourceAmount(CraftingResources[] craftingResources)//calculates how many of the needed item are in your inventory
    {
        int[] array = new int[craftingResources.Length];
        List<InventorySlot> list;
        for (int j = 0; j < craftingResources.Length; j++)
        {
            if (resourceList.ContainsKey(craftingResources[j].item.id))
            {
                resourceList.TryGetValue(craftingResources[j].item.id, out list);
            }
            else
            {
                list = null;
            }
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (array[j] == 0)
                    {
                        array[j] = 0;
                    }
                    array[j] += list[i].itemholder.amount;
                    if (array[j] >= 999)
                    {
                        array[j] = 999;
                        break;
                    }
                }
            }
        }
        return array;
    }

    private void CraftItem()//crafts item
    {
        if (!CreativeMode && checkForItem())//checks if you have the items needed for crafting
        {
            if (CheckAmountOfItem())//checks if you have the need amount of items
            {
                if (inventory.currentDraggedItem == null)//checks if dragged item is null
                {
                    RemoveItems();//removes items
                    selectedCraftingSlot.craftingCard.SetupCard(selectedCraftingSlot.craftingRecipe.output.item, selectedCraftingSlot.craftingRecipe.cost, CalculateResourceAmount(selectedCraftingSlot.craftingRecipe.cost));//recalculates amount of items to be written in the crafting card
                    inventory.DragableItemForCrafting(selectedCraftingSlot.craftingRecipe.output);//makes a dragged item holder with the crafted items
                }
                else if (inventory.currentDraggedItem.itemHolder.item.id == selectedCraftingSlot.craftingRecipe.output.item.id && inventory.currentDraggedItem.itemHolder.amount < inventory.currentDraggedItem.itemHolder.item.stackSize)//checks if the dragged itemholder's item is the same as the item youre attempting to craft
                {
                    RemoveItems();
                    selectedCraftingSlot.craftingCard.SetupCard(selectedCraftingSlot.craftingRecipe.output.item, selectedCraftingSlot.craftingRecipe.cost, CalculateResourceAmount(selectedCraftingSlot.craftingRecipe.cost));
                    inventory.AddToDraggedCraftingItem(selectedCraftingSlot.craftingRecipe.output);//adds crafting item to existing dragged item
                }
            }
        }
        else if (CreativeMode && inventory.currentDraggedItem == null)//if creative mode is on crafting doesnt cost anything
        {
            inventory.DragableItemForCrafting(selectedCraftingSlot.craftingRecipe.output);
        }
        else if (CreativeMode && inventory.currentDraggedItem != null && inventory.currentDraggedItem.itemHolder.item.id == selectedCraftingSlot.craftingRecipe.output.item.id)
        {
            inventory.DragableItemForCrafting(selectedCraftingSlot.craftingRecipe.output);
        }
    }

    private bool checkForItem()//runs though all item in crafting recipe and checks if they are in the resource dictionary
    {
        foreach (CraftingResources recipe in selectedCraftingSlot.craftingRecipe.cost)
        {
            if (!resourceList.ContainsKey(recipe.item.id))
            {
                return false;
            }
        }
        return true;
    }

    private bool CheckAmountOfItem()//checks if there enough of each item for the recipe
    {
        foreach (CraftingResources recipe in selectedCraftingSlot.craftingRecipe.cost)
        {
            resourceList.TryGetValue(recipe.item.id, out List<InventorySlot> list);
            int amount = 0;
            foreach (InventorySlot slot in list)
            {
                amount = +slot.itemholder.amount;
                if (amount >= 999)
                {
                    break;
                }
            }
            if (amount < recipe.amount)
            {
                return false;
            }
        }
        return true;
    }

    private void RemoveItems()//removes all the needed items for the recipe
    {
        foreach (CraftingResources recipe in selectedCraftingSlot.craftingRecipe.cost)
        {
            resourceList.TryGetValue(recipe.item.id, out List<InventorySlot> list);
            list.OrderBy(i => i.itemholder.amount);
            if (list.Last().itemholder.amount >= recipe.amount)
            {
                list.Last().itemholder.amount -= recipe.amount;
                list.Last().UpdateSlot();
                if (list.Last().itemholder.amount == 0)
                {
                    list.Last().ClearSlot();
                    list.Last().UpdateSlot();
                    list.Remove(list.Last());
                }
            }
            else
            {
                int resourcesRequired = recipe.amount;
                while (resourcesRequired > 0)
                {
                    list.Last().itemholder.amount--;
                    resourcesRequired--;
                    if (list.Last().itemholder.amount == 0)
                    {
                        list.Last().ClearSlot();
                        list.Last().UpdateSlot();
                        list.RemoveAt(list.Count-1);
                    }
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)//detects if crafting grid gets clicked while hovering a crafting slot
    {
        if (selectedCraftingSlot != null)
        {
            CraftItem();
        }
    }
}
