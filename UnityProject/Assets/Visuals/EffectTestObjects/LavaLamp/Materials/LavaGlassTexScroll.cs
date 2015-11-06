using UnityEngine;
using System.Collections;

public class LavaGlassTexScroll : MonoBehaviour 
{

	public float scrollSpeedY = -0.1F;
	public float scrollSpeedX = 0.1F;

	public Renderer rend;

	void Start() {
		rend = GetComponent<Renderer>();
	}
	void Update() {
		float offsetY = Time.time * scrollSpeedY;
		float offsetX = Time.time * scrollSpeedX;
		rend.material.SetTextureOffset("_MainTex", new Vector2(offsetX,offsetY));
	}
}
