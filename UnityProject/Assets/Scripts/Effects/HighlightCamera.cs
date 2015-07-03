using UnityEngine;
using System.Collections.Generic;

using UnityStandardAssets.ImageEffects;
using UnityEngine.Rendering;

public class HighlightCamera : MonoBehaviour 
{
	public enum OcclusionType
	{
		Occludee,
		Occluder
	}

	private Camera m_camera = null;
	private Material m_highlightShader = null;
	private BlurOptimized m_blur = null;

	public RenderTexture m_highlightRT;
	private IInteractableObject[] highlightObjects;

	private Material m_drawMaterial;

	private CommandBuffer m_renderBuffer;
	private CommandBuffer m_occlusionBuffer;

	private Renderer[] m_occluders = null;

	private void Awake()
	{
		m_camera = GetComponent<Camera>();
		m_highlightShader = new Material( Shader.Find("Custom/Highlight") );

		m_blur = gameObject.AddComponent<BlurOptimized>();
		m_blur.enabled = false;

		highlightObjects = FindObjectsOfType<IInteractableObject>();
		m_highlightRT = new RenderTexture( Screen.width, Screen.height, 24);

		m_drawMaterial = new Material( Shader.Find("Custom/SolidColor") );

		m_camera.targetTexture = m_highlightRT;

		m_renderBuffer = new CommandBuffer();
		m_camera.AddCommandBuffer( CameraEvent.BeforeImageEffects, m_renderBuffer );

		m_occlusionBuffer = new CommandBuffer();
		m_camera.AddCommandBuffer( CameraEvent.AfterImageEffects, m_occlusionBuffer );
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

	private void Update()
	{
		m_renderBuffer.Clear();
		m_occlusionBuffer.Clear();

		// Drawing objects
		foreach(IInteractableObject interaction in highlightObjects)
		{
			if( !interaction.IsActivated() )
				continue;

			Renderer renderer = interaction.GetComponent<Renderer>();
			if( renderer == null )
				continue;

			m_renderBuffer.DrawRenderer( renderer, m_drawMaterial, 0, (int) OcclusionType.Occludee );
		}

		// Occluding objects

		if( m_occluders == null )
			return;

		foreach(Renderer renderer in m_occluders)
		{	
			m_occlusionBuffer.DrawRenderer( renderer, m_drawMaterial, 0, (int) OcclusionType.Occluder );
		}
	}

	private void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		m_highlightShader.SetTexture("_OccludeMap", source);
		
		m_blur.OnRenderImage( source, destination );
		
		Graphics.Blit( destination, destination, m_highlightShader, 2 );
	}
}
