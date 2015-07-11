using UnityEngine;
using System.Collections;

public class SewLillebror :IInteractableObject 
{

	public override bool Activate()
	{
		if( !m_isInitialized )
		{
			Debug.Log("Too soon.");
			return false;
		}

		interactionIsActive = !interactionIsActive;
		IsCompleted = true;

		GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
		Inventory siljaInventory = siljaGO.GetComponent<Inventory>();
//		if(siljaGO.GetComponent<Inventory>().getLilleBrorPieces() > 4 && !siljaGO.GetComponent<Inventory>().lillebrorComplete)
//		{
			if(GameObject.Find("PuzzleTrigger") != null)
			{
				AudioClip audioClip = GameObject.Find("PuzzleTrigger").GetComponent<PuzzleLillebror>().sewingLillebrorSound;
				AudioSource soundSource = GameObject.Find("HeadAudioSource").transform.GetComponent<AudioSource>();
				soundSource.clip = audioClip;
				soundSource.Play();
			}

			if(siljaGO.GetComponent<CollectLillebrorUI>() == null)
			{
				CollectLillebrorUI objectiveController = siljaGO.AddComponent<CollectLillebrorUI>();
			}
			siljaGO.GetComponent<CollectLillebrorUI>().lillebrorMessage = "";
			siljaGO.GetComponent<CollectLillebrorUI>().multipleTask = false;

			siljaInventory.lillebrorComplete = true;
//		}

		return false;
    }
}
