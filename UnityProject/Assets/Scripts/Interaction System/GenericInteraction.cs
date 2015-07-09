using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GenericInteraction : IInteractableObject 
{
	public UnityEvent m_onInteractionActivate;
	public UnityEvent m_onInteractionDisactivate;

	public void Activate()
	{
		interactionIsActive = !interactionIsActive;

		if( interactionIsActive )
		{
			m_onInteractionActivate.Invoke();
		}
		else
		{
			m_onInteractionDisactivate.Invoke();
		}

		IsCompleted = interactionIsActive;
	}
}
