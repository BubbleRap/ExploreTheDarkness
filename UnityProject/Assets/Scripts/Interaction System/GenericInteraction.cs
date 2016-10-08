using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GenericInteraction : IInteractableObject 
{
	public UnityEvent m_onInteractionActivate;
	public UnityEvent m_onInteractionDisactivate;

	public override bool Activate()
	{
		if( !ObjectivesManager.Instance.IsInteractionEligable( this ) )
			return false;

        base.Activate();

        if(IsInteracting)
		{
			m_onInteractionActivate.Invoke();               
		}
		else
		{
			m_onInteractionDisactivate.Invoke();
		}

        ObjectivesManager.Instance.OnInteractionComplete( this, IsInteracting );
		return false;
	}
}
