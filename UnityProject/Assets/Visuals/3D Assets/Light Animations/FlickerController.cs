using UnityEngine;
using System.Collections;

public class FlickerController : MonoBehaviour {

	Renderer thisRenderer;
	public Color emitColorOn;
	public Color emitColorOff;
	public bool activateFlick;

	// Use this for initialization
	void Start () 
	{
		thisRenderer = this.gameObject.GetComponent<Renderer>();

		//emitColorOn = Color.white; 
		//emitColorOff = Color.black;


		thisRenderer.material.SetColor("_EmissionColor", emitColorOn);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(activateFlick)
		{
			thisRenderer.material.SetColor("_EmissionColor", emitColorOff);
		}

		if(!activateFlick)
		{
			thisRenderer.material.SetColor("_EmissionColor", emitColorOn*2f);
		}
	}
}
