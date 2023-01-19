using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarUiController : UiSwitch
{
    public override void CloseUi()
    {
        if (_canvasGroup == null)
        {
            _canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        }
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    public override void OpenUi()
    {
        if (_canvasGroup == null)
        {
            _canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        }
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }
}
