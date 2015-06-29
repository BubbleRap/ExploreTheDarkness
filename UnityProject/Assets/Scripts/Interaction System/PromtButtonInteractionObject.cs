using UnityEngine;
using System.Collections;

public class PromtButtonInteractionObject : IInteractableObject
{

	void Start()
	{
		Destroy(this);
	}

	public override void Activate()
	{
		interactionIsActive = !interactionIsActive;
	}
	

}
