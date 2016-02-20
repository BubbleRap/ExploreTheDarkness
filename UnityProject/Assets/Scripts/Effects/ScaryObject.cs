using UnityEngine;
using System.Collections;

public class ScaryObject : MonoBehaviour
{
	private SiljaBehaviour siljaBehComp = null;
	private SiljaShakeOnScary siljaScaryComp = null;
	private ObjectsSpawner monsterSpawner = null;
	private bool hasBeenSeen = false;

	void Start()
	{
		GameObject silja = GameObject.FindGameObjectWithTag ("Player");
		siljaScaryComp = silja.GetComponent<SiljaShakeOnScary> ();
		siljaBehComp = silja.GetComponent<SiljaBehaviour>();
		monsterSpawner = GameObject.Find("MonsterSpawner").GetComponent<ObjectsSpawner>();
	}

	void Update()
	{
		Vector3 cameraRelativePosition = siljaScaryComp.transform.InverseTransformPoint(transform.position);
		
		// if the object withing screen space
		bool isInView = cameraRelativePosition.x < 1.0f * cameraRelativePosition.z && 
						cameraRelativePosition.x > -1.0f * cameraRelativePosition.z;


		// check if any of the objects are in the direct sight
		bool inDirectSight = false;

		RaycastHit hitOut;
		
		// cast Default layer only
		LayerMask mask = 1 << 0;
		
		if( !Physics.Linecast(siljaScaryComp.firstPersonCameraShaker.transform.position, transform.position, out hitOut, mask ) )
		{
			//if( hitOut.collider == GetComponent<Collider>() )
			{
				inDirectSight = true;
			}
		}

		if(inDirectSight && isInView && !hasBeenSeen)
		{
			hasBeenSeen = true;
		}

		siljaBehComp.SetScaredState( inDirectSight && isInView );

		if(hasBeenSeen && !inDirectSight && !isInView)
		{
			monsterSpawner.RemoveSpawnedObject();
			monsterSpawner.SpawnObject(Random.Range(0,monsterSpawner.m_spawnPositions.Length));
			hasBeenSeen = false;
		}
	}
}
