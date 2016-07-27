using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnMouseDrag : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public delegate void PointerDataEvent(PointerEventData collider);

    public PointerDataEvent onMouseDragBegin = (PointerEventData collider) => {};
    public PointerDataEvent onMouseDrag = (PointerEventData collider) => {};
	public PointerDataEvent onMouseOut = (PointerEventData collider) => {};

	public delegate void ColiderEvent(Collider collider, PointerEventData eventData);
	public ColiderEvent onMouseOver = (Collider collider, PointerEventData eventData) => {};
	public ColiderEvent onMouseDragEnd = (Collider collider, PointerEventData eventData) => {};

    private Collider m_collider;

    public void Awake()
    {
        m_collider = GetComponent<Collider>();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        onMouseDragBegin(eventData);
    }

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		onMouseOver(m_collider, eventData);
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		onMouseOut(eventData);
	}

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        onMouseDrag(eventData);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
		onMouseDragEnd(m_collider, eventData);
    }
}
