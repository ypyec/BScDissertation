using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
	public float chaseWaitTime = 5f;                        // The amount of time to wait when the last sighting is reached.
	public float cooldown; 
	public Animator anim;
	public Vector3 attackposition;
	public Vector3 target;

	private EnemyAttackLight enemyAttack;
	private EnemyHealth enemyHealth;
	private EnemySight enemySight;                          // Reference to the EnemySight script.
	private NavMeshAgent nav;                               // Reference to the nav mesh agent.
	private GameObject player;                               // Reference to the player's transform.
	private PlayerHealth playerHealth;                      // Reference to the PlayerHealth script.
	private float chaseTimer;                               // A timer for the chaseWaitTime.
	private int wayPointIndex;                              // A counter for the way point array.
	private float stoppingDistance;
	private float normalspeed;

	void Awake ()
	{
		// Setting up the references.
		enemyAttack = GetComponent<EnemyAttackLight>();
		enemyHealth = GetComponent<EnemyHealth> ();
		enemySight = GetComponent<EnemySight> ();
		anim = GetComponent<Animator> ();
		nav = GetComponent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag ("Player");
		playerHealth = player.GetComponent<PlayerHealth>();
		stoppingDistance = nav.stoppingDistance;
		cooldown = enemyAttack.basicShotCD;
		normalspeed = nav.speed;
		attackposition = Vector3.zero;
	}


	void Update ()
	{
		cooldown += Time.deltaTime;
		switch (PlayerPrefs.GetInt ("Difficulty")) {
		case 1:
			GameEasy ();
			break;
		case 2:
			GameMedium ();
			break;
		case 3:
			GameHard ();
			break;
		}
	}

	void GameEasy() {
		target = enemySight.playerposition;
		attackposition = player.transform.position;
		if (enemySight.playerInSight &&
		    !enemyAttack.attacking &&
		    !enemyHealth.dead () &&
		    !enemyHealth.stuned &&
		    Vector3.Distance (transform.position, target) <= enemyAttack.attackRange) {
			Shooting ();
		}

		else if (enemySight.personalLastSighting != enemySight.resetposition &&
			!enemyAttack.attacking && 
			!enemyHealth.dead () && 
			!enemyHealth.stuned &&
			playerHealth.currentHealth > 0f) {
			// ... chase.
			if ((enemyAttack.isMelee &&
				(Vector3.Distance (transform.position, attackposition) < 1) || 
				attackposition == Vector3.zero) || 
				!enemyAttack.isMelee) {
					nav.Resume ();
					Chasing ();
				}
			nav.Resume ();
			Chasing ();
			}
	}

	void GameMedium() {
		target = enemySight.playerposition;
		attackposition = player.transform.position;
		if (enemyHealth.currentHealth < enemyHealth.startingHealth &&
		    enemySight.healthpackInSight &&
		    !enemyHealth.dead () &&
		    !enemyHealth.stuned) {
			nav.Resume ();
			SearchHealthPack ();
		
		} else if (enemySight.playerInSight &&
		         !enemyAttack.attacking &&
		         !enemyHealth.dead () &&
		         !enemyHealth.stuned &&
		         Vector3.Distance (transform.position, target) <= enemyAttack.attackRange) {
			Shooting ();
		}

		else if (enemySight.personalLastSighting != enemySight.resetposition &&
			!enemyAttack.attacking && 
			!enemyHealth.dead () && 
			!enemyHealth.stuned && 
			playerHealth.currentHealth > 0f) {
			// ... chase.
			if ((enemyAttack.isMelee &&
				(Vector3.Distance (transform.position, attackposition) < 10) || 
				attackposition == Vector3.zero) || 
				!enemyAttack.isMelee) {
				nav.Resume ();
				Chasing ();
			}
			nav.Resume ();
			Chasing ();
		}
	}

	void GameHard() {
		target = enemySight.playerposition;
		if (enemyHealth.currentHealth < enemyHealth.startingHealth &&
		    enemySight.healthpackInSight &&
		    !enemyHealth.dead () &&
		    !enemyHealth.stuned) {
			nav.Resume ();
			SearchHealthPack ();
		
		} else if (enemyHealth.currentHealth < enemyHealth.startingHealth &&
		        enemySight.boxInSight &&
		        !enemyAttack.attacking &&
		        !enemyHealth.dead () &&
		        !enemyHealth.stuned) {
			target = enemySight.boxposition;
			Shooting ();
		}

		else if (enemySight.playerInSight &&
			!enemyAttack.attacking &&
			!enemyHealth.dead () &&
			!enemyHealth.stuned &&
			Vector3.Distance (transform.position, target) <= enemyAttack.attackRange) {
			Shooting ();
		}

		else if (enemySight.personalLastSighting != enemySight.resetposition &&
			!enemyAttack.attacking && 
			!enemyHealth.dead () && 
			!enemyHealth.stuned && 
			playerHealth.currentHealth > 0f) {
			// ... chase.
			if ((enemyAttack.isMelee &&
				(Vector3.Distance (transform.position, attackposition) < 1) || 
				attackposition == Vector3.zero) || 
				!enemyAttack.isMelee) {
				nav.Resume ();
				Chasing ();
			}
			nav.Resume ();
			Chasing ();
		}		
	}

	void Shooting ()
	{
		if (!enemyAttack.attacking && 
			cooldown >= enemyAttack.basicShotCD &&
			!enemyHealth.dead () &&
			!enemyHealth.stuned &&
			player.GetComponent<PlayerHealth> ().currentHealth > 0f) {
			if (enemyAttack.isMelee) {
				AttackMelee ();
				if (enemyAttack.attacking &&
				    Vector3.Distance (transform.position, attackposition) < 1) {
					enemyAttack.attacking = false;
					attackposition = Vector3.zero;
					nav.speed = normalspeed;
					anim.ResetTrigger ("Hit");
				}
			} else {
				nav.Stop();
				StartCoroutine (enemyAttack.animateAttack ());
				enemyAttack.attacking = false;
				anim.ResetTrigger ("Hit");
			}
		}
	}

	void AttackMelee() {
		attackposition = target;
		Vector3 playerDirection = target - transform.position;
		float angleBetween = Vector3.Angle(transform.forward, playerDirection);
		if (angleBetween > 1)
			transform.forward = playerDirection;
		nav.speed = 10f;

		nav.SetDestination (attackposition);
	}

	void SearchHealthPack() {
		nav.stoppingDistance = 0;
		nav.SetDestination (enemySight.healthpackposition);
		if (transform.position == nav.destination) {
			enemySight.healthpackposition = enemySight.resetposition;
			nav.stoppingDistance = stoppingDistance;
		}
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