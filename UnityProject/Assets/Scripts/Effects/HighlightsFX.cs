using UnityEngine;
using System.Collections.Generic;

using UnityStandardAssets.ImageEffects;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
public class HighlightsFX : MonoBehaviour 
{
	public enum HighlightType
	{
		Glow,
		Solid
	}
	public enum SortingType
	{
		Overlay,
		DepthFilter
	}

	public HighlightType m_selectionType;
	public SortingType m_sortingType;

	public string m_occludersTag = "Occluder";

	public Color m_highlightColor = new Color(1f, 0f, 0f, 0.65f);

	private Material m_highlightMaterial;


	private BlurOptimized m_blur;
	
	private RenderTexture m_highlightRT;

	private IInteractableObject[] highlightObjects;
	
	private Material m_drawMaterial;
	
	private CommandBuffer m_renderBuffer;
	private CommandBuffer m_occlusionBuffer;
	
	private Renderer[] m_occluders = null;

	private RenderTargetIdentifier m_rtID;

	private void Awake()
	{
		m_highlightRT = new RenderTexture( Screen.width, Screen.height, 0);
		m_rtID = new RenderTargetIdentifier( m_highlightRT );

		m_renderBuffer = new CommandBuffer();
		m_occlusionBuffer = new CommandBuffer();
		
		m_blur = gameObject.AddComponent<BlurOptimized>();
		m_blur.enabled = false;

	
		m_highlightMaterial = new Material( Shader.Find("Custom/Highlight") );


		highlightObjects = FindObjectsOfType<IInteractableObject>();
		
		m_drawMaterial = new Material( Shader.Find("Custom/SolidColor") );

		SetSelectionColor(m_highlightColor);
		SetOccluderObjectsTag(m_occludersTag);
	}


	public void SetSelectionColor( Color col )
	{
		m_drawMaterial.SetColor( "_Color", col );
	}
	
	public void SetOccluderObjectsTag( string tag )
	{
		if( string.IsNullOrEmpty(tag) )
			return;
		
		GameObject[] occluderGOs = GameObject.FindGameObjectsWithTag(tag);
		
		List<Renderer> occluders = new List<Renderer>();
		foreach( GameObject go in occluderGOs )
		{
			Renderer renderer = go.GetComponent<Renderer>();
			if( renderer != null )
				occluders.Add( renderer );
		}
		
		m_occluders = occluders.ToArray();
	}
	
	public void ClearCommandBuffers()
	{
		m_renderBuffer.Clear();
		m_occlusionBuffer.Clear();
		
		RenderTexture.active = m_highlightRT;
		GL.Clear(true, true, Color.clear);
		RenderTexture.active = null;
	}
	
	public void RenderHighlights()
	{
		if( highlightObjects == null )
			return;


		m_renderBuffer.SetRenderTarget( m_rtID );
		
		for(int i = 0; i < highlightObjects.Length; i++)
		{
			if( highlightObjects[i] == null )
				continue;
			
			if( !highlightObjects[i].IsInViewport || highlightObjects[i].IsInteracting)
				continue;
			
			Renderer renderer = highlightObjects[i].GetComponent<Renderer>();
			if( renderer == null )
				continue;
			
			m_renderBuffer.DrawRenderer( renderer, m_drawMaterial, 0, (int) m_sortingType );
		}

		RenderTexture.active = m_highlightRT;
		Graphics.ExecuteCommandBuffer(m_renderBuffer);
		RenderTexture.active = null;
	}
	
	public void RenderOccluders()
	{
		if( m_occluders == null )
			return;

		m_occlusionBuffer.SetRenderTarget( m_rtID );
		
		foreach(Renderer renderer in m_occluders)
		{	
			m_occlusionBuffer.DrawRenderer( renderer, m_drawMaterial, 0, (int) m_sortingType );
		}

		RenderTexture.active = m_highlightRT;
		Graphics.ExecuteCommandBuffer(m_occlusionBuffer);
		RenderTexture.active = null;
	}

	

	private void OnRenderImage( RenderTexture source, RenderTexture destination )
	{
		ClearCommandBuffers();
		RenderHighlights();

		RenderTexture rt1 = RenderTexture.GetTemporary( Screen.width, Screen.height, 0 );
		m_blur.OnRenderImage( m_highlightRT, rt1 );

		RenderOccluders();

		m_highlightMaterial.SetTexture("_OccludeMap", m_highlightRT);
		Graphics.Blit( rt1, rt1, m_highlightMaterial, 2 );


		m_highlightMaterial.SetTexture("_OccludeMap", rt1);
		m_highlightMaterial.SetColor("_Color", m_highlightColor);
		Graphics.Blit (source, destination, m_highlightMaterial, (int) m_selectionType);

		RenderTexture.ReleaseTemporary(rt1);
	}
}
