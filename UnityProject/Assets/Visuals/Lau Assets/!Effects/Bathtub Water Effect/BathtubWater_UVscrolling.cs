using UnityEngine;
using System.Collections;

public class BathtubWater_UVscrolling : MonoBehaviour {

	Renderer myRenderer; 
	public float animSpeed;

	// Use this for initialization
	void Start () 
	{
		myRenderer = this.gameObject.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float offset = Time.time * animSpeed;
		myRenderer.material.mainTextureOffset = new Vector2(offset, 0);
	}
}
