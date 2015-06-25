using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioRaycaster : MonoBehaviour 
{
	public string m_raycastTag;

	[Tooltip("How many frames to skip. 0 - default")]
	public int m_raycastFPSSkip = 0;

	[Range(0f, 1f)]
	public float m_modulateValueMultiplier = 0.33f;
	
	private Coroutine raycasting;

	private List<AudioSource> m_audioSources = new List<AudioSource>();
	private List<float> m_originalVolumes = new List<float>();

	void Start()
	{
		GameObject[] sources = GameObject.FindGameObjectsWithTag(m_raycastTag);

		foreach( GameObject sourceGO in sources )
		{
			AudioSource aSource = sourceGO.GetComponent<AudioSource>();
			if( aSource == null )
				continue;

			m_audioSources.Add(aSource);
			m_originalVolumes.Add(aSource.volume);
		}

		raycasting = StartCoroutine(RaycastingAudio());
	}

	private IEnumerator RaycastingAudio()
	{
		while( true )
		{
			for( int i = 0; i < m_audioSources.Count; i++ )
			{
				Ray ray = new Ray(transform.position, (m_audioSources[i].transform.position - transform.position).normalized);
				float distance = (transform.position - m_audioSources[i].transform.position).magnitude;
				LayerMask mask = 0;

				// Raycasting "Default" layer, which mostly means walls and furniture
				RaycastHit[] outs = Physics.RaycastAll( ray, distance );
				m_audioSources[i].volume = Mathf.Clamp(m_originalVolumes[i] - outs.Length * m_modulateValueMultiplier * m_originalVolumes[i], 0f, m_originalVolumes[i]);
			}

			for( int i = 0; i < m_raycastFPSSkip; i++ )
				yield return null;

			yield return null;
		}
	}
}
