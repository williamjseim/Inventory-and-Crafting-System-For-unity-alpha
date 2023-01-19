using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTab : TabButton
{
    [SerializeField] CraftingController craftingController;

    public override void OpenTab()
    {
        base.OpenTab();
        craftingController.RemakeCraftGrid();
        craftingController.LoopThroughInventory();
    }

}
