using UnityEngine;
using System.Collections;

public class Emission_Blinking : MonoBehaviour {

	Color startEmission;
	Color emitTransfer;
	public Color endEmission;
	Renderer myRend;
	float t = 0;
	float duration = 0.75f;


	void Start () 
	{
		myRend = this.gameObject.GetComponent<Renderer> ();
		startEmission = Color.black;
		emitTransfer = startEmission;
		myRend.material.SetColor ("_EmissionColor", emitTransfer);
		StartCoroutine ("StartBlinking");
	}

	void OnEnable()
	{
		StartCoroutine ("StartBlinking");
	}
	void OnDisable()
	{
		StopCoroutine ("StartBlinking");
	}

	public void StopBlinking()
	{
		StopCoroutine ("StartBlinking");
		myRend.material.SetColor ("_EmissionColor", startEmission);
	}
	
	IEnumerator StartBlinking()
	{

		t = 0;

		while (true) {
			while (t < 1) {
				t += Time.deltaTime / duration;
				emitTransfer = Color.Lerp (startEmission, endEmission, t);
				myRend.material.SetColor ("_EmissionColor", emitTransfer*5.5f);
				yield return null;
			}

		t = 0;

			while (t < 1) {
				t += Time.deltaTime / duration;
				emitTransfer = Color.Lerp (endEmission, startEmission, t);
				myRend.material.SetColor ("_EmissionColor", emitTransfer);
				yield return null;
			}

			yield return null;
		}


	}

}
