using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventUiController : UiSwitch
{
    public UnityEvent uiEvent;

    public override void CloseUi()
    {
        base.CloseUi();
        uiEvent.Invoke();
    }
}
