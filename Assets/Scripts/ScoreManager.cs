﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
		// Set the displayed text to be the word "Score" followed by the score value.
		if (score > highscore) {
			PlayerPrefs.SetInt ("High Score", score);
		}
		text.text = "Puntaje actual: " + score;

	}
}