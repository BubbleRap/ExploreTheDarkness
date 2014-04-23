using UnityEngine;
using System.Collections;

public class SoundRandomizer : MonoBehaviour 
{
	public AudioSource theSource;
	public AudioClip[] randomSounds;

	[Range(0f,1f)]
	public float volume = 1f;

	private bool activated = false;
	
	void Start () 
	{
		if( theSource == null )
			theSource = GetComponent<AudioSource>();

		// if it is still null then I am sorry
		if( theSource == null )
		{	
			Debug.LogError("No audio sources found");
			return;
		}

		activated = true;
	}

	void Update()
	{
		if( !activated )
			return;

		if( !theSource.isPlaying )
			theSource.PlayOneShot (randomSounds[Random.Range(0, randomSounds.Length)], volume);
	}
}
