using UnityEngine;
using System.Collections;

public class PowerPack : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent <PlayerAttack> ()) {
			if (!other.GetComponent <PlayerAttack> ().empowered) {
				other.GetComponent <PlayerAttack> ().Empower ();
				Destroy (gameObject);
			}   
		}
	}
}
