using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHolder
{
    public int id;

    List<ItemHolder> items;

    public ResourceHolder(ItemHolder itemHolder)
    {
        items = new List<ItemHolder>();
        items.Add(itemHolder);
        id= itemHolder.id;
    }
}
