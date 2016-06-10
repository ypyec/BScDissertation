using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class EndLevel : MonoBehaviour {

	public GameObject particles;

	private bool changingLevel;

	void Awake(){
		changingLevel = false;
	}

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent <PlayerHealth> () && !changingLevel) {
			changingLevel = true;
			other.GetComponent <PlayerMovement> ().enabled = false;
			other.GetComponent <PlayerAttack> ().enabled = false;
			other.transform.position.Set (transform.position.x, transform.position.y + 0.05f, transform.position.z + 0.05f);
			other.GetComponent <Animator> ().SetTrigger ("EndLevel");
			GetComponent<AudioSource> ().Play ();
			GameObject particle = Instantiate (particles, particles.transform.position, particles.transform.rotation) as GameObject;
			particle.transform.position = other.transform.position;
			Invoke ("changeLevel", 7f);
		}
	}

	void changeLevel(){
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
