using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour
{
	public float fieldOfViewAngle = 360f;           // Number of degrees, centred on forward, for the enemy see.
	public bool playerInSight;                      // Whether or not the player is currently sighted.
	public Vector3 personalLastSighting;            // Last place this enemy spotted the player.
	public Vector3 resetposition = new Vector3 (1000f, 1000f, 1000f);
	public Vector3 playerposition = new Vector3 (1000f, 1000f, 1000f);
	public bool isMelee = false;
	public bool healthpackInSight;
	public Vector3 healthpackposition = new Vector3 (1000f, 1000f, 1000f);

	private SphereCollider col;                     // Reference to the sphere collider trigger component.
	private Animator anim;                          // Reference to the Animator.
	private GameObject player;                      // Reference to the player.
	private Vector3 previousSighting;               // Where the player was sighted last frame.
	private EnemyAttackLight enemyAttack;
	private GameObject healthpack;



	void Awake ()
	{
		// Setting up the references.
		col = GetComponentInChildren<SphereCollider>();
		anim = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player");
		enemyAttack = GetComponent<EnemyAttackLight> ();
		healthpack = GameObject.FindGameObjectWithTag ("HealthPack");
		healthpackInSight = false;

		// Set the personal sighting and the previous sighting to the reset position.
		personalLastSighting = resetposition;
		previousSighting = resetposition;
		col.radius *= 8;
	}


	void Update ()
	{
		// If the last global sighting of the player has changed...
		if(playerposition != previousSighting)
			// ... then update the personal sighting to be the same as the global sighting.
			personalLastSighting = playerposition;

		// Set the previous sighting to the be the sighting from this frame.
		previousSighting = playerposition;

		// If the player is alive...
		//if(playerHealth.health > 0f)
			// ... set the animator parameter to whether the player is in sight or not.
		//	anim.SetBool(hash.playerInSightBool, playerInSight);
		//else
			// ... set the animator parameter to false.
		//	anim.SetBool(hash.playerInSightBool, false);
	}


	void OnTriggerStay (Collider other)
	{
		
		// If the player has entered the trigger sphere...
		if(other.gameObject == player || other.gameObject == healthpack)
		{
			
			// By default the player is not in sight.
			playerInSight = false;
			healthpackInSight = false;

			// Create a vector from the enemy to the player and store the angle between it and forward.
			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);

			// If the angle between forward and where the player is, is less than half the angle of view...
			if(angle < fieldOfViewAngle * 0.5f)
			{
				
				RaycastHit hit;


				// ... and if a raycast towards the player hits something...
				if(Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius * transform.lossyScale.x))
				{
					
					// ... and if the raycast hits the player...
					if(hit.collider.gameObject == player)
					{
						// ... the player is in sight.
						playerInSight = true;

						// Set the last global sighting is the players current position.
						playerposition = player.transform.position;
					}
					if (hit.collider.gameObject == healthpack) {
						healthpackInSight = true;
						healthpackposition = healthpack.transform.position;
					}
				}
			}
			if (Vector3.Distance (transform.position, other.transform.position) < enemyAttack.attackRange)
				Animating (!playerInSight);
			else
				Animating (playerInSight);
		}
	}


	void OnTriggerExit (Collider other)
	{
		// If the player leaves the trigger zone...
		if(other.gameObject == player)
			// ... the player is not in sight.
			playerInSight = false;
		if (other.gameObject == healthpack)
			healthpackInSight = false;
	}

	void Animating (bool walking)
	{
		anim.SetBool ("isWalking", walking);	
	}
}