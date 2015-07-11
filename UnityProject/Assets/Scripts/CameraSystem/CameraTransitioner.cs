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

	private Vector3 fromPosition, toPosition;
	private Quaternion fromRotation, toRotation;

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

		iTween.Init( gameObject );
	}

	public void TransitionTPPtoFPP()
	{
		TPPCameraTransform.localPosition = ThisCamera.transform.localPosition;
		TPPCameraTransform.localRotation = ThisCamera.transform.localRotation;

		Transition( TPPCameraTransform, FPPCameraTransform, "TurnOffTpp", "TurnOnFpp", 0f, 1f );
	}

	public void TransitionFPPtoTPP()
	{
		FPPCameraTransform.localPosition = ThisCamera.transform.localPosition;
		FPPCameraTransform.localRotation = ThisCamera.transform.localRotation;

		Transition( FPPCameraTransform, TPPCameraTransform, "TurnOffFpp", "TurnOnTpp", 0f, 1f );
	}

	//THE transition function
	public void Transition(Transform toTransform, Transform fromTransform,
	                       string onStart, string onComplete,
	                       float timeFrom, float timeTo)
	{
		toPosition = toTransform.localPosition;
		toRotation = toTransform.localRotation;

		fromPosition = fromTransform.localPosition;
		fromRotation = fromTransform.localRotation;

		iTween.ValueTo(gameObject, iTween.Hash(
			"from", timeFrom,
			"to", timeTo,
			"time", TransitionTime,
			"easetype", iTween.EaseType.easeInOutSine,
			
			"onstart", onStart,			//callback on start of the tween
			"onupdate", "TransitionUpdate",		//method called on each update
			"oncomplete", onComplete,			//callback on complete of the tween
			
			"onstarttarget", gameObject,
			"onupdatetarget", gameObject,
			"oncompletetarget", gameObject,
			
			"ignoreTimeScale", true
			));
	}

	public void TransitionUpdate(float state){

		//not to calculate it all the time
		float negState = 1f - state;

		ThisCamera.transform.localPosition = 
			fromPosition * state + 
				toPosition * negState;

		ThisCamera.transform.localRotation = Quaternion.Slerp(
			fromRotation, 
			toRotation, negState);

//		if (state > 0.75f){
//			ThisCamera.cullingMask = FppCameraSetup.cullingMask;
//		} else {
//			ThisCamera.cullingMask = TppCameraSetup.cullingMask;
//		}

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

//		ThisCamera.clearFlags = TppCameraSetup.clearFlags;
//		ThisCamera.cullingMask = TppCameraSetup.cullingMask;
//		ThisCamera.useOcclusionCulling = TppCameraSetup.useOcclusionCulling;
//		ThisCamera.hdr = TppCameraSetup.hdr;
	}

	public void TurnOffTpp(){
		
		Mode = CameraMode.Transitioning;

		foreach (GameObject g in TppTransformChildren){
			g.SetActive(false);
		}

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

//		ThisCamera.clearFlags = FppCameraSetup.clearFlags;
//		ThisCamera.cullingMask = FppCameraSetup.cullingMask;
//		ThisCamera.useOcclusionCulling = FppCameraSetup.useOcclusionCulling;
//		ThisCamera.hdr = FppCameraSetup.hdr;

		onFPPTransitionComplete.Invoke();
		CleanFPPCompleteActions();
	}

	public void TurnOnTpp(){
		
		Mode = CameraMode.Tpp;

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
