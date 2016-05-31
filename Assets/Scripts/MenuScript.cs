using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public Canvas quitMenu;
	public Button campaignText;
	public Button arcadeText;
	public Button exitText;

	// Use this for initialization
	void Start () {
		quitMenu = quitMenu.GetComponent<Canvas> ();
		campaignText = campaignText.GetComponent<Button> ();
		arcadeText = arcadeText.GetComponent<Button> ();
		exitText = exitText.GetComponent<Button> ();
		quitMenu.enabled = false;
	}

	public void ExitPress() {
		quitMenu.enabled = true;
		campaignText.enabled = false;
		arcadeText.enabled = false;
		exitText.enabled = false;
	}

	public void NoPress() {
		quitMenu.enabled = false;
		campaignText.enabled = true;
		arcadeText.enabled = true;
		exitText.enabled = true;
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
