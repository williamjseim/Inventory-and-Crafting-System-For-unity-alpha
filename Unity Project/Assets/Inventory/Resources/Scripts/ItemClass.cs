using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is the super class of all items and cant be used to make an item
public abstract class ItemClass : ScriptableObject
{
    [SerializeField, HideInInspector] protected bool _hasBeenInitialised = false;

    [SerializeField] public int id = -1;
    [SerializeField] public Sprite sprite;
    [SerializeField] public int value;
    [SerializeField] public int stackSize = 1;
    [SerializeField] public string description;

    public virtual void OnValidate()
    {

    }

    public virtual void Use(PlayerController playerController)
    {

    }
}
