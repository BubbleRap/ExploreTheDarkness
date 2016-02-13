using UnityEngine;
using System.Collections;


// TO BE refactored due to the new Audio Mixer system
public class CharacterAudio : MonoBehaviour 
{
	public enum SoundLocation
	{
		Silja_Camera,
		Silja_Head,
		Silja_Feet
	}


    public AudioSource heartBeatAudioSource;
    public AudioClip heartBeatClip;
    public AudioSource breathingAudioSource;
    public AudioClip breathingClip;
    public AudioSource monsterAudioSource;
    public AudioClip monsterSearching;
    public AudioClip monsterChase;

    public AudioSource siljaOnScaredAudio;
    public AudioSource audioRunTension;
    public AudioSource flashlightTurnAudio;

    public AudioClip[] monsterCatchSiljaSound;

    private float volume1 = 1.0f;
    private float volume2 = 1.0f;
    private float volume3 = 1.0f;


    public float MonsterAudioVolume
    {
        get
        {
            return volume3;
        }
    }



    private int randomNumber = 0;
	private int lastNumber = 0;
	public SoundLocation location = SoundLocation.Silja_Camera;
	public AudioSource[] audioSources;

	public AudioClip[] stepSounds;
	[Range(0f,1f)]
	public float volume = 1f;

	public void OnStep()
	{
		while(randomNumber == lastNumber)
		{
			randomNumber = Random.Range(0, stepSounds.Length);
		}

		audioSources [(int)location].PlayOneShot (stepSounds[randomNumber], volume);
		lastNumber = randomNumber;
	}

    public void PlayScaredLoop()
    {
        if (siljaOnScaredAudio != null)
            siljaOnScaredAudio.Play();
    }

    public void PlayTensionLoop()
    {
        if (audioRunTension != null)
            audioRunTension.Play();
    }

    public void PlayFlashlightSound()
    {
        if (flashlightTurnAudio != null)
            flashlightTurnAudio.Play();
    }

    public void PlayHeartbeatLoop()
    {
        if (heartBeatAudioSource != null && !heartBeatAudioSource.isPlaying)
            heartBeatAudioSource.Play();
    }
    
    public void SetHeartbeatVolume(float volume)
    {
        volume1 = volume;

        if (heartBeatAudioSource != null)
            heartBeatAudioSource.volume = volume1;
    }

    public void PlayBreathingLoop()
    {
        if (breathingAudioSource != null && !breathingAudioSource.isPlaying)
            breathingAudioSource.Play();
    }

    public void SetBreathingVolume(float volume)
    {
        volume2 = volume;

        if (breathingAudioSource != null)
            breathingAudioSource.volume = volume2;
    }

    public void PlayMonsterLoop()
    {
        if (monsterAudioSource != null && !monsterAudioSource.isPlaying)
            monsterAudioSource.Play();
    }

    public void SetMonsterVolume(float volume)
    {
        volume3 = volume;

        if (monsterAudioSource != null)
            monsterAudioSource.volume = volume3;
    }

    public void SetMonsterAudioClip( AudioClip clip )
    {
        if(monsterAudioSource != null)
            monsterAudioSource.clip = clip;
    }

    public void SetMonsterAudioChase()
    {
        SetMonsterAudioClip(monsterChase);
    }

    public void SetMonsterAudioSearch()
    {
        SetMonsterAudioClip(monsterSearching);
    }

    public void PlaySiljaCaughtRandomSound()
    {
				AudioClip sound = null;

				if( monsterCatchSiljaSound.Length > 0 )
						sound = monsterCatchSiljaSound[Random.Range(0, monsterCatchSiljaSound.Length - 1)];
						
				
				if( sound != null )
        PlaySiljaCaughtSound(sound);
						else
								Debug.LogError("No monster caught silja sounds assigned.");
    }

    public void PlaySiljaCaughtSound(AudioClip clip)
    {
        AudioSource soundSource = GameObject.Find("HeadAudioSource").transform.GetComponent<AudioSource>();

        if (!soundSource.isPlaying)
        {
            soundSource.clip = clip;
            soundSource.Play();
        }
    }
}
