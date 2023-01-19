using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image sprite;
    [SerializeField] private TextMeshProUGUI text;
    public ItemHolder itemholder;
    public InventoryController inventoryController;

    

    public void UpdateSlot()//updates sprite and amount number and sets item to null/air item incase slot is empty
    {
        if(itemholder.amount > 0)
        {
            try
            {
                sprite.sprite = itemholder.item.sprite;
            }
            catch { }
            if(itemholder.amount > 1)
            text.text = itemholder.amount.ToString();
            else
            {
                text.text = "";
            }
        }
        else
        {
            itemholder.item = inventoryController.ItemDatabase.NullItem;
            try
            {
                sprite.sprite = itemholder.item.sprite;
            }
            catch { }
            text.text = "";
        }
    }

    public bool Empty()//checks if slots is empty
    {
        if (itemholder.id == -1)
            return true;
        else
            return false;
    }

    public void OverrideItemHolder(ItemHolder item)//easy way to override item
    {
        itemholder = new ItemHolder(item);
        UpdateSlot();
    }
    
    public void ClearSlot()//clears slot
    {
        itemholder = new ItemHolder(inventoryController.ItemDatabase.NullItem);
        UpdateSlot();
    }

    public void UseItem()//removes one item then it gets used
    {
        this.itemholder.amount -= 1;
        if(itemholder.amount == 0)
        {
            ClearSlot();
        }
        UpdateSlot();
    }

    public void OnPointerEnter(PointerEventData eventData)//sends this object to inventorycontroller then mouse is over slot
    {
        inventoryController.SelectedSlot = this;
    }

    public void OnPointerExit(PointerEventData eventData)//removes this object from inventory controller then mouse leaves slot
    {
        inventoryController.SelectedSlot = null;
    }
}
