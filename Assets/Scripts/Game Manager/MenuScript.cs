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
	public Button yesText;
	public Button noText;
	public Button easyText;
	public Button mediumText;
	public Button hardText;

	private int index;
	private float timer;

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt ("Current Level", 0);
		index = 0;
		timer = 0.5f;
		PlayerPrefs.SetInt ("Difficulty", 2);
		difficultyMenu = difficultyMenu.GetComponent<Canvas> ();
		quitMenu = quitMenu.GetComponent<Canvas> ();
		continueText = continueText.GetComponent<Button> ();
		campaignText = campaignText.GetComponent<Button> ();
		arcadeText = arcadeText.GetComponent<Button> ();
		difficultyText = difficultyText.GetComponent<Button> ();
		exitText = exitText.GetComponent<Button> ();
		yesText = yesText.GetComponent<Button> ();
		noText = noText.GetComponent<Button> ();
		easyText = easyText.GetComponent<Button> ();
		mediumText = mediumText.GetComponent<Button> ();
		hardText = hardText.GetComponent<Button> ();
		difficultyMenu.enabled = false;
		quitMenu.enabled = false;
		arcadeText.Select ();

		if (PlayerPrefs.GetInt ("Current Level") > 0) {
			continueText.interactable = true;
		} else {
			continueText.interactable = false;
		}
	}

	void Update() {
		timer += Time.deltaTime;
		if (!quitMenu.enabled && !difficultyMenu.enabled) {
			if (Input.GetAxisRaw ("Vertical") == -1 && timer >= 0.3f) {
				if (index == 4)
					index = 0;
				else
					index++;
				MenuSelect (index, "down");
			} else if (Input.GetAxisRaw ("Vertical") == 1 && timer >= 0.3f) {
				if (index == 0)
					index = 4;
				else
					index--;
				MenuSelect (index, "up");
			} 
		} else if (quitMenu.enabled && !difficultyMenu.enabled) {
			if (Input.GetAxisRaw ("Vertical") == -1 && timer >= 0.3f) {
				if (index >= 1)
					index = 0;
				else
					index++;
				quitMenuSelect (index);
			} else if (Input.GetAxisRaw ("Vertical") == 1 && timer >= 0.3f) {
				if (index == 0)
					index = 1;
				else
					index--;
				quitMenuSelect (index);
			} 
		} else if (!quitMenu.enabled && difficultyMenu.enabled) {
			if (Input.GetAxisRaw ("Vertical") == -1 && timer >= 0.3f) {
				if (index >= 2)
					index = 0;
				else
					index++;
				difficultyMenuSelect (index);
			} else if (Input.GetAxisRaw ("Vertical") == 1 && timer >= 0.3f) {
				if (index == 0)
					index = 2;
				else
					index--;
				difficultyMenuSelect (index);
			}
		}
			
	}

	void MenuSelect(int menuIndex, string direction) {
		if (index == 1 && !continueText.interactable && direction == "down") {
			index++;
			menuIndex++;
		} else if (index == 1 && !continueText.interactable && direction == "up") {
			index--;
			menuIndex--;
		}
		switch (menuIndex) {
		case 0:
			arcadeText.Select ();
			break;
		case 1:
			continueText.Select ();
			break;
		case 2:
			campaignText.Select ();
			break;
		case 3:
			difficultyText.Select ();
			break;
		case 4:
			exitText.Select ();
			break;
		}
		timer = 0f;
	}

	void quitMenuSelect(int menuIndex) {
		switch (menuIndex) {
		case 0:
			yesText.Select ();
			break;
		case 1:
			noText.Select ();
			break;
		}
		timer = 0f;
	}

	void difficultyMenuSelect(int menuIndex) {
		switch (menuIndex) {
		case 0:
			easyText.Select ();
			break;
		case 1:
			mediumText.Select ();
			break;
		case 2:
			hardText.Select ();
			break;
		}
		timer = 0f;
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
