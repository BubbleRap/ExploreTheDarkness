using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnMouseClick : MonoBehaviour, IPointerClickHandler
{
    public delegate void ColiderEvent(Collider collider, PointerEventData eventData);
    public ColiderEvent onMouseClick = (Collider collider, PointerEventData eventData) => {};

    private Collider m_collider;

    public void Awake()
    {
        m_collider = GetComponent<Collider>();
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        onMouseClick(m_collider, eventData);
    }
}
