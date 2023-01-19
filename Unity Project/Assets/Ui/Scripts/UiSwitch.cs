using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSwitch : MonoBehaviour
{
    [SerializeField] private PlayerController PlayerController;
    protected CanvasGroup _canvasGroup;
    bool disabled;
    public bool State => disabled;
    public virtual void CloseUi()//always call this before open ui
    {
        if(_canvasGroup == null)
        {
            _canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        }
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable= false;
        _canvasGroup.blocksRaycasts= false;
        disabled = false;
        PlayerController.uiOpen = disabled;
    }

    public virtual void OpenUi()
    {
        if (_canvasGroup == null)
        {
            _canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        }
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        disabled = true;
        PlayerController.uiOpen = disabled;
    }
}
