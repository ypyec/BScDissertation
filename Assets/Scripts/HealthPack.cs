using UnityEngine;
using System.Collections;

public class HealthPack : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent <PlayerHealth> ()) {
			if (other.GetComponent <PlayerHealth> ().currentHealth < other.GetComponent <PlayerHealth> ().startingHealth) {
				other.GetComponent <PlayerHealth> ().Heal ();
				Destroy (gameObject);
			}   
		} else if (other.GetComponent <EnemyHealth> ()) {
			if (other.GetComponent <EnemyHealth> ().currentHealth < other.GetComponent <EnemyHealth> ().startingHealth) {
				other.GetComponent <EnemyHealth> ().Heal ();
				Destroy (gameObject);
			}
		}
	}
}
