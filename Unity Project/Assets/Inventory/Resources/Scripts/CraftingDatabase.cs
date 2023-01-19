using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Crafting Database", menuName = "Database/Crafting")]
public class CraftingDatabase : ScriptableObject
{
    [SerializeField] public List<CraftingRecipeClass> craftingRecipes;

    [ContextMenu("Get Crafting Recipes")]
    private void GetCraftingRecipes()
    {
        craftingRecipes = new List<CraftingRecipeClass>();
        var foundItems = Resources.LoadAll<CraftingRecipeClass>(path: "CraftingData").ToList();
        foreach (var recipes in foundItems)
        {
            craftingRecipes.Add(recipes);
        }
    }
}
