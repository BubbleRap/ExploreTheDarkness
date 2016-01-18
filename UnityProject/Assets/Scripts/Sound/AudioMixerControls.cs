using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioMixerControls : MonoBehaviour 
{
    public enum MixingType
    {
        PlayOneShot,
        FadeInOutParameter,
        FadeToSnapshot
    }

    public MixingType m_mixingType;

    public float m_volume = 1f;

	public float m_minValue = -20f;
	public float m_maxValue = 80f;
	public string m_paramName = "My Exposed Parameter";

    public string m_snapshotName = "My Snapshot";
   

	public float m_time = 1f;

	public AudioMixer m_audioMixer;
	public AudioSource m_audioSource;

    private AudioMixerSnapshot m_snapshot = null;

	void Awake()
	{
        if( m_mixingType == MixingType.PlayOneShot )
        {
            if( m_audioSource == null )
                Debug.LogError( gameObject.name + " AudioSource is not assigned" ); 
        }

        if( m_mixingType == MixingType.FadeInOutParameter )
        {
		    if( m_audioMixer == null )
			    Debug.LogError( gameObject.name + " AudioMixer is not assigned" );
            if( m_audioSource == null )
                Debug.LogError( gameObject.name + " AudioSource is not assigned" ); 
        }

        if( m_mixingType == MixingType.FadeToSnapshot )
        {
            if( !string.IsNullOrEmpty(m_snapshotName) )
                m_snapshot = m_audioMixer.FindSnapshot(m_snapshotName);
            else
                Debug.LogError( gameObject.name + " Snapshot name isn't assigned" );
        }
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
        if( m_audioSource != null )
            m_audioSource.enabled = true;

        StopAllCoroutines();
        StartCoroutine(PlayOneShot());
    }

    public void FadeToSnapshot()
    {
        m_snapshot.TransitionTo(m_time);   
    }

	private IEnumerator FadeIn()
	{
		if( m_audioSource != null )
		    m_audioSource.enabled = true;

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

		if( m_audioSource != null )
		    m_audioSource.enabled = false;
	}

    private IEnumerator PlayOneShot()
    {
        m_audioSource.volume = m_volume;
        m_audioSource.Play();

        yield return new WaitForSeconds(m_audioSource.clip.length);

        if( m_audioSource != null )
            m_audioSource.enabled = false;
    }
}
