﻿using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour 
{
	private float horizontalShakeIntensity = 0.3f;
	private float verticalShakeIntensity = 0.3f;
	private float shakeFrequency = 0.1f;

	private Vector3 shakeOffset = Vector3.zero;
	private Vector3 originalPosition = Vector3.zero;
	private bool shaking = false;

	void Start()
	{
		originalPosition = transform.localPosition;
	}

	public void StartShake (float time) 
	{
		shaking = true;
		StartCoroutine(ShakeCamera());
		StartCoroutine(StopShaking(time));
	}

	void Update()
	{
		transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition + shakeOffset, Time.deltaTime);
	}
	
	IEnumerator ShakeCamera()
	{
		while (shaking) 
		{

			shakeOffset = new Vector3( Random.Range(-horizontalShakeIntensity , horizontalShakeIntensity ), 
			                          Random.Range(-verticalShakeIntensity , verticalShakeIntensity), 
			                          Random.Range(-verticalShakeIntensity , verticalShakeIntensity));
			print (shakeOffset);
			yield return new WaitForSeconds(shakeFrequency);
		}
	}

	IEnumerator StopShaking(float time)
	{
		yield return new WaitForSeconds(time);
		shaking = false;
		shakeOffset = Vector3.zero;
	}
}
