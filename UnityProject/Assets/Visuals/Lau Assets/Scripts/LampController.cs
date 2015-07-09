using UnityEngine;
using System.Collections;

public class LampController : MonoBehaviour {

	Renderer thisRenderer;
	float emissionIntensity = 1.0f;
	Color emitColor;
	Color emitColorOn;

	GameObject[] childrenLightSources;

	// Use this for initialization
	void Start () 
	{

		thisRenderer = this.gameObject.GetComponent<Renderer>();
		emitColor = Color.black; 

		emitColorOn = thisRenderer.material.GetColor("_EmissionColor");
		thisRenderer.material.SetColor("_EmissionColor", emitColorOn);

		childrenLightSources = new GameObject[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
		{
			childrenLightSources[i] = transform.GetChild(i).gameObject;
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButtonDown("Fire2"))
		{
			thisRenderer.material.SetColor("_EmissionColor", emitColor);
			DynamicGI.SetEmissive(thisRenderer, emitColor);

			foreach(GameObject _obj in childrenLightSources)
			{
				_obj.SetActive(false);
			}

		}

		if(Input.GetButtonDown("Fire1"))
		{
			thisRenderer.material.SetColor("_EmissionColor", emitColorOn);
			DynamicGI.SetEmissive(thisRenderer, emitColorOn * Mathf.LinearToGammaSpace(emissionIntensity));

			foreach(GameObject _obj in childrenLightSources)
			{
				_obj.SetActive(true);
			}

		}
	
	}
}
