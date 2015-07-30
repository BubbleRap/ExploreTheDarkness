using UnityEngine;
using System.Collections;

public class PickUpLillebrorPiece : IInteractableObject 
{

	public override bool Activate()
	{

		if( !ObjectivesManager.Instance.IsInteractionEligable( this ) )
			return false;

		interactionIsActive = !interactionIsActive;

		ObjectivesManager.Instance.OnInteractionComplete( this, true );

		GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
		Inventory siljaInventory = siljaGO.GetComponent<Inventory>();


		if(GameObject.Find("PuzzleTrigger") != null)
		{
			AudioClip[] audioClips = GameObject.Find("PuzzleTrigger").GetComponent<PuzzleLillebror>().collectLillebrorSounds;
			if(audioClips.Length > 0)
			{
				AudioSource soundSource = GameObject.Find("HeadAudioSource").transform.GetComponent<AudioSource>();
				if(siljaGO.GetComponent<Inventory>().getLilleBrorPieces() < audioClips.Length)
				{
					soundSource.clip = audioClips[siljaGO.GetComponent<Inventory>().getLilleBrorPieces()];
					soundSource.Play();
				}
				else if(siljaGO.GetComponent<Inventory>().getLilleBrorPieces() >= audioClips.Length)
				{
					soundSource.clip = audioClips[siljaGO.GetComponent<Inventory>().getLilleBrorPieces() % audioClips.Length];
					soundSource.Play();
				}
			}
		}

		siljaInventory.setLilleBrorPieces();
//		Destroy(transform.gameObject);
		gameObject.SetActive(false);

		return false;
    }
}
