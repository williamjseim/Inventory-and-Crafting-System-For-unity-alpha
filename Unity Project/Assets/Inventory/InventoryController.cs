using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InventoryController : MonoBehaviour, IPointerClickHandler, ISaveable
{
    [NonSerialized] public HotbarSlot currentHotBar;

    [SerializeField] private GameObject Grid;//grid object for inventory slots
    [SerializeField] private GameObject hotBarGrid;//grid object for hotbar slots
    [SerializeField] private GameObject hotBarSlotPrefab;//hotbar slot prefab
    [SerializeField] private GameObject InventoryHotbarGrid;//this is where the last 9 inventory slots that are linked to the hotbar the generated
    [SerializeField] private int InventorySize;//size of the inventory
    [SerializeField] private GameObject slotPrefab;//inventory slot prefab
    [SerializeField] private GameObject draggedItem;//prefab of dragged item
    [SerializeField] public InventorySlot[] slots;//array of all inventory slots so they can easily be called and destroyed

    [SerializeField] private HotbarSlot[] hotbarSlots = new HotbarSlot[9];//array of hotbarslots so they can easily be called and destroyed

    public delegate void HotbarEvent();//event to call hotbar then ever inventory closes
    public static HotbarEvent hotbarEvent;

    [SerializeField] GameObject storageGrid;//grid for storagebox where inventory slots for storagebox will be made
    private StorageBoxController storageBox;//reference to the storagebox controller on storageboxes
    [SerializeField] private InventorySlot[] storageBoxIventorySlots = null;

    private Database itemDatabase;//no calling this incase you change anything
    public Database ItemDatabase//call this instead
    {
        get { return itemDatabase; }
    }

    public InventorySlot SelectedSlot { get; set; }//reference to inventory slots mouse is over

    [NonSerialized] public DraggedItem currentDraggedItem;//reference to draggeditem gameobject so it can be called easily and destryed

    private void Awake()
    {
        itemDatabase = Resources.Load<Database>(path: "Item Database");//changes this to match your database
        foreach (InventorySlot slot in slots)
        {
            slot.itemholder = new ItemHolder(ItemDatabase.NullItem);
        }
    }

    private void Start()
    {
        UiController.uiEvent += CloseStorageBox;
        currentHotBar = hotbarSlots[0];
        currentHotBar.selectedHotbat();
    }

    [ContextMenu("Relink Hotbar")]
    private void RelinkHotbar()//relinks all hotbar slots to the last 9 inventory slots
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            hotbarSlots[i].linkedInventorySlot = slots[i + InventorySize - 9];
        }
    }

    [ContextMenu("RemakeGrid")]
    private void RemakeGrid()//only run in editor. remakes every inventory slot in grid
    {
        foreach (InventorySlot slot in slots)
        {
            if(slot != null)
            DestroyImmediate(slot.gameObject);
        }
        foreach (HotbarSlot slot in hotbarSlots)
        {
            if (slot != null)
                DestroyImmediate(slot.gameObject);
        }
        slots= new InventorySlot[InventorySize];
        for (int i = 0; i < InventorySize-9; i++)
        {
            InventorySlot obj = Instantiate(slotPrefab, Grid.transform).GetComponent<InventorySlot>();
            slots[i] = obj;
            obj.inventoryController= this;
        }
        for (int i = 0; i < 9; i++)
        {
            InventorySlot obj = Instantiate(slotPrefab, InventoryHotbarGrid.transform).GetComponent<InventorySlot>();
            slots[i+InventorySize-9] = obj;
            obj.inventoryController = this;
        }
        for (int i = 0; i < 9; i++)
        {
            HotbarSlot obj = Instantiate(hotBarSlotPrefab, hotBarGrid.transform).GetComponent<HotbarSlot>();
            hotbarSlots[i] = obj;
            obj.linkedInventorySlot = slots[i + InventorySize - 9];
        }
    }

    public void PickUpItem(DroppedItemHolder item)//checks if stack of item already exists or if theres space to make a new stack
    {
        if (CheckForSpace())
        {
            InventorySlot slot;
            if (slot = CheckForItemStack(item.id))
            {
                item.amount = AddToStack(item,slot);
            }
            if(item.amount > 0)
            {
                MakeNewStack(item);
            }
            if(item.amount == 0)
            {
                Destroy(item.gameObject);
            }
        }
    }

    private bool CheckForSpace()//checks for empty inventory slot
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.itemholder.id == -1)
                return true;
        }
        return false;
    }

    private InventorySlot CheckForItemStack(int id)//checks for existing stack
    {
        foreach (InventorySlot slot in slots)
        {
            if(slot.itemholder.id == id && slot.itemholder.amount < slot.itemholder.item.stackSize)
            {
                return slot;
            }
        }
        return null;
    }

    private void MakeNewStack(DroppedItemHolder item)//adds items to an empty slot
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.itemholder.id == -1)
            {
                slot.itemholder.ChangeItem(item);
                item.amount = AddToStack(item,slot);
            }
            if(item.amount == 0)
            {
                break;
            }
        }
    }

    private int AddToStack(DroppedItemHolder item,InventorySlot slot)//adds items to existing stack
    {
        while (item.amount > 0 &&slot.itemholder.amount < slot.itemholder.item.stackSize)
        {
            slot.itemholder.amount++;
            item.amount--;
        }
        slot.UpdateSlot();
        return item.amount;
    }

    private void CombineStacks(ItemHolder item,InventorySlot slot)//combines dragged item and selected inventory slot if the items are the same
    {
        while (item.amount > 0 && slot.itemholder.amount < slot.itemholder.item.stackSize)
        {
            slot.itemholder.amount++;
            item.amount--;
        }
        slot.UpdateSlot();
    }

    private void SwapStacks()//swaps items between dragged item and selected inventory slot
    {
        ItemHolder tempItem = new ItemHolder();
        tempItem = new ItemHolder(SelectedSlot.itemholder);
        SelectedSlot.OverrideItemHolder(currentDraggedItem.itemHolder);
        currentDraggedItem.OverrideItemHolder(tempItem);
    }

    private void PickupHalfStack()//picks up half a stack then right clicking on inventory slot
    {
        if(SelectedSlot.itemholder.amount> 1)
        {
            currentDraggedItem = Instantiate(draggedItem, Input.mousePosition, new Quaternion(), this.transform).GetComponent<DraggedItem>();
            currentDraggedItem.itemHolder = new ItemHolder(SelectedSlot.itemholder.item);
            currentDraggedItem.itemHolder.amount = (SelectedSlot.itemholder.amount) / 2;
            SelectedSlot.itemholder.amount = SelectedSlot.itemholder.amount - currentDraggedItem.itemHolder.amount;
            SelectedSlot.UpdateSlot();
            currentDraggedItem.UpdateSlot();
        }
        else
        {
            PickupStack();
        }
    }

    private void CheckItem()//checks if dragged item is empty
    {
        if(currentDraggedItem.itemHolder.amount == 0)
        {
            Destroy(currentDraggedItem.gameObject);
        }
    }

    private void PickupStack()//picks up full stack into a dragged item
    {
        currentDraggedItem = Instantiate(draggedItem, Input.mousePosition, new Quaternion(), this.transform).GetComponent<DraggedItem>();
        currentDraggedItem.OverrideItemHolder(SelectedSlot.itemholder);
        SelectedSlot.ClearSlot();
    }

    private void PlaceOneItem()//places one item on empty inventory or where items match and stack isnt full
    {
        if(SelectedSlot.Empty() || SelectedSlot.itemholder.id == currentDraggedItem.itemHolder.id && SelectedSlot.itemholder.amount < SelectedSlot.itemholder.item.stackSize)
        {
            if(SelectedSlot.Empty())
            {
                SelectedSlot.itemholder = new ItemHolder(currentDraggedItem.itemHolder.item);
            }
            SelectedSlot.itemholder.amount++;
            currentDraggedItem.itemHolder.amount--;
            currentDraggedItem.UpdateSlot();
            SelectedSlot.UpdateSlot();
        }
    }

    public void DragableItemForCrafting(CraftingResources craftedItem)//makes a dragged item after crafting
    {
        currentDraggedItem = Instantiate(draggedItem,Input.mousePosition,new Quaternion(), this.transform).GetComponent<DraggedItem>();
        currentDraggedItem.itemHolder = new ItemHolder(craftedItem);
        currentDraggedItem.UpdateSlot();
    }

    public void AddToDraggedCraftingItem(CraftingResources craftedItem)//adds crafted item to dragged item if items match
    {
        while (currentDraggedItem.itemHolder.amount < currentDraggedItem.itemHolder.item.stackSize&&craftedItem.amount > 0)
        {
            currentDraggedItem.itemHolder.amount++;
            craftedItem.amount--;
        }
        currentDraggedItem.UpdateSlot();
    }

    public void OnPointerClick(PointerEventData eventData)//detects if moouse clicks
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if (SelectedSlot != null&&currentDraggedItem == null)//picks up item from inventoryslot
            {
                if(Input.GetKey(KeyCode.LeftShift)|| Input.GetKey(KeyCode.RightShift))
                {

                }
                else if (!SelectedSlot.Empty() && currentDraggedItem == null)
                {
                    PickupStack();
                }
            }
            else if (SelectedSlot != null && currentDraggedItem != null)//puts stack down or add to existing stack
            {
                if (SelectedSlot.Empty())
                {
                    SelectedSlot.OverrideItemHolder(currentDraggedItem.itemHolder);
                    Destroy(currentDraggedItem.gameObject);
                }
                else if (!SelectedSlot.Empty())
                {
                    if(SelectedSlot.itemholder.id == currentDraggedItem.itemHolder.id)
                    {
                        CombineStacks(currentDraggedItem.itemHolder, SelectedSlot);
                        CheckItem();
                    }
                    else if (SelectedSlot.itemholder.id != currentDraggedItem.itemHolder.id)
                    {
                        SwapStacks();
                    }
                }
            }
        }
        else if(eventData.button == PointerEventData.InputButton.Right)//detects right click
        {
            if (!SelectedSlot.Empty() && currentDraggedItem == null)//picks up half a stack
            {
                PickupHalfStack();
            }
            else if (currentDraggedItem!=null)//places one item
            {
                PlaceOneItem();
            }
        }
    }

    public void HotbarScroll(int i)//scrolls through hotbar
    {
        currentHotBar = hotbarSlots[i];
        hotbarEvent.Invoke();
        currentHotBar.selectedHotbat();
    }

    public void UpdateHotbar()//updates hotbar images
    {
        hotbarEvent.Invoke();
    }

    public void OpenStorageBox(StorageBoxController box)//opens storagebox
    {
        if(storageBoxIventorySlots != null)
        {
            foreach (InventorySlot slot in storageBoxIventorySlots)
            {
                Destroy(slot.gameObject);
            }
            storageBoxIventorySlots = null;
        }
        storageBox = box;
        storageBoxIventorySlots = new InventorySlot[storageBox.itemholders.Length];
        for (int i = 0; i < storageBoxIventorySlots.Length; i++)
        {
            InventorySlot obj = Instantiate(slotPrefab, storageGrid.transform).GetComponent<InventorySlot>();
            storageBoxIventorySlots[i] = obj;
            obj.itemholder = storageBox.itemholders[i];
            obj.inventoryController = this;
            obj.UpdateSlot();
        }
    }

    [ContextMenu("closeChest")]
    public void CloseStorageBox()//closes the storage box importen that this is called then closing the chest ui or it wont save to the chest
    {
        if(storageBoxIventorySlots != null)
        {
            for (int i = 0; i < storageBox.itemholders.Length; i++)
            {
                storageBox.OverrideItemholders(storageBoxIventorySlots);
            }
            foreach (InventorySlot slot in storageBoxIventorySlots)
            {
                Destroy(slot.gameObject);
            }
        storageBoxIventorySlots = null;
        }
    }

    [Serializable]
    struct SaveData//use this for savíng
    {
        public List<ItemHolder> slots;
    }

    public object SaveState()
    {
        SaveData saveData = new SaveData
        {
            slots = new List<ItemHolder>() 
        };
        foreach (InventorySlot slot in slots)
        {
            saveData.slots.Add(slot.itemholder);
            slot.UpdateSlot();
        }
        return saveData;
    }

    public void LoadState(object State)
    {
        var savedata = (SaveData)State;
        for (int i = 0; i < savedata.slots.Count; i++)
        {
            slots[i].itemholder = new ItemHolder(savedata.slots[i], ItemDatabase.GetItem(savedata.slots[i].id));
            slots[i].UpdateSlot();
        }
    }
}
