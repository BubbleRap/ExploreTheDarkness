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
        if(m_objectives.Length == 0)
            return true;
        
		Objective currentObj = m_objectives[objectiveIdx];

		if( currentObj.m_type == Objective.ObjectiveType.Sequentive &&
            currentObj.m_interactions.Contains(interaction) )
		{
            for (int i=0; i< currentObj.m_interactions.Count; ++i)
            {
                if (!currentObj.hasInteracted(i))
                {
                    if (currentObj.m_interactions[i] == interaction)
                        return true;
                    else return false;
                }
            }
		}

		return true;
	}

    public void OnInteractionComplete( IInteractableObject interaction, bool state )
	{
        if(m_objectives.Length == 0)
            return;
        
		Objective current = m_objectives[objectiveIdx];
		if( current.m_interactions.Contains( interaction ) )
		   	current.OnInteractionComplete(interaction, state ); 
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
    public List<bool> interactionsComplete = new List<bool>();

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

	public void OnInteractionComplete(IInteractableObject interaction, bool state )
	{
        bool allComplete = true;
        
        for (int i = 0; i < m_interactions.Count; ++i)
        {
            if (m_interactions[i] == interaction)
            {
                if (!hasInteracted(i) && state)
                    interactionsComplete[i] = true;
                else if (hasInteracted(i) && !state)
                    interactionsComplete[i] = false;
            }
            else if (m_type == Objective.ObjectiveType.Sequentive && state)
            {
                return;
            }

            allComplete = allComplete && hasInteracted(i);
        }
       
		if( allComplete )
		{
			m_state = ObjectiveState.Completed;
			m_onObjectiveComplete.Invoke();

			Debug.Log("Objective completed");

			ObjectivesManager.Instance.InitializeNextObjective();
		}
	}

    internal bool hasInteracted(int i)
    {
        while (interactionsComplete.Count <= i)
            interactionsComplete.Add(false);

        return interactionsComplete[i];

    }
}
