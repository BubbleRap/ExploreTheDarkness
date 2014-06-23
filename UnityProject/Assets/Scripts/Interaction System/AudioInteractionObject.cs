using UnityEngine;
using System.Collections;

public class AudioInteractionObject : IInteractableObject 
{
	public AudioClip soundClip; 
	[Range(0f,1f)]
	public float volume = 1f;

	public bool internalPlay;
	public string subtitlesToPlay;

	private AudioSource audioSource;

	void Start()
	{
		if(internalPlay)
			audioSource = transform.gameObject.GetComponent<AudioSource>();
		else
			audioSource = Camera.main.gameObject.GetComponent<AudioSource>();


		if( audioSource == null )
			Debug.LogError("Look, I think there must be an audio source somewhere here.");
	}

	// called by Interactor.cs
	public override void Activate()
	{
		interactionIsActive = !interactionIsActive;

		// START INTERACTION
		if( interactionIsActive )
		{
			SubtitleManager.Instance.Stop();

			if( soundClip != null )
				audioSource.PlayOneShot(soundClip, volume);
			else
				Debug.LogError("You forgot to attach the goddamit audio clip!");
			
			if( !string.IsNullOrEmpty( subtitlesToPlay ) )
				SubtitleManager.Instance.SendMessage(subtitlesToPlay);
		}
		// EXIT FROM INTERACTION
		else
		{
			SubtitleManager.Instance.Stop();
			audioSource.Stop();
		}
	}
}
