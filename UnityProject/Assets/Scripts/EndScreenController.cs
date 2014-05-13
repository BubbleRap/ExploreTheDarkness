using UnityEngine;
using System.Collections;

public class EndScreenController : MonoBehaviour {

	public bool EndScreenActive = false;
	public Camera EndScreenCamera;

	public GameObject VictorScreen;
	public GameObject GameOverScreen;
	
	// Update is called once per frame
	void Update () {
		
		if (EndScreenActive) {
			if (Input.GetKeyDown(KeyCode.E)){
				(GameObject.FindObjectOfType<MenuController> () as MenuController).Restart ();
			}
		}
	}
	
	public void ShowEndScreen(bool victor){
		EndScreenActive = true;
		EndScreenCamera.enabled = true;

		if (victor)
			VictorScreen.SetActive(true);
		else
			GameOverScreen.SetActive(true);

		Time.timeScale = 0.0001f;
		foreach (AudioSource audio in GameObject.FindObjectsOfType<AudioSource>()){
			if (audio.time != 0f){
				audio.Pause();
			}
		}
	}
}
