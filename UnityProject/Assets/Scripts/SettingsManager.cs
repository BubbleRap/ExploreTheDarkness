using UnityEngine;
using System.Collections;

public class SettingsManager : MonoBehaviour 
{
	void Awake()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
}
