using UnityEngine;
using System.Collections;

public class Doors : MonoBehaviour {

	public int keysNeeded = 0;

	private GameObject player;
	private CapsuleCollider col;
	private Animator anim;

	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player");
		col = GetComponent<CapsuleCollider> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject == player)
			anim.SetBool ("Open", true);
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject == player)
			anim.SetBool ("Open", false);
	}
}
