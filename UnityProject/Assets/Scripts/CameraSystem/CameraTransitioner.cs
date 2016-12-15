using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;
using UnityEngine.Events;

public class CameraTransitioner : MonoBehaviour 
{
    public enum CameraMode 
    { 
        Tpp, 
        Fpp, 
        Transitioning 
    }

    public CameraMode Mode { get; set; }

	//two objects which mark the transforms of the FPP and TPP camera
	public Transform TPPCameraTransform, FPPCameraTransform;

	private Vector3 fromPosition, toPosition;
	private Quaternion fromRotation, toRotation;


	private UnityEvent onFPPTransitionComplete = new UnityEvent();
	private UnityEvent onTPPTransitionComplete = new UnityEvent();

    private Transform OtherCameraTransform;
    private Transform prevOtherCameraTransformParent;

    public void AddFPPCompleteAction( UnityAction action )
	{
		onFPPTransitionComplete.AddListener( action );
	}

	public void AddTPPCompleteAction( UnityAction action )
	{
		onTPPTransitionComplete.AddListener( action );
	}

	public void TransitionTPPtoFPP(Transform lookingAt = null)
	{
		TPPCameraTransform.localPosition = transform.localPosition;
		TPPCameraTransform.localRotation = transform.localRotation;

		Transition( TPPCameraTransform, FPPCameraTransform, 
		           TurnOnFpp, lookingAt );
	}

	public void TransitionFPPtoTPP()
	{
		FPPCameraTransform.localPosition = transform.localPosition;
		FPPCameraTransform.localRotation = transform.localRotation;

		Transition( FPPCameraTransform, TPPCameraTransform, 
		           TurnOnTpp);
	}

    public void TransitionFPPtoFPP()
    {
        FPPCameraTransform.localPosition = transform.localPosition;
        FPPCameraTransform.localRotation = transform.localRotation;

        Transition( transform, TPPCameraTransform, 
            TurnOnFpp);
    }

    public void TransitionTPPtoOther(Transform other)
    {
        TPPCameraTransform.localPosition = transform.localPosition;
        TPPCameraTransform.localRotation = transform.localRotation;

        OtherCameraTransform = other;
        prevOtherCameraTransformParent = other.parent;
        other.SetParent(FPPCameraTransform.parent);

        Transition(TPPCameraTransform, other,
                   TurnOnFpp);
    }

    public void TransitionOtherToTPP(Transform other)
    {
        OtherCameraTransform = other;
        prevOtherCameraTransformParent = other.parent;
        other.SetParent(FPPCameraTransform.parent);

        Transition(other, TPPCameraTransform,
                   TurnOnTpp);
    }

    public void TransitionFPPtoOther(Transform other)
    {
        FPPCameraTransform.localPosition = transform.localPosition;
        FPPCameraTransform.localRotation = transform.localRotation;

        OtherCameraTransform = other;
        prevOtherCameraTransformParent = other.parent;
        other.SetParent(FPPCameraTransform.parent);

        Transition(FPPCameraTransform, other,
            TurnOnFpp);
    }

    public void TransitionOtherToFPP(Transform other)
    {
        OtherCameraTransform = other;
        prevOtherCameraTransformParent = other.parent;
        other.SetParent(FPPCameraTransform.parent);

        Transition(other, FPPCameraTransform,
            TurnOnFpp);
    }

	//THE transition function
	public void Transition(Transform fromTransform, Transform toTransform,
	                       Action onComplete,
	                       Transform lookingAt = null)
	{
        Quaternion prevToTransformLookAtRot = toTransform.rotation;
        if (lookingAt != null)
            toTransform.LookAt(lookingAt);

		toPosition = toTransform.localPosition;
		toRotation = toTransform.localRotation;

        toTransform.rotation = prevToTransformLookAtRot;

        fromPosition = fromTransform.localPosition;
		fromRotation = fromTransform.localRotation;

        StartCoroutine(TransitionRoutine(onComplete));
	}

    private IEnumerator TransitionRoutine(Action onEnd)
    {
        float from = 0f;
        float to = 1f;

        Mode = CameraMode.Transitioning;

        while(from < to)
        {
            yield return null;

            transform.localPosition = Vector3.Lerp(
                fromPosition, toPosition, from / to );

            transform.localRotation = Quaternion.Lerp(
                fromRotation, 
                toRotation, from / to);

            from += Time.deltaTime;
        }

        onEnd();
    }
        

	public void TurnOnFpp(){
		
		Mode = CameraMode.Fpp;

        if (OtherCameraTransform != null)
            OtherCameraTransform.SetParent(prevOtherCameraTransformParent);
        OtherCameraTransform = null;

		onFPPTransitionComplete.Invoke();
        onFPPTransitionComplete.RemoveAllListeners();
	}

	public void TurnOnTpp(){
		
		Mode = CameraMode.Tpp;

        if (OtherCameraTransform != null)
            OtherCameraTransform.SetParent(prevOtherCameraTransformParent);
        OtherCameraTransform = null;

        onTPPTransitionComplete.Invoke();
        onTPPTransitionComplete.RemoveAllListeners();
	}
}
