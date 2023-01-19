using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ItemHolder
{
    [NonSerialized] public ItemClass item;//cant be saved to json
    public int amount;//will be saved to json
    public int id = -1;//will be saved to json

    public ItemHolder()
    {
        amount= 0;
        id = -1;
    }

    public ItemHolder(ItemClass item)
    {
        this.item = item;
        amount= 0;
        id = item.id;
    }

    public ItemHolder(ItemHolder item)
    {
        this.item = item.item;
        this.amount = item.amount;
        this.id = item.id;
    }

    public ItemHolder(ItemHolder item, ItemClass itemClass)
    {
        this.item = itemClass;
        this.amount = item.amount;
        this.id = item.id;
    }

    public ItemHolder(CraftingResources resource)
    {
        item = resource.item;
        amount = resource.amount;
        id = resource.item.id;
    }
    public void ChangeItem(DroppedItemHolder item)
    {
        this.item = item.item;
        id = item.id;
    }
}
