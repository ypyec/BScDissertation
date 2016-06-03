using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class EndLevel : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent <PlayerHealth> ()) {
			string resultString = Regex.Match(SceneManager.GetActiveScene().name, @"\d+").Value;
			int currentLevel = System.Int32.Parse (resultString);
			int nextLevel = currentLevel + 1;
			if (Application.CanStreamedLevelBeLoaded ("Level-" + nextLevel.ToString ())) {
				SceneManager.LoadScene ("Level-" + nextLevel.ToString ());
			} else {
				SceneManager.LoadScene ("Main Menu");
			}
		}
	}

}
