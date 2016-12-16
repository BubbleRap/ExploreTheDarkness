using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class HighlightsImageEffect
{
    public static HighlightsImageEffect Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void OnObjectMouseOver(Renderer renderer, Color color)
    {
        m_renderers.Add(renderer, color);
        CreateResources();
        CreateCommandBuffers();
    }

    public void OnObjectMouseExit()
    {
        ClearResources();
        m_renderers.Clear();
    }
}
