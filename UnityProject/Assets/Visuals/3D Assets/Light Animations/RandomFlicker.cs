using UnityEngine;
using System.Collections;

public class RandomFlicker : MonoBehaviour {

	private Animation thisAnimation;
	float randomizer;

	// Use this for initialization
	void Start () 
	{
		thisAnimation = this.gameObject.GetComponent<Animation>();
		StartCoroutine("FlickerTimer");
	}
	
	IEnumerator FlickerTimer()
	{
		while(true)
		{
		thisAnimation.Play();
		randomizer = Random.Range(1.0f, 5.0f);
		yield return new WaitForSeconds(randomizer);
		}
		yield return null;
	}

}
