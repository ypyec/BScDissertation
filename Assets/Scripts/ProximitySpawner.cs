using UnityEngine;
using System.Collections;

public class ProximitySpawner : MonoBehaviour {

	public GameObject enemy;

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent <PlayerHealth> ()) {
			//Invoke("spawnEnemy", 0.5f); 
			spawnEnemy ();

		} 
	}

	void spawnEnemy(){
		gameObject.SetActive(false);
		Instantiate (enemy, transform.position, transform.rotation);

	}
}
