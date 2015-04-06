using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour {

	private GameObject siljaCharacter = null;
	public int health = 4;
	public SiljaBehaviour siljaBeh;

	private float barDisplay = 0;
	private Vector2 pos = new Vector2(Screen.width/2-400,Screen.height-50);
	private Vector2 size = new Vector2(800,20);
	private Texture2D progressBarEmpty;
	private Texture2D progressBarFull;

	private GUIStyle currentStyle = new GUIStyle();
	public Texture2D texture;

	private GUIStyle currentStyle2 = new GUIStyle();
	public Texture2D texture2;

	public List<AudioSource> playAllOfThose;
	public List<AudioSource> playOneOfThose;

	void Start () {
		siljaCharacter = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update () {
		barDisplay = siljaBeh.getTeddyLight();
	}

	public void PlayScaredAudio()
	{
		foreach (AudioSource s in playAllOfThose)
			if( !s.isPlaying )
				s.Play ();
	}	                         

	public void looseLife() {
		health --;

		foreach (AudioSource s in playAllOfThose)
			s.Play ();

		if (playOneOfThose.Count > 0 )
			playOneOfThose[Random.Range(0,playOneOfThose.Count-1)].Play();

		if(health <= 0)
		{
			StartCoroutine(DelayedDeathAction(2f));
		}
	}

	IEnumerator DelayedDeathAction(float time)
	{
		yield return new WaitForSeconds(time);

		(GameObject.FindObjectOfType<EndScreenController> () as EndScreenController).ShowEndScreen (false);
	}
}
