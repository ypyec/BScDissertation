using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
	public static int score;        // The player's score.


	Text text;                      // Reference to the Text component.
	int highscore;

	void Awake ()
	{
		// Set up the reference.
		text = GetComponent <Text> ();
		highscore = PlayerPrefs.GetInt("High Score");
		// Reset the score.
		score = 0;
	}


	void Update ()
	{
		if (SceneManager.GetActiveScene ().name == "Arcade") {
			if (score > highscore) {
				PlayerPrefs.SetInt ("High Score", score);
			}
			text.text = "Puntaje actual: " + score;
		} else {
			text.text = "Puntaje: " + score;
		}


	}
}