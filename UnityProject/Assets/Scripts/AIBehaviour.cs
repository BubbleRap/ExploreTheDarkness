using UnityEngine;
using System.Collections;
using RAIN.Core;
using RAIN.Navigation;
using RAIN.Navigation.Targets;

public class AIBehaviour : MonoBehaviour 
{
	private AIRig aiMind = null;

	public string[] spawnPoints;
	private GameObject renderer = null;

	void Awake()
	{
		renderer = transform.FindChild("Renderer").gameObject;
	}
	
	public void SpawnAI()
	{
		NavigationTarget spawnPoint = NavigationManager.instance.GetNavigationTarget(spawnPoints[ Random.Range(0, spawnPoints.Length) ]);
		transform.position = spawnPoint.Position;
		aiMind.enabled = true;
		renderer.SetActive(true);
	}

	public void DespawnAI()
	{
		aiMind.enabled = false;
		renderer.SetActive(false);
	}

	public void RetriveLightProbeResult(float intensity)
	{
		aiMind.AI.WorkingMemory.SetItem("lightIntensity", intensity);
	}
}
