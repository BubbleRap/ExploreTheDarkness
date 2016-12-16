using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color m_highlightColor = Color.white;

    private Renderer m_renderer;

    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        HighlightsImageEffect.Instance.OnObjectMouseOver(m_renderer, m_highlightColor);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        HighlightsImageEffect.Instance.OnObjectMouseExit();
    }
}
