using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggedItem : MonoBehaviour
{
    [SerializeField] public ItemHolder itemHolder;
    [SerializeField] Image Image;

    private void Update()
    {
        this.transform.position = Input.mousePosition;
    }

    public void OverrideItemHolder(ItemHolder item)
    {
        itemHolder = new ItemHolder(item);
        SlotCheck();
    }

    private void SlotCheck()
    {
        try
        {
            Image.sprite = itemHolder.item.sprite;
        }
        catch { }
    }

    public void UpdateSlot()
    {
        try
        {
            Image.sprite = itemHolder.item.sprite;
        }
        catch { }
        if(itemHolder.amount == 0)
        {
            Destroy(gameObject);
        }
    }

}
