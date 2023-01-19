using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI ;

public class TabGroupController : MonoBehaviour
{
    [SerializeField] private CanvasGroup inventory;
    private List<TabButton> tabButtons= new List<TabButton>();
    public TabButton selectedTab;

    private void Start()
    {
        UiController.uiEvent += ResetUi;
    }

    public void Subscripe(TabButton tabButton)
    {
        if(tabButton!= null)
        {
            tabButtons.Add(tabButton);
        }
    }

    public void OntapSelected(TabButton tabButton)
    {
        selectedTab= tabButton;
        ResetTabs();
        for (int i = 0; i < tabButtons.Count; i++)
        {
            tabButtons[i].CloseTabs();
        }
        tabButton.OpenTab();
    }

    public void OntabEnter(TabButton tabButton)
    {
        ResetTabs();
    }

    public void OntabExit(TabButton tabButton)
    {
        ResetTabs();
    }

    public void ResetTabs()
    {
        foreach (TabButton tab in tabButtons)
        {

        }
    }

    private void ResetUi()
    {
        foreach (TabButton tab in tabButtons)
        {
            tab.CloseTabs();
        }
        inventory.alpha= 1;
        inventory.interactable = true;
        inventory.blocksRaycasts= true;
    }
}
