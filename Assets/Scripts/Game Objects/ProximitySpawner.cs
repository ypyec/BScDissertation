using UnityEngine;
using System.Collections;

public class ProximitySpawner : MonoBehaviour {

	public GameObject enemy;
	public GameObject particles;

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent <PlayerHealth> ()) {
			spawnEnemy ();

		} 
	}

	void spawnEnemy(){
		GameObject spawnEffect = Instantiate (particles, transform.position, transform.rotation) as GameObject;
		GameObject e = Instantiate (enemy, transform.position, transform.rotation) as GameObject;
		Destroy (spawnEffect, 0.5f);
		Destroy (gameObject);
	}
}
