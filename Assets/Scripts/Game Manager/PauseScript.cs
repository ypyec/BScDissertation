using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class PauseScript : MonoBehaviour {

	public Button continueText;
	public Button exitText;
	public static bool paused;


	Canvas pauseMenu;


	// Use this for initialization
	void Start () {
		pauseMenu = this.GetComponent<Canvas> ();
		continueText = continueText.GetComponent<Button> ();
		exitText = exitText.GetComponent<Button> ();
		pauseMenu.enabled = false;
		paused = false;
	}

	void Update() {
		if (Input.GetButton ("Cancel")) {
			if (!paused) {
				pauseMenu.enabled = paused = true;
				Time.timeScale = 0;
			}
		}
	}


	public void NoPress() {
		exitText.enabled = true;
		continueText.enabled = true;
	}


	public void Continue() {
		pauseMenu.enabled = paused = false;
		Time.timeScale = 1;
	}

	public void ExitGame() {
		SceneManager.LoadScene ("Main Menu");
	}
}
