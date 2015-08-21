using UnityEngine;
using System.Collections;

public class ObjectsTranslator : MonoBehaviour 
{
	public Transform[] m_movePositions;
	public GameObject m_objectToMove;

	public void MoveObjectTo ( int spawnIdx )
	{
		if( m_movePositions.Length == 0 )
		{
			Debug.LogError("Spawn points arent set up");
			return;
		}

		if( m_objectToMove == null )
		{
			Debug.LogError("Spawn object isnt set up");
			return;
		}

		spawnIdx = Mathf.Clamp( spawnIdx, 0, m_movePositions.Length );

		m_objectToMove.transform.position = m_movePositions[spawnIdx].position;
		m_objectToMove.transform.rotation = m_movePositions[spawnIdx].rotation;
	}
}
