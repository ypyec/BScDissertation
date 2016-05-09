﻿using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour
{
	public float fieldOfViewAngle = 180f;           // Number of degrees, centred on forward, for the enemy see.
	public bool playerInSight;                      // Whether or not the player is currently sighted.
	public Vector3 personalLastSighting;            // Last place this enemy spotted the player.
	public Vector3 resetposition = new Vector3 (1000f, 1000f, 1000f);
	public Vector3 playerposition = new Vector3 (1000f, 1000f, 1000f);


	//private NavMeshAgent nav;                       // Reference to the NavMeshAgent component.
	private SphereCollider col;                     // Reference to the sphere collider trigger component.
	private Animator anim;                          // Reference to the Animator.
	private GameObject player;                      // Reference to the player.
	//private Animator playerAnim;                    // Reference to the player's animator component.
	//private PlayerHealth playerHealth;              // Reference to the player's health script.
	private Vector3 previousSighting;               // Where the player was sighted last frame.
	private EnemyAttackLight enemyAttack;


	void Awake ()
	{
		// Setting up the references.
		//nav = GetComponent<NavMeshAgent>();
		col = GetComponentInChildren<SphereCollider>();
		anim = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player");
		enemyAttack = GetComponent<EnemyAttackLight> ();
		//playerAnim = player.GetComponent<Animator>();
		//playerHealth = player.GetComponent<PlayerHealth>();

		// Set the personal sighting and the previous sighting to the reset position.
		personalLastSighting = resetposition;
		previousSighting = resetposition;
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
		if(other.gameObject == player)
		{
			
			// By default the player is not in sight.
			playerInSight = false;

			// Create a vector from the enemy to the player and store the angle between it and forward.
			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);

			// If the angle between forward and where the player is, is less than half the angle of view...
			if(angle < fieldOfViewAngle * 0.5f)
			{
				
				RaycastHit hit;


				// ... and if a raycast towards the player hits something...
				if(Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius*2))
				{
					
					// ... and if the raycast hits the player...
					if(hit.collider.gameObject == player)
					{
						// ... the player is in sight.
						playerInSight = true;

						// Set the last global sighting is the players current position.
						playerposition = player.transform.position;
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
	}

	void Animating (bool walking)
	{
		anim.SetBool ("isWalking", walking);	
	}
}