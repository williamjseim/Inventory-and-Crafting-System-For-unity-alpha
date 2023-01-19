using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField] private List<UiSwitch> ui;
    [SerializeField] public UiSwitch uiSwitch;
    [SerializeField] public UiSwitch hotbar;

    public delegate void UiEvent();
    public static UiEvent uiEvent;

    private void Start()
    {
        foreach (UiSwitch item in ui)
        {
            item.CloseUi();
        }
        hotbar.OpenUi();
    }

    public void CloseAllUi()
    {
        foreach (UiSwitch item in ui)
        {
            item.CloseUi();
        }
        hotbar.OpenUi();
        uiEvent.Invoke();
    }

    public void OpenInventory()
    {
        hotbar.CloseUi();
    }

    
}
