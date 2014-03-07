using UnityEngine;
using System.Collections;

public class LuminocityCalc : MonoBehaviour 
{
	public RenderTexture[] textures;

	void Start()
	{
		print (RenderTexture.SupportsStencil (textures [0]));
	}

	void Update () 
	{
//		float totalLuminocity = 0f;
//
//		foreach (RenderTexture tex in textures) 
//		{
//			tex.
//		}
	}
}
