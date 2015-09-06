using UnityEngine;
using System.Collections;

public class ObjectsTranslator : MonoBehaviour 
{
	private static ObjectsTranslator m_instance;

	public Transform[] m_movePositions;
	public GameObject m_objectToMove;

	public static void MoveObjectTo ( int spawnIdx )
	{
		if( m_instance.m_movePositions.Length == 0 )
		{
			Debug.LogError("Spawn points arent set up");
			return;
		}

		if( m_instance.m_objectToMove == null )
		{
			Debug.LogError("Spawn object isnt set up");
			return;
		}

		spawnIdx = Mathf.Clamp( spawnIdx, 0, m_instance.m_movePositions.Length );

		m_instance.m_objectToMove.transform.position = m_instance.m_movePositions[spawnIdx].position;
		m_instance.m_objectToMove.transform.rotation = m_instance.m_movePositions[spawnIdx].rotation;
	}

	void Awake()
	{
		m_instance = this;
	}
}
