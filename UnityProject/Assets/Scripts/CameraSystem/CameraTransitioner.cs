using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;
using UnityEngine.Events;

public class CameraTransitioner : MonoBehaviour {

	//duration of the transition.
	public float TransitionTime;

	public enum CameraMode { Fpp,Tpp, Transitioning }
	public CameraMode Mode { get; private set; }

	//two objects which mark the transforms of the FPP and TPP camera
	public Transform TPPCameraTransform, FPPCameraTransform;

	//Two prefabs holding the values of camera presets
	public Camera TppCameraSetup, FppCameraSetup;

	[HideInInspector]
	public List<Behaviour> 
		//Those components are toggled in the beginning of transition
		TppOnlyComponents,
		FppOnlyComponents,
		//Gameplay components are disabled for the transition 
		TppOnlyGameplayComponents,
		FppOnlyGameplayComponents; 

	//those child objects are disabled/enabled in each mode
	public List<GameObject>
		TppTransformChildren,
		FppTransformChildren;

	//not to GetComponent all the time
	private Camera ThisCamera;

	private UnityEvent onFPPTransitionComplete = new UnityEvent();
	private UnityEvent onTPPTransitionComplete = new UnityEvent();

	public void AddFPPCompleteAction( UnityAction action )
	{
		onFPPTransitionComplete.AddListener( action );
	}

	public void AddTPPCompleteAction( UnityAction action )
	{
		onTPPTransitionComplete.AddListener( action );
	}

	public void CleanFPPCompleteActions()
	{
		onFPPTransitionComplete.RemoveAllListeners();
	}

	public void CleanTPPCompleteActions()
	{
		onTPPTransitionComplete.RemoveAllListeners();
	}

	void Awake (){

		ThisCamera = GetComponent<Camera>();
		Mode = CameraMode.Tpp;

		TppOnlyGameplayComponents.AddRange (new Behaviour[]{
//			GetComponent<CameraFollow>(),
//			GetComponent<CameraInput>(),
			GetComponent<CameraPhysics>(),
			GetComponent<AudioSource>() 
		});

		FppOnlyComponents.AddRange (new Behaviour[]{
			GetComponent<DepthOfField>(),
			GetComponent<Antialiasing>(),
			GetComponent<ScreenSpaceAmbientOcclusion>(),
			GetComponent<VignetteAndChromaticAberration>()
		});

		FppOnlyGameplayComponents.AddRange (new Behaviour[]{
//			GetComponent<MouseLook>(),
			GetComponent<Health>()
		});
	}

	//THE transition function
	public void Transition(){

		if (Mode == CameraMode.Tpp){

			//save the camera's position
			TPPCameraTransform.localPosition = ThisCamera.transform.localPosition;
			TPPCameraTransform.localRotation = ThisCamera.transform.localRotation;

			//tween using the 'TransitionUpdate' function
			iTween.ValueTo(gameObject, iTween.Hash(
				"from", 0f,
				"to", 1f,
				"time", TransitionTime,
				"easetype", iTween.EaseType.easeInOutSine,

				"onstart", "TurnOffTpp",			//callback on start of the tween
				"onupdate", "TransitionUpdate",		//method called on each update
				"oncomplete", "TurnOnFpp",			//callback on complete of the tween

				"onstarttarget", gameObject,
				"onupdatetarget", gameObject,
				"oncompletetarget", gameObject,

				"ignoreTimeScale", true
			));

		}
		else {

			//save the camera's position
			FPPCameraTransform.localPosition = ThisCamera.transform.localPosition;
			FPPCameraTransform.localRotation = ThisCamera.transform.localRotation;

			//tween using the 'TransitionUpdate' function
			iTween.ValueTo(gameObject, iTween.Hash(
				"from", 1f, //IN REVERSE
				"to", 0f,
				"time", TransitionTime,
				"easetype", iTween.EaseType.easeInOutSine,
				
				"onstart", "TurnOffFpp",			//callback on start of the tween
				"onupdate", "TransitionUpdate",		//method called on each update
				"oncomplete", "TurnOnTpp",			//callback on complete of the tween
				
				"onstarttarget", gameObject,
				"onupdatetarget", gameObject,
				"oncompletetarget", gameObject,
				
				"ignoreTimeScale", true
			));

		}
	}

	public void TransitionUpdate(float state){

		//not to calculate it all the time
		float negState = 1f-state;

		ThisCamera.transform.localPosition = 
			FPPCameraTransform.localPosition * state + 
				TPPCameraTransform.localPosition * negState;

		ThisCamera.transform.localRotation = Quaternion.Slerp(
			FPPCameraTransform.localRotation, 
			TPPCameraTransform.localRotation, negState);

		ThisCamera.backgroundColor = 
			FppCameraSetup.backgroundColor * state + 
			TppCameraSetup.backgroundColor * negState;

		ThisCamera.fieldOfView = 
			FppCameraSetup.fieldOfView * state + 
			TppCameraSetup.fieldOfView * negState;

		if (state > 0.75f){
			ThisCamera.cullingMask = FppCameraSetup.cullingMask;
		} else {
			ThisCamera.cullingMask = TppCameraSetup.cullingMask;
		}

	}

	//additional callback functions on the beginning and end of transitions - they turn on and off components.

	public void TurnOffFpp(){

		Mode = CameraMode.Transitioning;

		foreach (GameObject g in FppTransformChildren){
			g.SetActive(false);
		}
		
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

//		Destroy(GetComponent<Rigidbody>());
//		GetComponent<Rigidbody>().isKinematic = true;
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
		

	}

	public void TurnOnFpp(){
		
		Mode = CameraMode.Fpp;

		foreach (GameObject g in FppTransformChildren){
			g.SetActive(true);
		}

		foreach(Behaviour c in FppOnlyGameplayComponents){
			c.enabled = true;
		}

		ThisCamera.clearFlags = FppCameraSetup.clearFlags;
		ThisCamera.cullingMask = FppCameraSetup.cullingMask;
		ThisCamera.useOcclusionCulling = FppCameraSetup.useOcclusionCulling;
		ThisCamera.hdr = FppCameraSetup.hdr;

		onFPPTransitionComplete.Invoke();
		CleanFPPCompleteActions();
	}

	public void TurnOnTpp(){
		
		Mode = CameraMode.Tpp;

//		gameObject.AddComponent<Rigidbody>();
//		GetComponent<Rigidbody>().isKinematic = false;

		foreach (GameObject g in TppTransformChildren){
			g.SetActive(true);
		}

		GetComponent<SphereCollider>().enabled = true;
		foreach(Behaviour c in TppOnlyGameplayComponents){
			c.enabled = true;
		}

		onTPPTransitionComplete.Invoke();
		CleanTPPCompleteActions();
	}


}
