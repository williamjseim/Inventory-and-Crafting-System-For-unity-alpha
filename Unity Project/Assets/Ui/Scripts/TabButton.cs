using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    public TabGroupController controller;

    public Image image;

    public CanvasGroup[] tabs;

    public void CloseTabs()
    {
        foreach (CanvasGroup tab in tabs)
        {
            tab.alpha= 0;
            tab.interactable = false;
            tab.blocksRaycasts = false;
        }
    }

    public virtual void OpenTab()
    {
        foreach (CanvasGroup tab in tabs)
        {
            tab.alpha = 1;
            tab.interactable = true;
            tab.blocksRaycasts = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        controller.OntapSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        controller.OntabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        controller.OntabExit(this);
    }

    void Start()
    {
        image= GetComponent<Image>();
        controller.Subscripe(this);
    }

    void Update()
    {
        
    }
}
