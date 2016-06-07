using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
	public float chaseWaitTime = 5f;                        // The amount of time to wait when the last sighting is reached.

	private EnemyAttackLight enemyAttack;
	private EnemyHealth enemyHealth;
	private EnemySight enemySight;                          // Reference to the EnemySight script.
	private NavMeshAgent nav;                               // Reference to the nav mesh agent.
	private Transform player;                               // Reference to the player's transform.
	private PlayerHealth playerHealth;                      // Reference to the PlayerHealth script.
	private float chaseTimer;                               // A timer for the chaseWaitTime.
	private int wayPointIndex;                              // A counter for the way point array.
	private float stoppingDistance;


	void Awake ()
	{
		// Setting up the references.
		enemyAttack = GetComponent<EnemyAttackLight>();
		enemyHealth = GetComponent<EnemyHealth> ();
		enemySight = GetComponent<EnemySight>();
		nav = GetComponent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		playerHealth = player.GetComponent<PlayerHealth>();
		stoppingDistance = nav.stoppingDistance;
	}


	void Update ()
	{
		// If the player is in sight and is alive...
		if (enemySight.playerInSight && 
			!enemyAttack.attacking && 
			!enemyHealth.dead () && 
			!enemyHealth.stuned && 
			Vector3.Distance (transform.position, player.position) < enemyAttack.attackRange && 
			!enemyAttack.isMelee)

			// ... shoot.
			Shooting ();

		// If the player has been sighted and isn't dead...
		else if (enemySight.personalLastSighting != enemySight.resetposition &&
			!enemyAttack.attacking && 
			!enemyHealth.dead () && 
			!enemyHealth.stuned && 
			playerHealth.currentHealth > 0f) {
			// ... chase.
			if ((enemyAttack.isMelee &&
				(Vector3.Distance (transform.position, enemyAttack.attackposition) < 1) || 
				enemyAttack.attackposition == Vector3.zero) || 
				!enemyAttack.isMelee) {
				if (enemySight.healthpackposition !=
					enemySight.resetposition && 
					enemyHealth.currentHealth <= (enemyHealth.startingHealth / 2) &&
					enemySight.healthpackInSight && 
					Vector3.Distance (transform.position, player.position) > Vector3.Distance (transform.position, enemySight.healthpackposition))

					SearchHealthPack ();
				else {

					nav.Resume ();
					Chasing ();
				}
			}
		}
		else if (enemySight.healthpackposition != 
			enemySight.resetposition && 
			enemyHealth.currentHealth <= (enemyHealth.startingHealth / 2) && 
			enemySight.healthpackInSight && 
			Vector3.Distance (transform.position, player.position) > Vector3.Distance (transform.position, enemySight.healthpackposition))

			SearchHealthPack();
	}


	void Shooting ()
	{
		// Stop the enemy where it is.
		nav.Stop();
	}

	void SearchHealthPack() {
		nav.stoppingDistance = 0;
		nav.SetDestination (enemySight.healthpackposition);
		enemySight.healthpackposition = enemySight.resetposition;
	}


	void Chasing ()
	{
		nav.stoppingDistance = stoppingDistance;
		// Create a vector from the enemy to the last sighting of the player.
		Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;

		// If the the last personal sighting of the player is not close...
		if(sightingDeltaPos.sqrMagnitude > 4f)
			// ... set the destination for the NavMeshAgent to the last personal sighting of the player.
			nav.destination = enemySight.personalLastSighting;

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
		
}