﻿using UnityEngine;
using System.Collections;

public class DoorInteraction : IInteractableObject 
{
    public string m_openOutTrigger = "openOut";
    public string m_openInTrigger = "openIn";
    public string m_closeTrigger = "close";

    public string m_actionOnClosed = "Open";
    public string m_actionOnOpen = "Close";

    private Animator m_animator;
    private SphereCollider m_collider;

    private new void Awake()
    {
        base.Awake();

        m_animator = GetComponentInParent<Animator>();
        m_collider = GetComponent<SphereCollider>();
    }

    public override bool Activate()
    {
		interactionIsActive = !interactionIsActive;

		if (m_interactingBehaviour == null)
		{
			if(interactionIsActive)
			{
				ActionsToDisplay = m_actionOnOpen;
				//if(openOut)
				//    m_animator.SetTrigger(m_openOutTrigger);
				//else
				m_animator.SetTrigger(m_openInTrigger);
			}
			else
			{
				ActionsToDisplay = m_actionOnClosed;
				m_animator.SetTrigger(m_closeTrigger);
			}
			return false;
		}

        // calculate from which side the player is standing
        Vector3 doorRightVector = transform.right;
        Vector3 toPlayerVector = (transform.position - m_interactingBehaviour.transform.position).normalized;
        toPlayerVector.y = 0f;
        toPlayerVector.Normalize();

        Vector3 cross = Vector3.Cross(doorRightVector, toPlayerVector);
        float dot = Vector3.Dot(cross, Vector3.down);

        bool openOut = dot >= 0f ? true : false;

        if(interactionIsActive)
        {
            ActionsToDisplay = m_actionOnOpen;
            //if(openOut)
            //    m_animator.SetTrigger(m_openOutTrigger);
            //else
                m_animator.SetTrigger(m_openInTrigger);
        }
        else
        {
            ActionsToDisplay = m_actionOnClosed;
            m_animator.SetTrigger(m_closeTrigger);
        }

        return false;
    }
}
