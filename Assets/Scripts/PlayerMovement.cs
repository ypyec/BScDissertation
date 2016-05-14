using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;            

	Vector3 movement;                   
	Animator anim;                      
	Rigidbody playerRigidbody;
	public bool attacking;
	public bool blocking;

	void Awake ()
	{
		movement = new Vector3 (0f, 0f, 0f);
		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
		attacking = false;
	}
		

	void FixedUpdate ()
	{
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");


		if (!attacking) {
			Move (h, v);
			Animating (h, v);
		} else if (blocking) {
			Move (h,v); 
		}

	}



	void Move (float h, float v)
	{
		if(movement.x != h || movement.z != v)
			transform.forward = movement;

		movement.Set (h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;

		playerRigidbody.MovePosition (transform.position + movement);
	}
		

	void Animating (float h, float v)
	{
		bool walking = false;
		if (v != 0f || h != 0f)
			walking = true;

		anim.SetBool ("Walking", walking);	
	}
}