﻿using UnityEngine;
using System.Collections;

public class SiljaSound : MonoBehaviour 
{
	public enum SoundLocation
	{
		Silja_Camera,
		Silja_Head,
		Silja_Feet
	}

	public SoundLocation location = SoundLocation.Silja_Camera;
	public AudioSource[] audioSources;

	public AudioClip[] stepSounds;
	[Range(0f,1f)]
	public float volume = 1f;

	public void OnStep()
	{
		audioSources [(int)location].PlayOneShot (stepSounds[Random.Range(0, stepSounds.Length)], volume);
	}
}
