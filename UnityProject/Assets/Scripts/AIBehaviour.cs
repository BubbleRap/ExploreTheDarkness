using UnityEngine;
using System.Collections;
using RAIN.Core;

public class AIBehaviour : MonoBehaviour 
{
	private AIRig aiMind = null;

	public void Awake()
	{
		aiMind = GetComponentInChildren<AIRig>();
	}

	public void RetriveLightProbeResult(float intensity)
	{
		aiMind.AI.WorkingMemory.SetItem("lightIntensity", intensity);
	}
}
