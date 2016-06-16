using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KeyNeed : MonoBehaviour {

	public Text tutorialText;
	public RectTransform tutorial;

	private float timer;
	private float timerdelay;
	private GameObject player;

	void Awake () {
		player = GameObject.FindGameObjectWithTag ("Player");
		tutorialText = tutorialText.GetComponent<Text> ();
		tutorial = tutorial.GetComponent<RectTransform> ();
		timerdelay = 0f;
		timer = 0f;	
	}
	
	// Update is called once per frame
	void Update () {
		print (timerdelay);
		timer += Time.unscaledDeltaTime;
		if (timer < timerdelay && timerdelay != 0f) {
			tutorial.gameObject.SetActive (true);
		} else if (timer > timerdelay && timerdelay != 0f)
			tutorial.gameObject.SetActive (false);
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject == player) {
			timerdelay = timer + 6f;
			tutorialText.text = "Algunas puertas se encuentran bloqueadas, deberás encontrar las llaves necesarias para poder abrirlas";
		}
			
	}
}
