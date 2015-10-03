using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;
using UnityEngine.Events;

public class CameraTransitioner : MonoBehaviour {

	//duration of the transition.
	public float TransitionTime;

	public enum CameraMode { Fpp,Tpp, Transitioning }
	[HideInInspector]
	public CameraMode Mode;

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
			GetComponent<CameraPhysics>(),
			GetComponent<AudioSource>() 
		});

//		FppOnlyComponents.AddRange (new Behaviour[]{
//			GetComponent<DepthOfField>(),
//			GetComponent<Antialiasing>(),
//			GetComponent<VignetteAndChromaticAberration>()
//		});

		FppOnlyGameplayComponents.AddRange (new Behaviour[]{
			GetComponent<Health>()
		});

		iTween.Init( gameObject );
	}

	public void TransitionTPPtoFPP()
	{
		TPPCameraTransform.localPosition = ThisCamera.transform.localPosition;
		TPPCameraTransform.localRotation = ThisCamera.transform.localRotation;

		Transition( TPPCameraTransform, FPPCameraTransform, 
		           "TurnOffTpp", "TurnOnFpp", 
		           gameObject, gameObject );
	}

	public void TransitionFPPtoTPP()
	{
		FPPCameraTransform.localPosition = ThisCamera.transform.localPosition;
		FPPCameraTransform.localRotation = ThisCamera.transform.localRotation;

		Transition( FPPCameraTransform, TPPCameraTransform, 
		           "TurnOffFpp", "TurnOnTpp", 
		           gameObject, gameObject );
	}

	//THE transition function
	public void Transition(Transform fromTransform, Transform toTransform,
	                       string onStart, string onComplete,
	                       GameObject onStartTarget, GameObject onCompleteTarget)
	{
		toPosition = toTransform.localPosition;
		toRotation = toTransform.localRotation;

		fromPosition = fromTransform.localPosition;
		fromRotation = fromTransform.localRotation;

		iTween.ValueTo(gameObject, iTween.Hash(
			"from", 0f,
			"to", 1f,
			"time", TransitionTime,
			"easetype", iTween.EaseType.easeInOutSine,
			
			"onstart", onStart,			//callback on start of the tween
			"onupdate", "TransitionUpdate",		//method called on each update
			"oncomplete", onComplete,			//callback on complete of the tween
			
			"onstarttarget", onStartTarget,
			"onupdatetarget", gameObject,
			"oncompletetarget", onCompleteTarget,
			
			"ignoreTimeScale", true
			));
	}

	public void TransitionUpdate(float state)
	{

		ThisCamera.transform.localPosition = Vector3.Lerp(
			fromPosition, toPosition, state );

		ThisCamera.transform.localRotation = Quaternion.Lerp(
			fromRotation, 
			toRotation, state);

	

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
