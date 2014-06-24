using UnityEngine;
using System.Collections;

public class PromtButtonInteractionObject : IInteractableObject
{
	private GameObject buttonPrompt;
	private Interactor interactor;
	
	public override void Activate()
	{
		// doing nothing
		// this script is controlled by itself
	}
	
	void Start()
	{
		buttonPrompt = Instantiate(Resources.Load<GameObject>("buttonPrompt")) as GameObject;
		buttonPrompt.SetActive(false);

		GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
		interactor = siljaGO.GetComponent<Interactor>();
	}

	private void Update()
	{
		ActivatePromtButton((transform.position - interactor.transform.position).magnitude < 3f);
		
		Vector3 direction = ((transform.position - Vector3.up * 1.5f) - Camera.main.transform.position).normalized;
		buttonPrompt.transform.position = transform.position - direction * 0.25f;
		buttonPrompt.transform.LookAt(Camera.main.gameObject.transform);	
	}

	private void ActivatePromtButton( bool state )
	{
		if( interactionIsActive == state )
			return;

		if( state )
			interactor.OnInteractionEnter( gameObject );
		else
			interactor.OnInteractionExit( gameObject );
	
		buttonPrompt.SetActive(state);
		interactionIsActive = state;
	}

	void OnDestroy()
	{
		Destroy(buttonPrompt);
		buttonPrompt = null;
		Resources.UnloadUnusedAssets();
	}
}
