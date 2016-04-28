using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;            

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

		Move (h, v);
		Animating (h, v);
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