using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioMixerControls : MonoBehaviour 
{
	public float m_minValue = -20f;
	public float m_maxValue = 80f;
	public string m_paramName = "Master";
	public float m_time = 1f;

	public AudioMixer m_audioMixer;


	void Awake()
	{
		if( m_audioMixer == null )
			Debug.LogError( gameObject.name + " AudioMixer is not assigned" );
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

	private IEnumerator FadeIn( )
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
	
	private IEnumerator FadeOut( )
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
}
