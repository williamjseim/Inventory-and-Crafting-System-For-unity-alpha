using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Inventory/Crafting/Recipe")]
public class CraftingRecipeClass : ScriptableObject
{
    [SerializeField] public CraftingResources[] cost;
    [SerializeField] public CraftingResources output;
}

[Serializable]
public struct CraftingResources
{
    public ItemClass item;
    public int amount;
}
