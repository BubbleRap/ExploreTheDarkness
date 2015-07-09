using UnityEngine;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Gamma Correction")]
public class GammaCorrectionEffect : ImageEffectBase
{
	[Range(0f, 5f)]
    public float gamma = 1f;

    // Called by camera to apply image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetFloat("_Gamma", 1f/gamma);
        ImageEffects.BlitWithMaterial(material, source, destination);
    }
}