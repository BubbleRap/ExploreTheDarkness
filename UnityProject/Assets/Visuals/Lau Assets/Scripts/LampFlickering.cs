using UnityEngine;
using System.Collections;

public class LampFlickering : MonoBehaviour {
		
		Renderer thisRenderer;
		float emissionIntensity = 1.0f;
		Color emitColor;
		Color emitColorOn;
		
		GameObject[] childrenLightSources;

		float randomValue;
		public float minIntensity;
		public float maxIntensity;

		public float flickerSpeed;
		
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

			randomValue = Random.Range(0.0f, 100.0f);
			
		}

		void Update()
		{
			float noise = Mathf.PerlinNoise(randomValue, Time.time*flickerSpeed);

			thisRenderer.material.SetColor("_EmissionColor", emitColorOn*noise);

			foreach(GameObject _obj in childrenLightSources)
			{
				_obj.gameObject.GetComponent<Light>().intensity = Mathf.Lerp(minIntensity, maxIntensity*2, noise);
			}

		}

	}