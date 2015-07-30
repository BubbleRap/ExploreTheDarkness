using UnityEngine;
using UnityEngine.Events;

public class GenericTrigger : MonoBehaviour 
{
	public UnityEvent m_onTriggerEnter;
	public UnityEvent m_onTriggerExit;

	public string m_tagName = "";

	void OnTriggerEnter(Collider other) {
		if(other.tag == m_tagName || string.IsNullOrEmpty(m_tagName))
			m_onTriggerEnter.Invoke();
		
	}
	
	void OnTriggerExit(Collider other) {
		if(other.tag == m_tagName || string.IsNullOrEmpty(m_tagName))
			m_onTriggerExit.Invoke();
	}
}
