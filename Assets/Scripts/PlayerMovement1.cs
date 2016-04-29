using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
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
		print (h + ';' + v);
		movement.Set (h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;

		playerRigidbody.MovePosition (transform.position + movement);
	}


	void Animating (float h, float v)
	{
		bool idle = true;
		bool walking = false;
		bool backward = false;
		bool left = false;
		bool right = false;
		if ((v != 0f || h != 0f) && v > 0) {
			walking = true;
			idle = false;
		} else if ((v != 0f || h != 0f) && v < 0) {
			backward = true;
			idle = false;
		} else if ((v != 0f || h != 0f) && h < 0 && v == 0) {
			right = true;
			idle = false;
		} else if ((v != 0f || h != 0f) && h > 0 && v == 0) {
			left = true;
			idle = false;
		}

		anim.SetBool ("Walking", walking);	
		anim.SetBool ("Backwards", backward);
		anim.SetBool ("IDLE", idle);
		anim.SetBool ("Left", left);
		anim.SetBool ("Right", right);
	}
}