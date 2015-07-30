using UnityEngine;
using System.Collections;

public class SewLillebror :IInteractableObject 
{

	public override bool Activate()
	{
		if( !ObjectivesManager.Instance.IsInteractionEligable( this ) )
			return false;

		interactionIsActive = !interactionIsActive;

		ObjectivesManager.Instance.OnInteractionComplete( this, true );

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

			siljaInventory.lillebrorComplete = true;
//		}

		return false;
    }
}
