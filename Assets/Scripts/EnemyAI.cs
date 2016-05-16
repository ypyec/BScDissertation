﻿using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
	public float patrolSpeed = 2f;                          // The nav mesh agent's speed when patrolling.
	public float chaseSpeed = 5f;                           // The nav mesh agent's speed when chasing.
	public float chaseWaitTime = 5f;                        // The amount of time to wait when the last sighting is reached.
	public float patrolWaitTime = 1f;                       // The amount of time to wait when the patrol way point is reached.
	public Transform[] patrolWayPoints;                     // An array of transforms for the patrol route.

	private EnemyAttackLight enemyAttack;
	private EnemyHealth enemyHealth;
	private EnemySight enemySight;                          // Reference to the EnemySight script.
	private NavMeshAgent nav;                               // Reference to the nav mesh agent.
	private Transform player;                               // Reference to the player's transform.
	private PlayerHealth playerHealth;                      // Reference to the PlayerHealth script.
	private float chaseTimer;                               // A timer for the chaseWaitTime.
	private float patrolTimer;                              // A timer for the patrolWaitTime.
	private int wayPointIndex;                              // A counter for the way point array.


	void Awake ()
	{
		// Setting up the references.
		enemyAttack = GetComponent<EnemyAttackLight>();
		enemyHealth = GetComponent<EnemyHealth> ();
		enemySight = GetComponent<EnemySight>();
		nav = GetComponent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		playerHealth = player.GetComponent<PlayerHealth>();
	}


	void Update ()
	{
		print (enemySight.healthpackposition);
		// If the player is in sight and is alive...
		if (enemySight.playerInSight && !enemyAttack.attacking && !enemyHealth.dead () && !enemyHealth.stuned && Vector3.Distance (transform.position, player.position) < enemyAttack.attackRange && !enemyAttack.isMelee)
			// ... shoot.
			Shooting ();

		// If the player has been sighted and isn't dead...
		else if (enemySight.personalLastSighting != enemySight.resetposition && !enemyAttack.attacking && !enemyHealth.dead () && !enemyHealth.stuned && playerHealth.currentHealth > 0f) {
			// ... chase.
			if ((enemyAttack.isMelee && (Vector3.Distance (transform.position, enemyAttack.attackposition) < 1) || enemyAttack.attackposition == Vector3.zero) || !enemyAttack.isMelee) {
				if (enemySight.healthpackposition != enemySight.resetposition && enemyHealth.currentHealth <= (enemyHealth.startingHealth / 2) && enemySight.healthpackInSight && Vector3.Distance (transform.position, player.position) > Vector3.Distance (transform.position, enemySight.healthpackposition))
					SearchHealthPack ();
				else {
					nav.Resume ();
					Chasing ();
				}
			}
		}
		else if (enemySight.healthpackposition != enemySight.resetposition && enemyHealth.currentHealth <= (enemyHealth.startingHealth / 2) && enemySight.healthpackInSight && Vector3.Distance (transform.position, player.position) > Vector3.Distance (transform.position, enemySight.healthpackposition))
			SearchHealthPack();
		// Otherwise...
		//else
			// ... patrol.
		//	Patrolling();
	}


	void Shooting ()
	{
		// Stop the enemy where it is.
		nav.Stop();
	}

	void SearchHealthPack() {
		nav.SetDestination (enemySight.healthpackposition);
		enemySight.healthpackposition = enemySight.resetposition;
	}


	void Chasing ()
	{
		// Create a vector from the enemy to the last sighting of the player.
		Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;

		// If the the last personal sighting of the player is not close...
		if(sightingDeltaPos.sqrMagnitude > 4f)
			// ... set the destination for the NavMeshAgent to the last personal sighting of the player.
			nav.destination = enemySight.personalLastSighting;

		// Set the appropriate speed for the NavMeshAgent.
		nav.speed = chaseSpeed;

		// If near the last personal sighting...
		if(nav.remainingDistance < nav.stoppingDistance)
		{
			// ... increment the timer.
			chaseTimer += Time.deltaTime;

			// If the timer exceeds the wait time...
			if(chaseTimer >= chaseWaitTime)
			{
				// ... reset last global sighting, the last personal sighting and the timer.
				enemySight.playerposition = enemySight.resetposition;
				enemySight.personalLastSighting = enemySight.resetposition;
				chaseTimer = 0f;
			}
		}
		else
			// If not near the last sighting personal sighting of the player, reset the timer.
			chaseTimer = 0f;
	}


	/*void Patrolling ()
	{
		// Set an appropriate speed for the NavMeshAgent.
		nav.speed = patrolSpeed;

		// If near the next waypoint or there is no destination...
		if(nav.destination == lastPlayerSighting.resetPosition || nav.remainingDistance < nav.stoppingDistance)
		{
			// ... increment the timer.
			patrolTimer += Time.deltaTime;

			// If the timer exceeds the wait time...
			if(patrolTimer >= patrolWaitTime)
			{
				// ... increment the wayPointIndex.
				if(wayPointIndex == patrolWayPoints.Length - 1)
					wayPointIndex = 0;
				else
					wayPointIndex++;

				// Reset the timer.
				patrolTimer = 0;
			}
		}
		else
			// If not near a destination, reset the timer.
			patrolTimer = 0;

		// Set the destination to the patrolWayPoint.
		nav.destination = patrolWayPoints[wayPointIndex].position;
	}*/
}