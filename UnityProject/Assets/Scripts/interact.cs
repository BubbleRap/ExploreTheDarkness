using UnityEngine;
using System.Collections;

public class interact : MonoBehaviour {

	public Transform firstPersonCamera;
	public Transform thirdPersonCamera;
	bool isFirstPerson = false;
	[HideInInspector]
	public bool isInteractMode = false;
	[HideInInspector]
	public HighlightedObject interactedObject;

	private CameraInput camInput = null;
	private CharacterController charMotor = null;

	void Awake()
	{
		camInput = GetComponentInChildren<CameraInput>();
		charMotor = GetComponent<CharacterController>();
	}

	void Update () 
	{
		if(interactedObject != null)
		{
			if(interactedObject.StoppedPlaying() && interactedObject.soundClip != null)
			{
				if(isFirstPerson)
				{
					firstPersonCamera.gameObject.SetActive(false);
					transform.gameObject.GetComponent<MovementController>().canMove = true;
					camInput.enabled = true;
					isInteractMode = false;
				}
			}
		}
		if (isInteractMode && Input.GetKeyDown(KeyCode.E))
		{
			if (SubtitleManager.Instance.isPlaying){
				SubtitleManager.Instance.Stop();
			}
			if (interactedObject != null && !interactedObject.StoppedPlaying()) {
				interactedObject.StopAudio();
			}

			if(isFirstPerson)
			{
				firstPersonCamera.gameObject.SetActive(false);
			}
			transform.gameObject.GetComponent<MovementController>().canMove = true;
			camInput.enabled = true;
			isInteractMode = false;
		}
		else if(!isInteractMode)
		{
			if( interactedObject == null )
				return;

			if(Input.GetKeyDown(KeyCode.E))
			{
				if (SubtitleManager.Instance.isPlaying)
					SubtitleManager.Instance.Stop();

				if(interactedObject.nextLevel)
				{
					Application.LoadLevel(1);
					return;
				}

				firstPersonCamera.gameObject.SetActive(true);
				isFirstPerson = true;
				firstPersonCamera.LookAt(interactedObject.transform);
				transform.gameObject.GetComponent<MovementController>().canMove = false;
				camInput.enabled = false;

				isInteractMode = true;

				if(interactedObject.soundClip != null)
					interactedObject.PlayAudio();

				if(interactedObject.subtitlesToPlay != null && interactedObject.subtitlesToPlay.Length != 0)
					SubtitleManager.Instance.SendMessage(interactedObject.subtitlesToPlay);

			}
		}
	}
}
