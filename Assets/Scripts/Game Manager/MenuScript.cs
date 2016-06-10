using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class MenuScript : MonoBehaviour {

	public Canvas missionSelect;
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
	public Button atrasText;

	public RectTransform[] levels;

	private int index;
	private int hIndex;
	private float timer;
	private Image splashScreen; 
	private Color color;

	// Use this for initialization
	void Start () {
		if(PlayerPrefs.GetInt("Difficulty") != null)
			PlayerPrefs.SetInt ("Difficulty", 2); 
		index = 0;
		hIndex = 0;
		timer = 0.25f;
		missionSelect = missionSelect.GetComponent<Canvas> ();
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
		atrasText = atrasText.GetComponent<Button> ();
		difficultyMenu.enabled = false;
		quitMenu.enabled = false;
		missionSelect.enabled = false;
		arcadeText.Select ();

		if (PlayerPrefs.GetInt ("Current Level") > 0) {
			continueText.interactable = true;
		} else {
			continueText.interactable = false;
		}

		splashScreen = GameObject.FindGameObjectWithTag("SplashScreen").GetComponent<Image> ();
		color = splashScreen.color;
		color.a = 0;
		splashScreen.color = color;
	}

	void Update() {
		timer += Time.deltaTime;
		if (!quitMenu.enabled && !difficultyMenu.enabled && !missionSelect.enabled) {
			if (Input.GetAxisRaw ("Vertical") == -1 && timer >= 0.25f) {
				if (index == 4)
					index = 0;
				else
					index++;
				MenuSelect (index, "down");
			} else if (Input.GetAxisRaw ("Vertical") == 1 && timer >= 0.25f) {
				if (index == 0)
					index = 4;
				else
					index--;
				MenuSelect (index, "up");
			} 
		} else if (quitMenu.enabled && !difficultyMenu.enabled  && !missionSelect.enabled) {
			if (Input.GetAxisRaw ("Vertical") == -1 && timer >= 0.25f) {
				if (index >= 1)
					index = 0;
				else
					index++;
				quitMenuSelect (index);
			} else if (Input.GetAxisRaw ("Vertical") == 1 && timer >= 0.25f) {
				if (index == 0)
					index = 1;
				else
					index--;
				quitMenuSelect (index);
			} 
		} else if (!quitMenu.enabled && difficultyMenu.enabled && !missionSelect.enabled) {
			if (Input.GetAxisRaw ("Vertical") == -1 && timer >= 0.25f) {
				if (index >= 2)
					index = 0;
				else
					index++;
				difficultyMenuSelect (index);
			} else if (Input.GetAxisRaw ("Vertical") == 1 && timer >= 0.25f) {
				if (index == 0)
					index = 2;
				else
					index--;
				difficultyMenuSelect (index);
			}
		} else if (!quitMenu.enabled && !difficultyMenu.enabled && missionSelect.enabled) {
			if (Input.GetAxisRaw ("Vertical") == -1 && timer >= 0.25f) {
				if (index >= 1)
					index = 0;
				else
					index++;
				missionMenuSelect (index, hIndex);
			} else if (Input.GetAxisRaw ("Vertical") == 1 && timer >= 0.25f) {
				if (index == 0)
					index = 1;
				else
					index--;
				missionMenuSelect (index, hIndex);
			} else if (Input.GetAxisRaw ("Horizontal") == -1 && timer >= 0.25f && index == 1) {
				if (hIndex <= 0)
					hIndex = PlayerPrefs.GetInt ("Current Level");
				else
					hIndex--;
				
				missionMenuSelect (index, hIndex);
			} else if (Input.GetAxisRaw ("Horizontal") == 1 && timer >= 0.25f  && index == 1) {
				if (hIndex >= PlayerPrefs.GetInt ("Current Level"))
					hIndex = 0;
				else
					hIndex++;
				missionMenuSelect (index, hIndex);
			}
		}
			
	}

	void MenuSelect(int menuIndex, string direction) {
		this.GetComponents<AudioSource> ()[0].Play ();
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
		this.GetComponents<AudioSource> ()[0].Play ();
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
		this.GetComponents<AudioSource> ()[0].Play ();
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

	void missionMenuSelect(int menuIndex, int missionIndex) {
		this.GetComponents<AudioSource> ()[0].Play ();
		switch (menuIndex) {
		case 0:
			atrasText.Select ();
			break;
		case 1:
			for (int i = 0; i < levels.Length; i++) {
				if (i == missionIndex) {
					levels [i].gameObject.SetActive (true);
					levels [i].GetComponent <Button> ().Select ();
				} else {
					levels [i].gameObject.SetActive (false);
				}
			}
			break;
		}
		timer = 0f;
	}
		
	public void DifficultyPress() {
		this.GetComponents<AudioSource> ()[1].Play ();
		difficultyMenu.enabled = true;
		this.GetComponent<Canvas> ().sortingOrder = -1;
		difficultyText.enabled = false;
		continueText.enabled = false;
		campaignText.enabled = false;
		arcadeText.enabled = false;
		exitText.enabled = false;
	}

	public void ExitPress() {
		this.GetComponents<AudioSource> ()[1].Play ();
		difficultyText.enabled = false;
		quitMenu.enabled = true;
		this.GetComponent<Canvas> ().sortingOrder = -1;
		continueText.enabled = false;
		campaignText.enabled = false;
		arcadeText.enabled = false;
		exitText.enabled = false;
	}

	public void NoPress() {
		this.GetComponents<AudioSource> ()[1].Play ();
		difficultyText.enabled = true;
		quitMenu.enabled = false;
		missionSelect.enabled = false;
		this.GetComponent<Canvas> ().sortingOrder = 1;
		campaignText.enabled = true;
		arcadeText.enabled = true;
		exitText.enabled = true;
		continueText.enabled = true;
	}

	public void EasyPress() {
		this.GetComponents<AudioSource> ()[1].Play ();
		PlayerPrefs.SetInt ("Difficulty", 1);
		this.GetComponent<Canvas> ().sortingOrder = 1;
		difficultyText.enabled = true;
		difficultyMenu.enabled = false;
		campaignText.enabled = true;
		arcadeText.enabled = true;
		exitText.enabled = true;
		continueText.enabled = true;
	}

	public void MediumPress() {
		this.GetComponents<AudioSource> ()[1].Play ();
		PlayerPrefs.SetInt ("Difficulty", 2);
		this.GetComponent<Canvas> ().sortingOrder = 1;
		difficultyText.enabled = true;
		difficultyMenu.enabled = false;
		campaignText.enabled = true;
		arcadeText.enabled = true;
		exitText.enabled = true;
		continueText.enabled = true;
	}

	public void HardPress() {
		this.GetComponents<AudioSource> ()[1].Play ();
		PlayerPrefs.SetInt ("Difficulty", 3);
		this.GetComponent<Canvas> ().sortingOrder = 1;
		difficultyText.enabled = true;
		difficultyMenu.enabled = false;
		campaignText.enabled = true;
		arcadeText.enabled = true;
		exitText.enabled = true;
		continueText.enabled = true;
	}

	public void Continue() {
		this.GetComponents<AudioSource> ()[1].Play ();
		if (Application.CanStreamedLevelBeLoaded ("Level-" + PlayerPrefs.GetInt ("Current Level"))) {
			color.a = 255;
			splashScreen.color = color;
			SceneManager.LoadScene ("Level-" + PlayerPrefs.GetInt ("Current Level"));
		}
	}

	public void StartCampaign() {
		this.GetComponents<AudioSource> ()[1].Play ();
		if (PlayerPrefs.GetInt ("Current Level") == 0) {
			color.a = 255;
			splashScreen.color = color;
			SceneManager.LoadScene ("Level-0");
		} else {
			this.GetComponent<Canvas> ().sortingOrder = -1;
			difficultyText.enabled = false;
			continueText.enabled = false;
			campaignText.enabled = false;
			arcadeText.enabled = false;
			exitText.enabled = false;
			missionSelect.enabled = true;
		}
			
			
	}

	public void StartLevel() {
		this.GetComponents<AudioSource> ()[1].Play ();
		if (Application.CanStreamedLevelBeLoaded ("Level-" + hIndex)) {
			this.GetComponent<Canvas> ().sortingOrder = 1;
			color.a = 255;
			splashScreen.color = color;
			SceneManager.LoadScene ("Level-" + hIndex);
		}
	}

	public void StartArcade() {
		color.a = 255;
		splashScreen.color = color;
		this.GetComponents<AudioSource> ()[1].Play ();
		SceneManager.LoadScene ("Arcade");
	}

	public void ExitGame() {
		this.GetComponents<AudioSource> ()[1].Play ();
		Application.Quit ();
	}
}
