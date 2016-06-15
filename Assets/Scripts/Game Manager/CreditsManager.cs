using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour {

	void Start () {
		Invoke ("LoadMenu", 65f);	
	}

	void Update () {
	
	}

	void LoadMenu() {
		SceneManager.LoadScene ("Main Menu");
	}
}
