using UnityEngine;
using System.Collections;

public class PickUpLillebrorPiece : IInteractableObject 
{

	public override void Activate()
	{
		interactionIsActive = !interactionIsActive;

		GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
		Inventory siljaInventory = siljaGO.GetComponent<Inventory>();

		if(siljaGO.GetComponent<CollectLillebrorUI>() == null)
		{
			CollectLillebrorUI objectiveController = siljaGO.AddComponent<CollectLillebrorUI>();
		}
		siljaGO.GetComponent<CollectLillebrorUI>().lillebrorMessage = "Collect the pieces";
		siljaGO.GetComponent<CollectLillebrorUI>().multipleTask = true;

		if(siljaGO.GetComponent<Inventory>().getLilleBrorPieces() == 4)
		{
			siljaGO.GetComponent<CollectLillebrorUI>().lillebrorMessage = "Sew Lillebror together";
			siljaGO.GetComponent<CollectLillebrorUI>().multipleTask = false;
		}

		if(GameObject.Find("PuzzleTrigger") != null)
		{
			AudioClip[] audioClips = GameObject.Find("PuzzleTrigger").GetComponent<PuzzleLillebror>().collectLillebrorSounds;
			if(audioClips.Length > 0)
			{
				AudioSource soundSource = GameObject.Find("HeadAudioSource").transform.audio;
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
		Destroy(transform.gameObject);
    }
}
