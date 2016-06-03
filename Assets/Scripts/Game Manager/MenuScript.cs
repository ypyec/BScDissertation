using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class MenuScript : MonoBehaviour {

	public Canvas difficultyMenu;
	public Canvas quitMenu;
	public Button continueText;
	public Button campaignText;
	public Button arcadeText;
	public Button difficultyText;
	public Button exitText;

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt ("Difficulty", 2);
		difficultyMenu = difficultyMenu.GetComponent<Canvas> ();
		quitMenu = quitMenu.GetComponent<Canvas> ();
		continueText = continueText.GetComponent<Button> ();
		campaignText = campaignText.GetComponent<Button> ();
		arcadeText = arcadeText.GetComponent<Button> ();
		difficultyText = difficultyText.GetComponent<Button> ();
		exitText = exitText.GetComponent<Button> ();
		difficultyMenu.enabled = false;
		quitMenu.enabled = false;

		if (PlayerPrefs.GetInt ("Current Level") > 0) {
			continueText.interactable = true;
		} else {
			continueText.interactable = false;
		}
	}

	public void DifficultyPress() {
		difficultyMenu.enabled = true;
		difficultyText.enabled = false;
		continueText.enabled = false;
		campaignText.enabled = false;
		arcadeText.enabled = false;
		exitText.enabled = false;
	}

	public void ExitPress() {
		difficultyText.enabled = false;
		quitMenu.enabled = true;
		continueText.enabled = false;
		campaignText.enabled = false;
		arcadeText.enabled = false;
		exitText.enabled = false;
	}

	public void NoPress() {
		difficultyText.enabled = true;
		quitMenu.enabled = false;
		campaignText.enabled = true;
		arcadeText.enabled = true;
		exitText.enabled = true;
		continueText.enabled = true;
	}

	public void EasyPress() {
		PlayerPrefs.SetInt ("Difficulty", 1);
		difficultyText.enabled = true;
		difficultyMenu.enabled = false;
		campaignText.enabled = true;
		arcadeText.enabled = true;
		exitText.enabled = true;
		continueText.enabled = true;
	}

	public void MediumPress() {
		PlayerPrefs.SetInt ("Difficulty", 2);
		difficultyText.enabled = true;
		difficultyMenu.enabled = false;
		campaignText.enabled = true;
		arcadeText.enabled = true;
		exitText.enabled = true;
		continueText.enabled = true;
	}

	public void HardPress() {
		PlayerPrefs.SetInt ("Difficulty", 3);
		difficultyText.enabled = true;
		difficultyMenu.enabled = false;
		campaignText.enabled = true;
		arcadeText.enabled = true;
		exitText.enabled = true;
		continueText.enabled = true;
	}

	public void Continue() {
		if (Application.CanStreamedLevelBeLoaded ("Level-" + PlayerPrefs.GetInt ("Current Level"))) {
			SceneManager.LoadScene ("Level-" + PlayerPrefs.GetInt ("Current Level"));
		}
	}

	public void StartCampaign() {
		SceneManager.LoadScene ("Level-0");
	}

	public void StartArcade() {
		SceneManager.LoadScene ("Arcade");
	}

	public void ExitGame() {
		Application.Quit ();
	}
}
