using UnityEngine;
using UnityEngine.Events;

using System;
using System.Collections;
using System.Collections.Generic;

public class ObjectivesManager : MonoBehaviour 
{
	public Objective[] m_objectives;
	private int objectiveIdx = 0;

	#region Miki Research group code

	private GUIStyle ObjectiveUIFont;
	private string theObjective = "";
	private bool haveObjective = false;
	private int margin = 10;

	// Use this for initialization
	void Start () {
		ObjectiveUIFont = new GUIStyle();
        ObjectiveUIFont.fontSize = Mathf.Min(Screen.width, Screen.height) / 25;
        ObjectiveUIFont.normal.textColor = Color.white;
        ObjectiveUIFont.wordWrap = false;
        ObjectiveUIFont.fontStyle = FontStyle.Bold;


		AssignCallback();
		InitializeObjective();
	}
	
	void OnGUI() {
		if(theObjective != "")
		{
			Vector2 size2 = ObjectiveUIFont.CalcSize(new GUIContent(theObjective));
			GUI.Label (new Rect (Screen.width - size2.x - margin,20,100,50), theObjective, ObjectiveUIFont);
		}
	}

	public void setTheObjective(string objective)
	{
		theObjective = objective;
	}

	#endregion

	
	private void AssignCallback()
	{
		foreach( Objective obj in m_objectives )
		{
			Objective currentObjective = obj;
			foreach( IInteractableObject interaction in currentObjective.m_interactions )
			{
				interaction.m_onInteractionComplete.AddListener( () => 
				                                                { 
					currentObjective.OnInteractionComplete(); 
				} );
			}
		}
	}

	public void InitializeNextObjective()
	{
		objectiveIdx++;
		InitializeObjective();
	}

	public void InitializeObjective()
	{
		if( objectiveIdx < m_objectives.Length )
			m_objectives[objectiveIdx].Initialize( this );
	}
}

[Serializable]
public class Objective 
{
	public string name;
	private ObjectivesManager m_manager = null;


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

	public IInteractableObject[] m_interactions;
	
	public bool IsCompleted
	{
		get
		{ 
			foreach( IInteractableObject interaction in m_interactions )
				if( !interaction.IsCompleted )
					return false;
			return true;
		}
	}

	public UnityEvent m_onObjectiveComplete = new UnityEvent();

	public void OnInteractionComplete()
	{
		if( IsCompleted )
		{
			m_state = ObjectiveState.Completed;
			m_onObjectiveComplete.Invoke();

			m_manager.InitializeNextObjective();
		}
	}

	public void Initialize( ObjectivesManager manager )
	{
		m_manager = manager;

		foreach( IInteractableObject interaction in m_interactions )
			interaction.Initialize();
	}
}
