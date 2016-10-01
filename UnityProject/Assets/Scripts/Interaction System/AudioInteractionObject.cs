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

	new void Start()
	{
        base.Start();

		if(internalPlay)
			soundSource = transform.gameObject.GetComponent<AudioSource>();
		else
			soundSource = Camera.main.gameObject.GetComponent<AudioSource>();

		if( soundSource == null )
			Debug.LogError("Look, I think there must be an audio source somewhere here: " + gameObject.name);
	}

	// called by Interactor.cs
	public override bool Activate()
	{
        base.Activate();

		// START INTERACTION
        if( IsInteracting )
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

		return false;
	}

	private void PlayOneShot(AudioClip clip, float volume)
	{
		soundSource.clip = clip;
		soundSource.volume = volume;
		soundSource.Play();
	}

	private IEnumerator DisactivateOnStopPlaying()
	{
        yield return new WaitForSeconds(0.1f);

		if(! soundSource.isPlaying )
		{
			Debug.Log("Nooo, something is wrong with the " + soundSource.gameObject.name);
			yield break;
		}

		while( soundSource.isPlaying )
			yield return null;

        // if there's a two-state interaction happening, don't disable the interaction.
        // (we don't wanna play the audio again when we're leaving the interaction)
        foreach (MonoBehaviour behaviour in gameObject.GetComponents<MonoBehaviour>())
        {
            IInteractableObject interactableInterface = behaviour as IInteractableObject;
            if (interactableInterface != null)
                if ((interactableInterface is LookInDetailInteraction ||
                    interactableInterface is LookAtInteractionObject ||
                    interactableInterface is PeepHoleInteraction) &&
                    interactableInterface.IsInteracting)
                    yield break;
        }

        if (IsInteracting)
            Activate();		
	}
}
