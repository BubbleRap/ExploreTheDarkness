using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnMouseDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public delegate void PointerDataEvent(PointerEventData collider);

    public PointerDataEvent onMouseDragBegin = (PointerEventData collider) => {};
    public PointerDataEvent onMouseDrag = (PointerEventData collider) => {};
    public PointerDataEvent onMouseDragEnd = (PointerEventData collider) => {};

    private Collider m_collider;

    public void Awake()
    {
        m_collider = GetComponent<Collider>();
    }

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
