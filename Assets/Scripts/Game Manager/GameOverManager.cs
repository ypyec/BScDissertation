using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
	public PlayerHealth playerHealth;       // Reference to the player's health.
	public Button restartText;
	public Button exitText;
	public AudioSource audioSource;
	public Text text1;
	public Text text2;
	public Text botText;
	public Text topText;


	Animator anim;                          // Reference to the animator component.
	float restartTimer;                     // Timer to count up to restarting the level
	private float timer;
	private int index;
	Canvas GameOverMenu;


	void Awake ()
	{
		// Set up the reference.
		anim = GetComponent <Animator> ();
		GameOverMenu = GetComponent<Canvas> ();
		restartText = restartText.GetComponent<Button> ();
		exitText = exitText.GetComponent<Button> ();
		timer = 0.5f;
		index = 0;
		GameOverMenu.gameObject.SetActive (false);
		GameOverMenu.enabled = false;
		audioSource = audioSource.GetComponent<AudioSource> ();
		GameOverSelect (index);
		text1 = text1.GetComponent<Text> ();
		text2 = text2.GetComponent<Text> ();
		botText= botText.GetComponent<Text> ();
		topText = topText.GetComponent<Text> ();
	}


	void Update ()
	{
		timer += Time.unscaledDeltaTime;
		// If the player has run out of health...
		if(playerHealth.currentHealth <= 0 && timer > 3f)
		{
			GameOverMenu.gameObject.SetActive (true);
			text1.enabled = false;
			text2.enabled = false;
			topText.text = text2.text;
			botText.text = text1.text;
			audioSource.Pause ();
			anim.SetTrigger ("GameOver");
			GameOverMenu.enabled = true;
			if (Input.GetAxisRaw ("Vertical") == -1 && timer >= 0.3f) {
				if (index == 1)
					index = 0;
				else
					index++;
				GameOverSelect (index);
			} else if (Input.GetAxisRaw ("Vertical") == 1 && timer >= 0.3f) {
				if (index == 0)
					index = 1;
				else
					index--;
				GameOverSelect (index);
			} 
		}
	}

	private void GameOverSelect(int MenuIndex) {
		this.GetComponents<AudioSource> ()[0].Play ();
		switch (MenuIndex) {
		case 0:
			restartText.Select ();
			timer = 0f;
			break;
		case 1:
			exitText.Select ();
			timer = 0f;
			break;
		}
	}

	public void Restart() {
		this.GetComponents<AudioSource> ()[1].Play ();
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}

	public void ExitGame() {
		this.GetComponents<AudioSource> ()[1].Play ();
		SceneManager.LoadScene ("Main Menu");
	}
}