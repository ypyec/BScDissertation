using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class PauseScript : MonoBehaviour {

	public Button continueText;
	public Button exitText;
	public static bool paused;

	private int index;
	private float timer;
	Canvas pauseMenu;


	// Use this for initialization
	void Start () {
		index = 0;
		timer = 0.5f;
		pauseMenu = this.GetComponent<Canvas> ();
		continueText = continueText.GetComponent<Button> ();
		exitText = exitText.GetComponent<Button> ();
		pauseMenu.enabled = false;
		paused = false;
	}

	void Update() {
		timer += Time.unscaledDeltaTime;
		if (Input.GetButton ("Cancel")) {
			if (!paused) {
				pauseMenu.enabled = paused = true;
				Time.timeScale = 0;
				PauseSelect (index);
			}
		}
		if (pauseMenu.enabled) {
			if (Input.GetAxisRaw ("Vertical") == -1 && timer >= 0.3f) {
				if (index == 1)
					index = 0;
				else
					index++;
				PauseSelect (index);
			} else if (Input.GetAxisRaw ("Vertical") == 1 && timer >= 0.3f) {
				if (index == 0)
					index = 1;
				else
					index--;
				PauseSelect (index);
			} 
		}
	}

	private void PauseSelect(int MenuIndex) {
		switch (MenuIndex) {
		case 0:
			continueText.Select ();
			timer = 0f;
			break;
		case 1:
			exitText.Select ();
			timer = 0f;
			break;
		}
	}

	public void Continue() {
		pauseMenu.enabled = paused = false;
		Time.timeScale = 1;
	}

	public void ExitGame() {
		Time.timeScale = 1;
		SceneManager.LoadScene ("Main Menu");
	}
}
