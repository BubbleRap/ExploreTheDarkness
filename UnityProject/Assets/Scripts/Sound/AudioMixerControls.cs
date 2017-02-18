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
		ContextBasedListRandom,
		ContextBasedListOrdered
    }

	[Serializable]
	public class AudioListContainer
	{
		public List<AudioClip> m_clips;
		public AudioSource m_outputSource;
	}

    public MixingType m_mixingType;

    public float m_volume = 1f;

	public float m_minValue = -20f;
	public float m_maxValue = 80f;
	public string m_paramName = "My Exposed Parameter";


	public float m_time = 1f;

	public AudioMixer m_audioMixer;
	//public AudioSource m_audioSource;
	public List<AudioListContainer> m_audioContainer;


	public List<AudioMixerSnapshot> m_snapshots;

	private int m_audioIndex;
	private int m_stateIndex;

	void Awake()
	{
        //if( m_mixingType == MixingType.PlayOneShot )
        //{
        //    if( m_audioSource == null )
        //        Debug.LogError( gameObject.name + " AudioSource is not assigned" ); 
        //}

        if( m_mixingType == MixingType.FadeInOutParameter )
        {
		    if( m_audioMixer == null )
			    Debug.LogError( gameObject.name + " AudioMixer is not assigned" );
            //if( m_audioSource == null )
            //    Debug.LogError( gameObject.name + " AudioSource is not assigned" ); 
        }

        //if( m_mixingType == MixingType.FadeToSnapshot )
        //{
        //    if( !string.IsNullOrEmpty(m_snapshotName) )
        //        m_snapshot = m_audioMixer.FindSnapshot(m_snapshotName);
        //    else
        //        Debug.LogError( gameObject.name + " Snapshot name isn't assigned" );
        //}
	}

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

		if(m_mixingType == MixingType.ContextBasedListRandom)
		{	
			m_audioIndex = UnityEngine.Random.Range(0,m_audioContainer[m_stateIndex].m_clips.Count);
		}

		if(m_audioContainer[m_stateIndex].m_clips.Count <= 0)
			return;



		var audioSource = m_audioContainer[m_stateIndex].m_outputSource;
		var audioClip = m_audioContainer[m_stateIndex].m_clips[m_audioIndex];

		audioSource.clip = audioClip;

        StopAllCoroutines();
		StartCoroutine(PlayOneShot(audioSource));

		m_audioContainer[m_stateIndex].m_clips.Remove(m_audioContainer[m_stateIndex].m_clips[m_audioIndex]);
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

	private IEnumerator PlayOneShot(AudioSource source)
    {
		if( source != null )
			source.enabled = true;
		
		source.volume = m_volume;
		source.Play();

		yield return new WaitForSeconds(source.clip.length);

		source.Stop();
		if( source != null )
			source.enabled = false;
    }
}
