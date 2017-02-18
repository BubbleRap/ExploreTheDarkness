using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioMixerControls : MonoBehaviour 
{
    public enum MixingType
    {
		PlayOneShot,
        FadeInOutParameter,
        FadeToSnapshot,
    }

	[Serializable]
	public class AudioListContainer
	{
		public List<AudioClip> m_clips;
	}

    public MixingType m_mixingType;

    public float m_volume = 1f;

	public float m_minValue = -20f;
	public float m_maxValue = 80f;
	public string m_paramName = "My Exposed Parameter";


	public float m_time = 1f;

	public bool m_loopLastClip;
	public bool m_playClipsRandomly;
	public AudioMixer m_audioMixer;
	public AudioSource m_audioSource;
	public List<AudioListContainer> m_audioContainer;
	public List<AudioMixerSnapshot> m_snapshots;

	private int m_audioIndex;
	private int m_stateIndex;

	public void FadeInAudio()
	{
		StopAllCoroutines();
		StartCoroutine(FadeIn());
	}

	public void FadeOutAudio()
	{
		StopAllCoroutines();
		StartCoroutine(FadeOut());
	}

    public void PlayAudioSource()
    {
		if( m_stateIndex >= m_audioContainer.Count)
			return;

		var clips = m_audioContainer[m_stateIndex].m_clips;

		if(m_playClipsRandomly)
		{	
			m_audioIndex = UnityEngine.Random.Range(0, clips.Count);
		}

		if(clips.Count <= 0)
			return;
		
		var audioClip = clips[m_audioIndex];

		m_audioSource.clip = audioClip;

        StopAllCoroutines();
		StartCoroutine(PlayOneShot());
		
		bool keep = m_loopLastClip && clips.Count <= 1;

		if(!keep) 
			clips.RemoveAt(m_audioIndex);
    }

    public void FadeToSnapshot()
    {
		foreach(var snapshot in m_snapshots)
			snapshot.TransitionTo(m_time);   
    }

	private IEnumerator FadeIn()
	{
		m_audioMixer.SetFloat(m_paramName, m_minValue);
		float timer = 0f;

		while( timer < m_time )
		{
			timer += Time.deltaTime;
			m_audioMixer.SetFloat(m_paramName, (timer / m_time) * (m_maxValue - m_minValue) + m_minValue);
			yield return null;
		}

		m_audioMixer.SetFloat(m_paramName, m_maxValue);
	}
	
	private IEnumerator FadeOut()
	{
		m_audioMixer.SetFloat(m_paramName, m_maxValue);
		float timer = 0f;
		
		while( timer < m_time )
		{
			timer += Time.deltaTime;
			m_audioMixer.SetFloat(m_paramName, (1f - timer / m_time) * (m_maxValue - m_minValue) + m_minValue);
			yield return null;
		}

		m_audioMixer.SetFloat(m_paramName, m_minValue);
	}

	private IEnumerator PlayOneShot()
    {
		if( m_audioSource != null )
			m_audioSource.enabled = true;
		
		m_audioSource.volume = m_volume;
		m_audioSource.Play();

		yield return new WaitForSeconds(m_audioSource.clip.length);

		m_audioSource.Stop();
		if( m_audioSource != null )
			m_audioSource.enabled = false;
    }
}
