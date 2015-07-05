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
	public string m_occludersTag = "Occluder";

	public Color m_highlightColor = new Color(1f, 0f, 0f, 0.65f);

	private Material m_highlightMaterial;
	private Camera m_camera;
	private HighlightCamera m_highlightCtrl;

	private BlurOptimized m_blur;
	
	private RenderTexture m_highlightRT;

	private void Awake()
	{
		m_highlightRT = new RenderTexture( Screen.width, Screen.height, 0);

		Camera mainCamera = GetComponent<Camera>();
		mainCamera.depthTextureMode = DepthTextureMode.Depth;


		m_camera = (new GameObject("Highlight Camera")).AddComponent<Camera>();
		m_camera.CopyFrom( mainCamera );

		m_highlightCtrl = m_camera.gameObject.AddComponent<HighlightCamera>();
		m_highlightCtrl.SetSelectionColor(m_highlightColor);
		m_highlightCtrl.SetOccluderObjectsTag(m_occludersTag);

		m_camera.cullingMask = 0;
		m_camera.clearFlags = CameraClearFlags.Nothing;
		m_camera.backgroundColor = Color.clear;
		m_camera.depthTextureMode = DepthTextureMode.None;


		m_camera.targetTexture = m_highlightRT;


		m_blur = m_camera.gameObject.AddComponent<BlurOptimized>();
		m_blur.enabled = false;

		m_camera.enabled = false;
		m_highlightCtrl.enabled = false;

		m_camera.transform.parent = mainCamera.transform;

		m_highlightMaterial = new Material( Shader.Find("Custom/Highlight") );
	}

	private void OnRenderImage( RenderTexture source, RenderTexture destination )
	{
		m_highlightCtrl.ClearCommandBuffers();
		m_highlightCtrl.RenderHighlights();

		RenderTexture rt1 = RenderTexture.GetTemporary( Screen.width, Screen.height, 0 );
		m_blur.OnRenderImage( m_highlightRT, rt1 );

		m_highlightCtrl.RenderOccluders();

		m_highlightMaterial.SetTexture("_OccludeMap", m_highlightRT);
		Graphics.Blit( rt1, rt1, m_highlightMaterial, 2 );


		m_highlightMaterial.SetTexture("_OccludeMap", rt1);
		m_highlightMaterial.SetColor("_Color", m_highlightColor);
		Graphics.Blit (source, destination, m_highlightMaterial, (int) m_selectionType);

		RenderTexture.ReleaseTemporary(rt1);
	}
}
