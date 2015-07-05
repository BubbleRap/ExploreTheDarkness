using UnityEngine;
using System.Collections.Generic;

using UnityStandardAssets.ImageEffects;
using UnityEngine.Rendering;

public class HighlightCamera : MonoBehaviour 
{
	private Camera m_camera = null;
	private Material m_highlightMaterial = null;

	private IInteractableObject[] highlightObjects;

	private Material m_drawMaterial;

	private CommandBuffer m_renderBuffer;
	private CommandBuffer m_occlusionBuffer;

	private Renderer[] m_occluders = null;

	private void Awake()
	{
		m_camera = GetComponent<Camera>();
		m_highlightMaterial = new Material( Shader.Find("Custom/Highlight") );

		highlightObjects = FindObjectsOfType<IInteractableObject>();

		m_drawMaterial = new Material( Shader.Find("Custom/SolidColor") );

		m_renderBuffer = new CommandBuffer();
		m_camera.AddCommandBuffer( CameraEvent.AfterEverything, m_renderBuffer );

		m_occlusionBuffer = new CommandBuffer();
		m_camera.AddCommandBuffer( CameraEvent.AfterEverything, m_occlusionBuffer );
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

		RenderTexture.active = m_camera.targetTexture;
		GL.Clear(true, true, Color.clear);
		RenderTexture.active = null;
	}

	public void RenderHighlights()
	{
		if( highlightObjects == null )
			return;

		for(int i = 0; i < highlightObjects.Length; i++)
		{
			if( highlightObjects[i] == null )
				continue;
			
			if( !highlightObjects[i].IsInViewport || highlightObjects[i].IsInteracting)
				continue;
			
			Renderer renderer = highlightObjects[i].GetComponent<Renderer>();
			if( renderer == null )
				continue;
			
			m_renderBuffer.DrawRenderer( renderer, m_drawMaterial, 0, 1 );
		}

//		m_camera.Render();

		RenderTexture.active = m_camera.targetTexture;
		Graphics.ExecuteCommandBuffer(m_renderBuffer);
		RenderTexture.active = null;
	}

	public void RenderOccluders()
	{
//		m_renderBuffer.Clear();

		if( m_occluders == null )
			return;
		
		foreach(Renderer renderer in m_occluders)
		{	
			m_occlusionBuffer.DrawRenderer( renderer, m_drawMaterial, 0, 1 );
		}

//		m_camera.Render();

		RenderTexture.active = m_camera.targetTexture;
		Graphics.ExecuteCommandBuffer(m_occlusionBuffer);
		RenderTexture.active = null;
	}
}
