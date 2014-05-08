using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour {

	public static bool canQuit = false;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) && canQuit) {
			Application.Quit();
		}
	}
}
