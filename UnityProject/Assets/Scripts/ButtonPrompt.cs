﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonPrompt : MonoBehaviour {

	public float distance = 0.5f;

    private Text m_text;
    private Image m_clickImage;
    private CanvasGroup m_group;

    private Canvas m_canvas;
    private RectTransform m_Rect;
    private Transform m_connectedTransform;

    public bool IsVisible { get; private set; }

    void Awake()
    {
        m_Rect = transform as RectTransform;
        m_group = GetComponent<CanvasGroup>();
        m_text = GetComponentInChildren<Text>();
        m_clickImage = GetComponentInChildren<Image>();
    }

    void Start()
    {
        m_canvas = GetComponentInParent<Canvas>();
    }

	public void SetText(string text)
    {
        m_text.text = text;
	}

	public void setInteractableUI(bool boolean)
    {
		m_clickImage.enabled = boolean;

		//Vector3 basePosition = m_text.gameObject.transform.localPosition;
        //
		//m_text.gameObject.transform.localPosition = 
        //    new Vector3(
        //        basePosition.x, 
        //        boolean ? 0.021f : 0f, 
        //        basePosition.y);
        //
		//m_clickImage.gameObject.transform.localPosition = 
        //    new Vector3(
        //        basePosition.x, 
        //        boolean ? -0.021f : 0f, 
        //        basePosition.y);
	}

	public void SetConnectedTransform(Transform t)
    {
		m_connectedTransform = t;
	}

	void Update () 
    {
        Vector2 pos = UIManager.WorldToCanvasPosition(m_connectedTransform.position);
        m_Rect.anchoredPosition = pos;

        float alpha = Mathf.Clamp01(GetAlpha());

        IsVisible = alpha > 0f;

        // t was 0.1f here for some reason?
        float currentAlpha = Mathf.Lerp(m_text.color.a, alpha, 0.1f);

        Color col = Color.white;
        col.a = currentAlpha;

        m_text.color = col;
        m_clickImage.color = col;
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
