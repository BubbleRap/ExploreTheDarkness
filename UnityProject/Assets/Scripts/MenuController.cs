﻿using UnityEngine;
using System.Collections.Generic;

public class MenuController : MonoBehaviour {

	public static bool canToggleMenu = false;
	public bool menuOn = false;

	public Camera menuCamera;

	public List<MenuOption> options;
	public int currentOption = 0;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) && canToggleMenu) {
			toggleMenu();
		}

		if (menuOn) {
			if (Input.GetKeyDown(KeyCode.DownArrow)){
				changeOption(true);
			}
			if (Input.GetKeyDown(KeyCode.UpArrow)){
				changeOption(false);
			}
			if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)){
				confirm();
			}
		}
	}

	private void toggleMenu(){
		menuOn = !menuOn;
		menuCamera.enabled = !menuCamera.enabled;

		//TODO freeze time!
	}

	private void changeOption(bool forward){
		options [currentOption].ToggleHighlight ();

		if (forward) {
			currentOption++;
			if (currentOption == options.Count){
				currentOption = 0;
			}
		} else {
			currentOption--;
			if (currentOption == -1){
				currentOption = options.Count -1;
			}
		}

		options [currentOption].ToggleHighlight ();
	}

	private void confirm(){
		Invoke (options [currentOption].action,0f);
	}

	public void Resume(){
		toggleMenu ();

		options [currentOption].ToggleHighlight ();
		currentOption = 0;
		options [currentOption].ToggleHighlight ();
	}

	public void Restart(){
		Application.LoadLevel(0);
	}

	public void Quit(){
		Application.Quit ();
	}
}
