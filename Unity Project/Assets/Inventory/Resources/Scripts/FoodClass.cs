using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Food", menuName = "Inventory/Item/Food")]
public class FoodClass : ItemClass
{
    [SerializeField] public int health;
    [SerializeField] public int energy;

    public override void OnValidate()
    {
        if (!_hasBeenInitialised)
        {
            stackSize = 99;
            _hasBeenInitialised = true;
        }
    }

    public override void Use(PlayerController player)
    {
        player.AddEnergy(energy, health);
    }
}
