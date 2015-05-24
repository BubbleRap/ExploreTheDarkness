using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

public class CameraTransitioner : MonoBehaviour {

	public float TransitionTime;

	public enum CameraMode { Fpp,Tpp, Transitioning }
	public CameraMode Mode { get; private set; }

	public Transform TPPCameraTransform, FPPCameraTransform;

	//Those two components are referenced from prefabs and lerped by values
	public Camera TppCameraSetup, FppCameraSetup;
	public VignetteAndChromaticAberration TppVignette, FppVignette;

	public List<Behaviour> 
		//Those components are toggled in the beginning of transition
		TppOnlyComponents,
		FppOnlyComponents,
		//Gameplay components are also disabled for the transition 
		TppOnlyGameplayComponents,
		FppOnlyGameplayComponents; 

	public List<GameObject>
		TppTransformChildren,
		FppTransformChildren;

	private Camera ThisCamera;
	private VignetteAndChromaticAberration ThisVignette;

	void Awake (){

		ThisCamera = GetComponent<Camera>();
		ThisVignette = GetComponent<VignetteAndChromaticAberration>();

		Mode = CameraMode.Tpp;

		TppOnlyComponents.AddRange (new Behaviour[]{
			GetComponent<SunShafts>()
		});

		TppOnlyGameplayComponents.AddRange (new Behaviour[]{
			GetComponent<CameraFollow>(),
			GetComponent<CameraInput>(),
			GetComponent<CameraPhysics>(),
			GetComponent<AudioSource>() 
		});

		FppOnlyComponents.AddRange (new Behaviour[]{
			GetComponent<DepthOfField>(),
			GetComponent<Antialiasing>(),
			GetComponent<ScreenSpaceAmbientOcclusion>()
		});

		FppOnlyGameplayComponents.AddRange (new Behaviour[]{
			GetComponent<MouseLook>(),
			GetComponent<CameraShaker>(),
			GetComponent<Health>(),
		});
	}

	public void Transition(){

		if (Mode == CameraMode.Tpp){

			iTween.ValueTo(gameObject, iTween.Hash(
				"from", 0f,
				"to", 1f,
				"time", TransitionTime,
				"easetype", iTween.EaseType.easeInOutSine,

				"onstart", "TurnOffTpp",
				"onupdate", "TransitionUpdate",
				"oncomplete", "TurnOnFpp",

				"onstarttarget", gameObject,
				"onupdatetarget", gameObject,
				"oncompletetarget", gameObject,

				"ignoreTimeScale", true
			));

		}
		else {

			iTween.ValueTo(gameObject, iTween.Hash(
				"from", 1f,
				"to", 0f,
				"time", TransitionTime,
				"easetype", iTween.EaseType.easeInOutSine,
				
				"onstart", "TurnOffFpp",
				"onupdate", "TransitionUpdate",
				"oncomplete", "TurnOnTpp",
				
				"onstarttarget", gameObject,
				"onupdatetarget", gameObject,
				"oncompletetarget", gameObject,
				
				"ignoreTimeScale", true
			));

		}
	}

	public void TransitionUpdate(float state){

		float negState = 1f-state;

		transform.position = 
			FPPCameraTransform.position * state + 
			TPPCameraTransform.position * negState;

		transform.rotation = Quaternion.Euler(
			FPPCameraTransform.rotation.eulerAngles * state + 
			TPPCameraTransform.rotation.eulerAngles * negState);

		ThisCamera.backgroundColor = 
			FppCameraSetup.backgroundColor * state + 
			TppCameraSetup.backgroundColor * negState;

		ThisCamera.fieldOfView = 
			FppCameraSetup.fieldOfView * state + 
			TppCameraSetup.fieldOfView * negState;

		ThisCamera.depth = 
			FppCameraSetup.depth * state + 
			TppCameraSetup.depth * negState;

		ThisVignette.intensity = 
			FppVignette.intensity * state + 
			TppVignette.intensity * negState;

		ThisVignette.blur = 
			FppVignette.blur * state + 
			TppVignette.blur * negState;

		ThisVignette.blurDistance = 
			FppVignette.blurDistance * state + 
			TppVignette.blurDistance * negState;

		ThisVignette.chromaticAberration = 
			FppVignette.chromaticAberration * state + 
			TppVignette.chromaticAberration * negState;
	}

	public void TurnOffFpp(){

		Mode = CameraMode.Transitioning;

		foreach (GameObject g in FppTransformChildren){
			g.SetActive(false);
		}

		GetComponent<MeshRenderer>().enabled = false;
		foreach(Behaviour c in FppOnlyGameplayComponents){
			c.enabled = false;
		}

		foreach(Behaviour c in FppOnlyComponents){
			c.enabled = false;
		}

		foreach(Behaviour c in TppOnlyComponents){
			c.enabled = true;
		}

		ThisCamera.clearFlags = TppCameraSetup.clearFlags;
		ThisCamera.cullingMask = TppCameraSetup.cullingMask;
		ThisCamera.useOcclusionCulling = TppCameraSetup.useOcclusionCulling;
		ThisCamera.hdr = TppCameraSetup.hdr;
	}

	public void TurnOffTpp(){
		
		Mode = CameraMode.Transitioning;

		foreach (GameObject g in TppTransformChildren){
			g.SetActive(false);
		}

		Destroy(GetComponent<Rigidbody>());
		GetComponent<SphereCollider>().enabled = false;
		foreach(Behaviour c in TppOnlyGameplayComponents){
			c.enabled = false;
		}
		
		foreach(Behaviour c in TppOnlyComponents){
			c.enabled = false;
		}

		foreach(Behaviour c in FppOnlyComponents){
			c.enabled = true;
		}

		ThisCamera.clearFlags = FppCameraSetup.clearFlags;
		ThisCamera.cullingMask = FppCameraSetup.cullingMask;
		ThisCamera.useOcclusionCulling = FppCameraSetup.useOcclusionCulling;
		ThisCamera.hdr = FppCameraSetup.hdr;
	}

	public void TurnOnFpp(){
		
		Mode = CameraMode.Fpp;

		foreach (GameObject g in FppTransformChildren){
			g.SetActive(true);
		}

		GetComponent<MeshRenderer>().enabled = true;
		foreach(Behaviour c in FppOnlyGameplayComponents){
			c.enabled = true;
		}
	}

	public void TurnOnTpp(){
		
		Mode = CameraMode.Fpp;

		gameObject.AddComponent<Rigidbody>();

		foreach (GameObject g in TppTransformChildren){
			g.SetActive(true);
		}

		GetComponent<SphereCollider>().enabled = true;
		foreach(Behaviour c in TppOnlyGameplayComponents){
			c.enabled = true;
		}
	}


}
