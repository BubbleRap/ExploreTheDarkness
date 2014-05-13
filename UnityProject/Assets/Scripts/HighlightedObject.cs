using UnityEngine;
using System.Collections;

public class HighlightedObject : MonoBehaviour {
	
	public bool firstperson;
	public AudioClip soundClip; 
	private AudioSource audioSource;
	[Multiline]
	public string subtitlesToPlay;
	[HideInInspector]
	public bool internalPlay;
	public bool hitObject = false;
	public bool nextLevel = false;
	public bool transitionToDarkness = false;

	public Transform lookFromPoint = null;

 	private GameObject buttonPrompt;

	private bool activated = false;

	private interact interactionScript = null;
	private float originalAlpha = 1f;

	void Start () {

		if(internalPlay)
		{
			audioSource = transform.gameObject.GetComponent<AudioSource>();
		}
		else
		{
			audioSource = Camera.main.gameObject.GetComponent<AudioSource>();
		}

		buttonPrompt = Instantiate(Resources.Load<GameObject>("buttonPrompt")) as GameObject;
		buttonPrompt.GetComponent<ButtonPrompt> ().highlightedObject = this;
		buttonPrompt.SetActive(false);

		interactionScript = FindObjectOfType(typeof(interact)) as interact;

		Color defaultColor = renderer.material.color;
		originalAlpha = defaultColor.a;
		defaultColor.a = 0f;
		renderer.material.color = defaultColor;
	}

	void Update () {

		if( interactionScript.isInteractMode )
		{
			ActivateHighlights(false);
			ActivatePromtButton(false);
			return;
		}

		if( !renderer.isVisible )
			return;

		Vector3 cameraRelativePosition = Camera.main.transform.InverseTransformPoint(transform.position);
		if( cameraRelativePosition.x < 0.8f * cameraRelativePosition.z && cameraRelativePosition.x > -0.8f * cameraRelativePosition.z
		   && cameraRelativePosition.z < 5f)
			hitObject = true;
		else
			hitObject = false;

		ActivateHighlights(hitObject);
		ActivatePromtButton((transform.position - Camera.main.transform.position).magnitude < 3f && activated);

		hitObject = false;

		if( !buttonPrompt.activeInHierarchy )
			return;

		Vector3 direction = ((transform.position - Vector3.up * 1.5f) - Camera.main.transform.position).normalized;
		buttonPrompt.transform.position = transform.position - direction * 0.25f;
	}

	public void PlayAudio () {
		audioSource.clip = soundClip;
		audioSource.Play();
	}

	public void StopAudio () {
		audioSource.Stop();
	}

	public bool StoppedPlaying () {
		if(!audioSource.isPlaying)
		{
			return true;
		}
		return false;
	}

	private void ActivateHighlights( bool state )
	{
		if( activated == state )
			return;

		Color curColor = renderer.material.color;

		curColor.a = state ? originalAlpha : 0f;

		renderer.material.color = curColor;
			
		activated = state;

	}

	private void ActivatePromtButton( bool state )
	{
		if( buttonPrompt.activeInHierarchy == state )
			return;

		if( state )
		{
			buttonPrompt.transform.LookAt(Camera.main.gameObject.transform);		
			interactionScript.interactedObject = this;
		}

		buttonPrompt.SetActive(state);
	}
}
