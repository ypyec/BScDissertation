using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent <PlayerHealth> ()) {
			KeyManager.keys++;
			GetComponent<AudioSource> ().Play ();
			Destroy (gameObject, 0.4f);
		}
	}
}
