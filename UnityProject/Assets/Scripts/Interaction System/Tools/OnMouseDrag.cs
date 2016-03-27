using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnMouseDrag : MonoBehaviour, IDragHandler
{
    public delegate void PointerDataEvent(PointerEventData collider);
    public PointerDataEvent onMouseDrag = (PointerEventData collider) => {};

    private Collider m_collider;

    public void Awake()
    {
        m_collider = GetComponent<Collider>();
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        onMouseDrag(eventData);
    }
}
