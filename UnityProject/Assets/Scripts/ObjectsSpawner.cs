using UnityEngine;
using System.Collections;

public class ObjectsSpawner : MonoBehaviour 
{
	public Transform[] m_spawnPositions;
	public GameObject m_objectToSpawn;

	private GameObject m_spawnedObject;

	public void SpawnObject( int spawnIdx )
	{
		if( m_spawnPositions.Length == 0 )
		{
			Debug.LogError("Spawn points arent set up");
			return;
		}

		if( m_objectToSpawn == null )
		{
			Debug.LogError("Spawn object isnt set up");
			return;
		}

		spawnIdx = Mathf.Clamp( spawnIdx, 0, m_spawnPositions.Length );

		m_spawnedObject = 	GameObject.Instantiate(m_objectToSpawn, 
		                       		m_spawnPositions[spawnIdx].position, 
		                       		m_spawnPositions[spawnIdx].rotation) as GameObject;
	}

	public void RemoveSpawnedObject()
	{
		if( m_spawnedObject == null )
		{
			Debug.LogWarning("There is nothing spawned");
			return;
		}

		GameObject.Destroy( m_spawnedObject );
	}
}
