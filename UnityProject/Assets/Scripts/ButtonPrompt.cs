using UnityEngine;
using System.Collections;

public class ButtonPrompt : MonoBehaviour {

	public float distance = 0.5f;

    private TextMesh textMesh;
    private Renderer m_renderer;
    private Renderer cachedRenderer;
    private Transform connectedObject;
	private SpriteRenderer spriteRenderer;

    public bool isVisible { get; private set; }

    void Awake()
    {
        textMesh = GetComponentInChildren<TextMesh>();
        cachedRenderer = textMesh.GetComponent<Renderer>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

	void OnEnable()
    {
        textMesh.color = Color.white;
        cachedRenderer.material.color = new Color (1f, 1f, 1f, 0f);
	}

	public void SetText(string text)
    {
        textMesh.text = text;
	}

	public void setInteractableUI(bool boolean)
    {
		spriteRenderer.enabled = boolean;

		Vector3 basePosition = textMesh.gameObject.transform.localPosition;

		textMesh.gameObject.transform.localPosition = 
            new Vector3(
                basePosition.x, 
                boolean ? 0.021f : 0f, 
                basePosition.y);
        
		spriteRenderer.gameObject.transform.localPosition = 
            new Vector3(
                basePosition.x, 
                boolean ? -0.021f : 0f, 
                basePosition.y);
	}

	public void SetConnectedTransform(Transform t)
    {
		connectedObject = t;
        m_renderer = connectedObject.GetComponent<Renderer>();
	}

	void Update () 
    {
        float size = (Camera.main.transform.position - transform.position).magnitude ;
		transform.localScale = new Vector3(size,size,size);

        float alpha = GetAlpha();

        isVisible = alpha > 0f;

		if(alpha < 0f)
		{
			alpha = 0;
		}

		cachedRenderer.material.color = new Color (1f, 1f, 1f, Mathf.Lerp(cachedRenderer.material.color.a, alpha, 0.1f));
		spriteRenderer.material.color = cachedRenderer.material.color;

        transform.LookAt(Camera.main.transform);
	}

	private float GetAlpha()
    {
        if (m_renderer != null && !m_renderer.isVisible)
			return 0f;

        float distFromSilja = Vector3.Distance (transform.position, Camera.main.transform.position);

        if(distFromSilja > 10f)
            return 0f;
        
		distFromSilja /= 4f;

        return 1f - (distFromSilja * distFromSilja);
	}
}
