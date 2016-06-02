using UnityEngine;
using System.Collections;

public class LevelClear : MonoBehaviour {

	private BoxCollider col;
	private GameObject player;

	// Use this for initialization
	void Start () {
		col = GetComponent<BoxCollider> ();
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject == player)
			print ("Enter");
	}
}
