using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trigger_animation : MonoBehaviour {

	private Transitioner transitionContoller = null;

	public List<Transform> animationsToTrigger;
	public List<float> animationDelays;

	// Use this for initialization
	void Awake () {
		transitionContoller = Component.FindObjectOfType(typeof(Transitioner)) as Transitioner;
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player")
		{
			for (int i=0; i<animationsToTrigger.Count; ++i){
				StartCoroutine("DelayAndPlay",i);
			}

			collider.enabled = false;
		}
	}
	
	public IEnumerator DelayAndPlay(int index){
		if (animationDelays.Count > index)
			yield return new WaitForSeconds(animationDelays[index]);

		animationsToTrigger[index].animation.Play();

		if(animationsToTrigger[index].GetComponent<AudioSource>() != null)
			animationsToTrigger[index].GetComponent<AudioSource>().Play();
	}
}
