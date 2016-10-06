using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonPrompt : MonoBehaviour {

	public float distance = 0.5f;

    private TextMesh m_text;
    private Renderer m_textRenderer;
    private SpriteRenderer m_clickImage;

    private Transform m_connectedTransform;

    public bool IsVisible { get; private set; }

    void Awake()
    {
        m_text = GetComponentInChildren<TextMesh>();
        m_textRenderer = m_text.GetComponent<Renderer>();
        m_clickImage = GetComponentInChildren<SpriteRenderer>();
    }

	public void SetText(string text)
    {
        m_text.text = text;
	}

	public void setInteractableUI(bool boolean)
    {
		m_clickImage.enabled = boolean;

		Vector3 basePosition = m_text.transform.localPosition;
        
		m_text.transform.localPosition = 
            new Vector3(
                basePosition.x, 
                boolean ? 0.021f : 0f, 
                basePosition.y);
        
		m_clickImage.transform.localPosition = 
            new Vector3(
                basePosition.x, 
                boolean ? -0.021f : 0f, 
                basePosition.y);
	}

	public void SetConnectedTransform(Transform t)
    {
		m_connectedTransform = t;
	}

	void Update () 
    {
        Vector3 direction = ((m_connectedTransform.position - Vector3.up * 1.5f) - Camera.main.transform.position).normalized;
        transform.position = m_connectedTransform.position - direction * 0.25f;
        transform.LookAt(Camera.main.gameObject.transform);

        float size = (Camera.main.transform.position - transform.position).magnitude ;
        transform.localScale = new Vector3(size,size,size);

        float alpha = Mathf.Clamp01(GetAlpha());
        IsVisible = alpha > 0f;

        float currentAlpha = Mathf.MoveTowards(m_text.color.a, alpha, Time.deltaTime);

        Color col = Color.white;
        col.a = currentAlpha;

        m_text.color = col;
        //m_clickImage.color = col;
        m_textRenderer.material.color = col;
	}

	private float GetAlpha()
    {
        float distFromSilja = Vector3.Distance (m_connectedTransform.position, Camera.main.transform.position);

        if(distFromSilja > 10f)
            return 0f;
        
		distFromSilja /= 4f;

        return 1f - (distFromSilja * distFromSilja);
	}
}
