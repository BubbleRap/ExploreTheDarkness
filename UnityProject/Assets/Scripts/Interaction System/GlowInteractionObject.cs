using UnityEngine;
using System.Collections;

public class GlowInteractionObject : IInteractableObject 
{
	private float originalAlpha = 1f;


	[HideInInspector]
	public bool activated = false;

	public override void Activate()
	{
		// doing nothing
		// this script is controlled by itself
	}

	void Start () 
	{
		Color defaultColor = renderer.material.color;
		originalAlpha = defaultColor.a;
		defaultColor.a = 0f;
		renderer.material.color = defaultColor;
	}
	
	void Update()
	{
		bool hitObject = false;

		// IF LOOKING AT OBJECT
		Vector3 cameraRelativePosition = Camera.main.transform.InverseTransformPoint(transform.position);

		// if the object withing space between -0.8 and 0.8 of screen
		// and closer than 5 units
		hitObject = cameraRelativePosition.x < 0.8f * cameraRelativePosition.z && cameraRelativePosition.x > -0.8f * cameraRelativePosition.z
			&& cameraRelativePosition.z < 5f;

		ActivateHighlights( hitObject );
	}

	private void ActivateHighlights( bool state )
	{
		if( activated == state || !renderer.isVisible)
			return;
		
		Color curColor = renderer.material.color;
		curColor.a = state ? originalAlpha : 0f;
		renderer.material.color = curColor;	

		activated = state;
	}
}
