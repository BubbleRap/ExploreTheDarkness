using UnityEngine;
using System.Collections;

using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(Camera))]
public class HighlightsFX : MonoBehaviour 
{
	public enum HighlightType
	{
		Glow,
		Solid
	}
	public HighlightType m_selectionType;

	public Color m_highlightColor;

	private Material m_highlightMaterial;
	private Camera m_camera;
	
	private void Awake()
	{
		Camera mainCamera = GetComponent<Camera>();

		m_camera = (new GameObject("Highlight Camera")).AddComponent<Camera>();
		m_camera.CopyFrom( mainCamera );

		HighlightCamera highlightCtrl = m_camera.gameObject.AddComponent<HighlightCamera>();
		highlightCtrl.SetSelectionColor(m_highlightColor);

		m_camera.cullingMask = 1 << 30;
		m_camera.depth = mainCamera.depth + 1;
		m_camera.clearFlags = CameraClearFlags.Color;
		m_camera.backgroundColor = Color.clear;

		m_camera.transform.parent = mainCamera.transform;

		m_highlightMaterial = new Material( Shader.Find("Custom/Highlight") );
	}
	
//	private void OnPostRender()
//	{
//		Graphics.Blit(m_camera.targetTexture, null, m_highlightMaterial, 1);
//	}

	private void OnRenderImage( RenderTexture source, RenderTexture destination )
	{
		m_highlightMaterial.SetTexture("_OccludeMap", m_camera.targetTexture);
		m_highlightMaterial.SetColor("_Color", m_highlightColor);
		Graphics.Blit (source, destination, m_highlightMaterial, (int) m_selectionType);
	}
}
