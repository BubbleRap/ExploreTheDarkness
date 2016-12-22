using UnityEngine;
using System;
using UnityEngine.Rendering;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(Camera))]
public partial class HighlightsImageEffect : MonoBehaviour 
{
    public enum HighlightType
    {
        Glow,
        Solid
    }

    public enum HighlightPass
    {
        MarkStencilArea = 0,
        DrawSolid = 1,
        ExtrudeOutline = 2,
        Overlay = 3,
        DebugStencil = 4
    }

    public enum DrawType
    {
        Depth,
        Overlay
    }

    [Header("Highlight")]

	public HighlightType m_selectionType;
    public DrawType m_drawType;

    [SerializeField]
    private Shader highlightShader;

    [Range(0f,1f)]
    public float m_cutoffValue = 1f;
    [Range(0f, 3f)]
    public float m_intensity = 1f;
  
    [Header("Debug")]
    public bool DrawStencilBuffer;


    private Camera m_camera;
    private BlurOptimized m_blurImageEffect;

    private RenderTexture m_colorRT;
    private CommandBuffer m_renderCmdBuffer;

    private int m_rtWidth = -1, m_rtHeight = -1;
    private Material m_highlightMaterial;
    private Dictionary<Renderer, Color> m_renderers = new Dictionary<Renderer, Color>();

    #region Unity inteface

    private void OnEnable()
    {
        m_camera = GetComponent<Camera>();
        //m_camera.depthTextureMode = DepthTextureMode.Depth;
        m_camera.clearStencilAfterLightingPass = false;

        m_blurImageEffect = GetComponent<BlurOptimized>();
        m_blurImageEffect.enabled = false;
    }
                
    private void Reset()
    {
        highlightShader = Shader.Find("Custom/Highlight");
        m_blurImageEffect = gameObject.AddComponent<BlurOptimized>();
        m_blurImageEffect.enabled = false;
    }

    private void OnRenderImage (RenderTexture src, RenderTexture dst)
    {
        if(m_renderers.Count > 0)
        {          
            var outputRT = RenderTexture.GetTemporary(m_rtWidth, m_rtHeight, 24);

            if(DrawStencilBuffer)
            {
                m_highlightMaterial.SetColor("_Color", Color.red);
                Graphics.Blit(src, outputRT, m_highlightMaterial, (int)HighlightPass.DebugStencil);
            }
            else
            {
                if(m_selectionType == HighlightType.Glow)
                {
                    m_blurImageEffect.OnRenderImage(m_colorRT, m_colorRT);
                }

                m_highlightMaterial.SetFloat("_Intensity", m_intensity);
                m_highlightMaterial.SetTexture("_SecondTex", m_colorRT);
                Graphics.Blit(src, outputRT, m_highlightMaterial, (int)HighlightPass.Overlay );
            }
    
            Graphics.Blit(outputRT, dst);   
            RenderTexture.ReleaseTemporary(outputRT);
        }
        else
        {
            // nothing to draw
            Graphics.Blit(src, dst);
        }
    }

    #endregion

    #region private interface

    private void CreateResources()
    {
        m_rtWidth = m_camera.pixelWidth;
        m_rtHeight = m_camera.pixelHeight;

        m_highlightMaterial = new Material(highlightShader);

        m_colorRT = new RenderTexture(m_rtWidth, m_rtHeight, 24);
        m_colorRT.name = "Highlights Render Texture";

        m_colorRT.Create();
    }

    private void CreateCommandBuffers()
    {
        m_renderCmdBuffer = new CommandBuffer();
        m_renderCmdBuffer.name = "Highlights: Drawing Objects";

        // Drawing objects to the buffer
        m_renderCmdBuffer.SetRenderTarget(m_colorRT, BuiltinRenderTextureType.CameraTarget);
        m_renderCmdBuffer.ClearRenderTarget(false, true, Color.clear);

        var pass = m_selectionType == HighlightType.Glow ? HighlightPass.MarkStencilArea : HighlightPass.DrawSolid;
        foreach(var hObject in m_renderers)
        {
            m_renderCmdBuffer.SetGlobalColor("_Color", hObject.Value);
            m_renderCmdBuffer.SetGlobalFloat("_Cutoff",  m_cutoffValue * 0.5f);
            m_renderCmdBuffer.DrawRenderer(hObject.Key, m_highlightMaterial, 0, (int) pass);
        }
            
        foreach(var hObject in m_renderers)
        {
            m_renderCmdBuffer.SetGlobalColor("_Color", hObject.Value);
            m_renderCmdBuffer.DrawRenderer(hObject.Key, m_highlightMaterial, 0, (int) HighlightPass.ExtrudeOutline);
        }

        var drawEvent = GetCameraDrawEvent();
        m_camera.AddCommandBuffer(drawEvent, m_renderCmdBuffer);
    }

    private void ClearResources()
    {
        if(m_highlightMaterial != null)
        {
            Destroy(m_highlightMaterial);
        }

        if(m_renderCmdBuffer != null)
        {
            var drawEvent = GetCameraDrawEvent();
            m_camera.RemoveCommandBuffer(drawEvent, m_renderCmdBuffer);
        }

        if(m_colorRT != null)
        {
            m_colorRT.Release();
        }
    }

    private CameraEvent GetCameraDrawEvent()
    {
        CameraEvent drawEvent;

        if(m_camera.actualRenderingPath == RenderingPath.Forward)
        {
            drawEvent = m_drawType == DrawType.Overlay ? CameraEvent.AfterForwardOpaque : CameraEvent.BeforeForwardOpaque;
        }
        else
        {
            drawEvent = m_drawType == DrawType.Overlay ? CameraEvent.BeforeGBuffer : CameraEvent.AfterFinalPass;
        }

        return drawEvent;
    }

    #endregion
   
}
