using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Action<PointerEventData> onMouseOver = (PointerEventData eventData) => {};
    public Action<PointerEventData> onMouseOut = (PointerEventData collider) => {};

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        onMouseOver(eventData);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        onMouseOut(eventData);
    }
}
