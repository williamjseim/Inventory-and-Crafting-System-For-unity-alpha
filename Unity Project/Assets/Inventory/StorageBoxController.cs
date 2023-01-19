using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StorageBoxController : MonoBehaviour ,ISaveable
{
    [SerializeField] private int StorageSize = 9;
    public ItemHolder[] itemholders;

    private void Start()
    {
        itemholders = new ItemHolder[StorageSize];
        for (int i = 0; i < itemholders.Length; i++)
        {
            itemholders[i] = new ItemHolder(GetNullItem());
        }
    }

    public void OverrideItemholders(InventorySlot[] inventorySlots)//overrides itemholder in storage box so it can be saved
    {
        List<ItemHolder> list = new List<ItemHolder>();
        foreach (InventorySlot slot in inventorySlots)
        {
            list.Add(new ItemHolder(slot.itemholder));
        }
        itemholders = list.ToArray();
    }

    public ItemClass GetNullItem()//gets nullitem from database
    {
        var database = Resources.Load<Database>(path: "Item Database");
        return database.NullItem;
    }

    [Serializable]
    struct SaveData//use for saving data
    {
        public List<ItemHolder> itemholders;
    }

    public object SaveState()
    {
        SaveData saveData = new SaveData
        {
            itemholders = this.itemholders.ToList<ItemHolder>(),
        };
        return saveData;
    }

    ItemClass GetItem(int id)//gets item from database based on id
    {
        return Resources.Load<Database>(path: "Item Database").GetItem(id);
    }

    public void LoadState(object State)
    {
        var saveData = (SaveData)State;
        itemholders = new ItemHolder[saveData.itemholders.Count];
        for (int i = 0; i < this.itemholders.Length; i++)
        {
            itemholders[i] = new ItemHolder(saveData.itemholders[i], GetItem(saveData.itemholders[i].id));
        }
    }
}
