using UnityEngine;
using System.Collections;

public class AudioInteractionObject : IInteractableObject 
{
	public AudioClip soundClip; 
	[Range(0f,1f)]
	public float volume = 1f;

	public bool internalPlay;
	public string subtitlesToPlay;

	private AudioSource soundSource;

	void Start()
	{
		if(internalPlay)
			soundSource = transform.gameObject.GetComponent<AudioSource>();
		else
			soundSource = Camera.main.gameObject.GetComponent<AudioSource>();

		if( soundSource == null )
			Debug.LogError("Look, I think there must be an audio source somewhere here: " + gameObject.name);
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
			{
				PlayOneShot(soundClip, volume);
				Debug.Log("Audio clip " + soundClip.name + " has started on " + soundSource.gameObject.name);
			}
			else
			{
				Debug.LogError("You forgot to attach the goddamn audio clip!");
			}
			
			if( !string.IsNullOrEmpty( subtitlesToPlay ) )
				SubtitleManager.Instance.SendMessage(subtitlesToPlay);

			StartCoroutine( DisactivateOnStopPlaying() );
		}
		// EXIT FROM INTERACTION
		else
		{
			SubtitleManager.Instance.Stop();
			soundSource.Stop();
			StopAllCoroutines();
		}
	}

	private void PlayOneShot(AudioClip clip, float volume)
	{
		soundSource.clip = clip;
		soundSource.volume = volume;
		soundSource.Play();
	}

	private IEnumerator DisactivateOnStopPlaying()
	{
		if(! soundSource.isPlaying )
		{
			Debug.Log("Nooo, something is wrong with the " + soundSource.gameObject.name);
			yield break;
		}

		while( soundSource.isPlaying )
			yield return null;

		// disactivate all the interaction components
		foreach( MonoBehaviour behaviour in gameObject.GetComponents<MonoBehaviour>() )
		{
			IInteractableObject interactableInterface = behaviour as IInteractableObject;
			if( interactableInterface != null )
				interactableInterface.Activate();
		}
	}
}
