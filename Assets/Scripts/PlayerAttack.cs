using UnityEngine;

public class PlayerAttack : MonoBehaviour
{         
	Vector3 movement;                   
	Animator anim;                      
	Rigidbody playerRigidbody;          

	void Awake ()
	{
		movement = new Vector3 (0f, 0f, 0f);
		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
	}


	void FixedUpdate ()
	{
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");
		Animating ();
	}


	void Animating ()
	{
		if (Input.GetAxisRaw ("Fire1") != 0) {
			anim.SetTrigger ("Hit");
		} else if (Input.GetAxisRaw ("Fire2") != 0) {
			anim.SetTrigger ("Hit2");
		} else {
			anim.ResetTrigger ("Hit");
			anim.ResetTrigger ("Hit2");
		}
	}
}