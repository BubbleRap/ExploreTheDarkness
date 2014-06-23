using UnityEngine;
using System.Collections;

public class SwitchModeInteractionObject : IInteractableObject 
{
	public float darknessDelay = 10f;

	public bool nextLevel = false;
	public bool transitionToDarkness = false;

	// called from Interactor.cs
	public override void Activate()
	{
		if(nextLevel)
		{
			//(GameObject.FindObjectOfType<EndScreenController>() as EndScreenController).ShowLoadingScreen(Application.LoadLevelAsync(1));
			Application.LoadLevelAsync(1);
		}
		
		if (transitionToDarkness){
			Invoke("TriggerDarkness",darknessDelay);
		}
	}

	public void TriggerDarkness(){

		GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
		SiljaBehaviour siljaBeh = siljaGO.GetComponent<SiljaBehaviour>();

		Transitioner transitionContoller = Component.FindObjectOfType(typeof(Transitioner)) as Transitioner;
		transitionContoller.doTransition(true);

		siljaBeh.FreezeSilja(false, null, null);

		// COMMENTED AS I THINK THERE IS SOME DIFFERENCE

		//firstPersonCamera.gameObject.SetActive(false);
		//charMotor.enabled = true;
		//transform.gameObject.GetComponent<MovementController>().canMove = true;
		//camInput.enabled = true;
	}
}
