using UnityEngine;
using System.Collections;

public class LuminocityIE : ImageEffectBase {

	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit (source, destination, material);
	}
}
