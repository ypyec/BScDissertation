using UnityEngine;
using System.Collections;

public class HealthPack : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent <EnemyHealth> ()) {
			other.GetComponent <EnemyHealth> ().Heal ();
			Destroy (gameObject);
		} else if(other.GetComponent <PlayerHealth> ()) {
			other.GetComponent <PlayerHealth> ().Heal ();
			Destroy (this.gameObject);    
		}
	}
}
