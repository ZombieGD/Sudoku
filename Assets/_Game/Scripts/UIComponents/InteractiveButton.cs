using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractiveButton : Button, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    public event Action OnPointerUpAction;
    public event Action OnPointerEnterAction;
    public event Action OnPointerExitAction;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        eventData.pointerPress = gameObject;
        OnPointerEnterAction?.Invoke();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        eventData.pointerPress = null;
        OnPointerExitAction?.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpAction?.Invoke();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {

    }
}
