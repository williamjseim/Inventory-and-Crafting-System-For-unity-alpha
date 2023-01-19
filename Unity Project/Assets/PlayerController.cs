using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InventoryController inventory;//this is importen and need to be imported to your project player controller or the equivilent
    [SerializeField] private AnswerCardController answerCard;//this is importen and need to be imported to your project player controller or the equivilent
    private UiController uiController;
    public bool uiOpen = false;
    private int hotbarSlot;//this is importen and need to be imported to your project player controller or the equivilent
    public int health = 0;
    public int energy = 0;

    public int maxHealth;
    public int maxEnergy;

    private void Awake()
    {
        uiController= GetComponent<UiController>();
    }

    private void Update()//this need to be imported to your player controller or a standalone controller in your project so it can controll with inventory controller. or you  can modify it an make your own one
    {//you could also use a collider to pick up items i just use raycast for testing purpose
        if (Input.GetMouseButtonDown(0) && !uiOpen)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero))//picks up item
            {
                if(hit.collider.tag == "Item")
                {
                    inventory.PickUpItem(hit.transform.GetComponent<DroppedItemHolder>());
                }
                else if(hit.collider.tag == "StorageBox")
                {
                    inventory.OpenStorageBox(hit.collider.GetComponent<StorageBoxController>());
                    uiController.uiSwitch.OpenUi();
                    uiController.OpenInventory();
                }
            }
        }
        if (Input.GetMouseButtonDown(1) && !uiOpen)//uses item then right clicking
        {
            if(inventory.currentHotBar.linkedInventorySlot.itemholder.id != -1 && inventory.currentHotBar.linkedInventorySlot.itemholder.item.GetType()==typeof(FoodClass))
            {
                    inventory.currentHotBar.linkedInventorySlot.itemholder.item.Use(this);
                    inventory.currentHotBar.linkedInventorySlot.UseItem();
                    inventory.currentHotBar.updateHotbar();
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (uiController.uiSwitch.State)
            {
                uiController.uiSwitch.CloseUi();
                inventory.currentHotBar.selectedHotbat();
                uiController.hotbar.OpenUi();
                UiController.uiEvent.Invoke();
            }
            else
            {
                uiController.uiSwitch.OpenUi();
                uiController.OpenInventory();
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            uiController.CloseAllUi();
        }
        if(Input.mouseScrollDelta.y == 1f)
        {
            hotbarSlot--;
            if(hotbarSlot < 0)
            {
                hotbarSlot= 8;
            }
            inventory.HotbarScroll(hotbarSlot);
        }
        if(Input.mouseScrollDelta.y == -1f)
        {
            hotbarSlot++;
            if (hotbarSlot > 8)
            {
                hotbarSlot = 0;
            }
            inventory.HotbarScroll(hotbarSlot);
        }
    }

    internal void AddEnergy(int energy,int health)
    {
        this.health += health;
        this.health = Mathf.Clamp(this.health, -10, maxHealth);
        this.energy += energy;
        this.energy = Mathf.Clamp(this.energy, -10, maxEnergy);
    }
}
