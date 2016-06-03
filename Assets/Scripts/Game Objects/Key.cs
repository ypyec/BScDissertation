using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent <PlayerHealth> ()) {
			KeyManager.keys++;
			Destroy (gameObject);
		}
	}
}
