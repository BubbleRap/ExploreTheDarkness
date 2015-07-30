using UnityEngine;
using UnityEngine.Events;

using System;
using System.Collections;
using System.Collections.Generic;

public class ObjectivesManager : MonoBehaviour 
{
	private static ObjectivesManager s_instance;
	public static ObjectivesManager Instance
	{
		get{ return s_instance; }
	}

	public Objective[] m_objectives;
	private int objectiveIdx = 0;

	void Awake()
	{
		s_instance = this;
	}

	public bool IsInteractionEligable( IInteractableObject interaction )
	{
		Objective currentObj = m_objectives[objectiveIdx];

		if( currentObj.m_interactions.Contains( interaction ) )
		{
			if( currentObj.m_type == Objective.ObjectiveType.Sequentive )
			{
				int interactionIdx = currentObj.m_interactions.IndexOf( interaction );

				return interactionIdx == currentObj.interactionIdx;
			}
		}

		return true;
	}

	public void OnInteractionComplete( IInteractableObject interaction, bool state )
	{
		Objective current = m_objectives[objectiveIdx];
		if( current.m_interactions.Contains( interaction ) )
		   	current.OnInteractionComplete( state ); 
	}

	public void InitializeNextObjective()
	{
		objectiveIdx++;
	}
}

[Serializable]
public class Objective 
{
	public string name;

	[HideInInspector]
	public int interactionIdx = 0;

	public enum ObjectiveState
	{
		None = 0,
		Active,
		Completed
	}
	private ObjectiveState m_state;
	public ObjectiveState State
	{
		get{ return m_state; }
	}

	public enum ObjectiveType
	{
		Selective,
		Sequentive
	}
	public ObjectiveType m_type;

	public List<IInteractableObject> m_interactions = new List<IInteractableObject>();

	public UnityEvent m_onObjectiveComplete = new UnityEvent();

	public void OnInteractionComplete( bool state )
	{
		if( state )
			interactionIdx ++;
		else
			interactionIdx --;

		if( interactionIdx >= m_interactions.Count )
		{
			m_state = ObjectiveState.Completed;
			m_onObjectiveComplete.Invoke();

			Debug.Log("Objective completed");

			ObjectivesManager.Instance.InitializeNextObjective();
		}
	}
}
