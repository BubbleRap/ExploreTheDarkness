using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnMouseClick : MonoBehaviour, IPointerClickHandler
{
    public Action<PointerEventData> onMouseClick = (PointerEventData eventData) => {};

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        onMouseClick(eventData);
    }
}
