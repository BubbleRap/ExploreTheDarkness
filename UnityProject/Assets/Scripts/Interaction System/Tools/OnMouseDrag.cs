using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnMouseDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Action<PointerEventData> onMouseDragBegin = (PointerEventData collider) => {};
    public Action<PointerEventData> onMouseDrag = (PointerEventData collider) => {};
    public Action<PointerEventData> onMouseDragEnd = (PointerEventData eventData) => {};

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        onMouseDragBegin(eventData);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        onMouseDrag(eventData);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
		onMouseDragEnd(eventData);
    }
}
