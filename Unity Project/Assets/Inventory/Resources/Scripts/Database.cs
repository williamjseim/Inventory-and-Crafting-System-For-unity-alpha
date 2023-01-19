using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;
[CreateAssetMenu(fileName = "Database", menuName = "Inventory/Item/Database")]
public class Database : ScriptableObject
{
    [SerializeField] private List<ItemClass> _itemDatabase;
    [SerializeField] private ItemClass nullItem;
    [SerializeField] private string[] FolderNames;
    public ItemClass NullItem => nullItem;

    List<ItemClass> foundItems;

    [ContextMenu("Assign Id")]
    public void AssignID()
    {
        _itemDatabase = new List<ItemClass>();
        foundItems = new List<ItemClass>();
        foreach (string folder in FolderNames)
        {
            List<ItemClass> temp = Resources.LoadAll<ItemClass>(path: $"ItemData/{folder}").OrderBy(i => i.id).ToList();
            Debug.Log(temp.Count);
            for (int i = 0; i < temp.Count; i++)
            {
                foundItems.Add(temp[i]);
            }
        }
        var hasIdInRange = foundItems.Where(i => i.id != -1 && i.id < foundItems.Count).OrderBy(i => i.id).ToList();
        var hasIdNotInRange = foundItems.Where(i => i.id != -1 && i.id >= foundItems.Count).OrderBy(i => i.id).ToList();
        var noId = foundItems.Where(i => i.id <= -1).ToList();

        var index = 0;
        for (int i = 0; i < foundItems.Count; i++)
        {
            ItemClass itemToAdd;
            itemToAdd = hasIdInRange.Find(d => d.id == i);
            if (itemToAdd != null)
            {
                _itemDatabase.Add(itemToAdd);
            }
            else if (index < noId.Count)
            {
                noId[index].id = i;
                itemToAdd = noId[index];
                index++;
                _itemDatabase.Add(itemToAdd);
            }

            foreach (var item in hasIdNotInRange)
            {
                _itemDatabase.Add(item);
            }
        }
    }

    public ItemClass GetItem(int id)
    {
        return _itemDatabase.Find(d => d.id == id);
    }
}
