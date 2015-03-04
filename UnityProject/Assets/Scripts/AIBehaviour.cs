﻿using UnityEngine;
using System.Collections;
using RAIN.Core;
using RAIN.Entities;
using RAIN.Navigation;
using RAIN.Navigation.Targets;

public class AIBehaviour : MonoBehaviour 
{
	private AIRig aiMind = null;

	public string[] spawnPoints;
	private GameObject aiRenderer = null;

	private bool aiIsEnabled = false;

	void Awake()
	{
		aiRenderer = transform.FindChild("Renderer").gameObject;
		aiMind = transform.GetComponentInChildren<AIRig>();
	}
	
	public void SpawnAI()
	{
		if( aiIsEnabled )
			return;

		NavigationTarget spawnPoint = NavigationManager.Instance.GetNavigationTarget(spawnPoints[0]);
		float maxDist = 0f;

		foreach (string t in spawnPoints){
			NavigationTarget targ = NavigationManager.Instance.GetNavigationTarget(t);
			if ((targ.Position - GameObject.FindObjectOfType<Health>().transform.position).magnitude > maxDist){
				spawnPoint = targ;
			}
		}

		transform.position = spawnPoint.Position;

		aiMind.enabled = true;
		aiRenderer.SetActive(true);
		aiIsEnabled = true;

		if (this.GetComponent<AudioSource>() != null && GetComponent<AudioSource>().isPlaying)
			GetComponent<AudioSource>().Stop();
	}

	public void DespawnAI()
	{
		if( !aiIsEnabled )
			return;

		aiMind.AI.WorkingMemory.SetItem("lightIntensity", 0f);
		//aiMind.AI.WorkingMemory.SetItem("characterForm", null);
		//aiMind.AI.WorkingMemory.SetItem("moveTarget", null);

		aiMind.enabled = false;
		aiRenderer.SetActive(false);
		aiIsEnabled = false;

		foreach (AudioSource s in GetComponentsInChildren<AudioSource>())
			s.Stop();

		if (this.GetComponent<AudioSource>() != null)
			GetComponent<AudioSource>().Play();
	}

	public void RetriveLightProbeResult(float intensity)
	{
		aiMind.AI.WorkingMemory.SetItem("lightIntensity", intensity);
	}
}
