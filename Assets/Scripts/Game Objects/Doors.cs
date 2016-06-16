using UnityEngine;
using System.Collections;

public class Doors : MonoBehaviour {

	public bool keyLocked = false;

	private GameObject player;
	private CapsuleCollider col;
	private Animator anim;

	private AudioSource sndOpen;
	private AudioSource sndLocked;
	private AudioSource sndUnlock;

	private bool enemyLocked;

	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player");
		col = GetComponent<CapsuleCollider> ();
		anim = GetComponent<Animator> ();
		sndOpen = GetComponents<AudioSource> ()[0];
		sndLocked = GetComponents<AudioSource> ()[1];
		sndUnlock = GetComponents<AudioSource> ()[2];
		enemyLocked = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find ("M-MA(Clone)") != null ||
			GameObject.Find ("M-AV(Clone)") != null ||
			GameObject.Find ("R-MT(Clone)") != null ||
			GameObject.Find ("R-SP(Clone)") != null) {
			enemyLocked = true;
			anim.SetBool ("Open", false);
		} else {
			enemyLocked = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject == player) {
			if (keyLocked) {
				if (KeyManager.keys > 0) {
					KeyManager.keys--;
					sndUnlock.Play ();
					keyLocked = false;
					anim.SetBool ("Open", true);
					sndOpen.Play ();
				} else {
					sndLocked.Play ();
				}
			} else if (!enemyLocked) {
				anim.SetBool ("Open", true);
				sndOpen.Play ();
			} else {
				sndLocked.Play ();
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject == player)
			anim.SetBool ("Open", false);
	}
}
