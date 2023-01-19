using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    [SerializeField] public CraftingRecipeClass craftingRecipe;
    [NonSerialized] public CraftingController CraftingController;
    [NonSerialized] public CraftingCard craftingCard;

    public void NewRecipe(CraftingRecipeClass recipe)
    {
        this.craftingRecipe= recipe;
        try
        {
            image.sprite = recipe.output.item.sprite;
        }
        catch { }
    }

    public void OnPointerEnter(PointerEventData eventData)//detects if mouse is over crafting slot
    {
        craftingCard = Instantiate(CraftingController.craftingCard, this.transform).GetComponent<CraftingCard>();//makes a crafting card so user can see items needed
        craftingCard.SetupCard(craftingRecipe.output.item, craftingRecipe.cost,CraftingController.CalculateResourceAmount(craftingRecipe.cost));
        CraftingController.selectedCraftingSlot = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(craftingCard.gameObject);//destroys crafting card then mouse leaves
        CraftingController.selectedCraftingSlot = null;
    }
}
