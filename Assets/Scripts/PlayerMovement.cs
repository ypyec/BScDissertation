using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;            

	Vector3 movement;                   
	Animator anim;                      
	Rigidbody playerRigidbody;
	bool attacking;

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

		if(	this.anim.GetCurrentAnimatorStateInfo(0).IsName("bigHit") ||
			this.anim.GetCurrentAnimatorStateInfo(0).IsName("block") ||
			this.anim.GetCurrentAnimatorStateInfo(0).IsName("SuperCharge"))
		{
			// Avoid any reload.
			this.attacking = true;
		}
		else if (this.attacking)
		{
			this.attacking = false;
			// You have just leaved your state!
		}

		if (!attacking) {
			Move (h, v);
			Animating (h, v);
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