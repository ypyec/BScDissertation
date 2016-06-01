using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System;

public class HighScoreManager : MonoBehaviour
{
	Text text;

	void Awake (){
		text = GetComponent <Text> ();
	}


	void Update (){
		if (SceneManager.GetActiveScene ().name == "Arcade")
			text.text = "Mejor Puntaje: " + PlayerPrefs.GetInt ("High Score");
		else {
			//guarda el número tomado del nombre del nivel en una variable y la convierte a Int
			string resultString = Regex.Match(SceneManager.GetActiveScene().name, @"\d+").Value;
			int currentLevel = Int32.Parse (resultString);

			//si el nivel actual es más avanzado lo guarda en el PlayerPrefs
			if (PlayerPrefs.GetInt ("Current Level") < currentLevel) {
				PlayerPrefs.SetInt ("Current Level", currentLevel);
			}

			text.text = "Nivel: " + currentLevel;
		}
			
	}
}