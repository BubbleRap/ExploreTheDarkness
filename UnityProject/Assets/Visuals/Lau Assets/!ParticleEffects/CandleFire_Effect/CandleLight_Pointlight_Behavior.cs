using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class CandleLight_Pointlight_Behavior : MonoBehaviour {

	public Light candleLight;
	float randomValue;
	public float minIntensity;
	public float maxIntensity;
	public float flickerSpeed;

	// Use this for initialization
	void Start () {
		candleLight = this.gameObject.light;
		randomValue = Random.Range(0.0f, 65535.0f);
	}
	
	// Update is called once per frame
	void Update () {
		float noise = Mathf.PerlinNoise(randomValue, Time.time*flickerSpeed);
		candleLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
	}
}
