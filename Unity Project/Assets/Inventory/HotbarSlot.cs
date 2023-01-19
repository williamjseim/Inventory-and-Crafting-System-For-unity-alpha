using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    [SerializeField] public InventorySlot linkedInventorySlot;
    [SerializeField] public Image frame;

    [SerializeField] private Image sprite;
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        InventoryController.hotbarEvent += updateHotbar;
    }

    public void updateHotbar()
    {
        if (linkedInventorySlot != null)
        {
            try
            {
                sprite.sprite = linkedInventorySlot.itemholder.item.sprite;
            }
            catch { }
            frame.color = Color.white;
            if (linkedInventorySlot.itemholder.amount > 1)
            {
                text.text = linkedInventorySlot.itemholder.amount.ToString();
            }
            else
            {
                text.text = "";
            }
        }
    }

    public void selectedHotbat()
    {
        try
        {
            sprite.sprite = linkedInventorySlot.itemholder.item.sprite;
        }
        catch { }
        frame.color = Color.black;

    }

}
