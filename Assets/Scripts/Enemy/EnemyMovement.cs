using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
	Transform player;               // Reference to the player's position.
	//PlayerHealth playerHealth;      // Reference to the player's health.
	//EnemyHealth enemyHealth;        // Reference to this enemy's health.
	NavMeshAgent nav;               // Reference to the nav mesh agent.
	SphereCollider range;
	Animator anim;
	bool walk;


	void Awake ()
	{
		// Set up the references.
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		//playerHealth = player.GetComponent <PlayerHealth> ();
		//enemyHealth = GetComponent <EnemyHealth> ();
		nav = GetComponent <NavMeshAgent> ();
		range = GetComponent <SphereCollider> ();
		anim = GetComponent <Animator> ();
	}


	void Update ()
	{
		// If the enemy and the player have health left...
		//if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
		//{
			// ... set the destination of the nav mesh agent to the player.
			
		//}
		// Otherwise...
		//else
		//{
			// ... disable the nav mesh agent.
		//	nav.enabled = false;
		//}
	}

	void OnTriggerEnter(Collider enemy) {
		if (enemy.CompareTag ("Player")) {
			walk = true;
			Animating (walk);
			nav.SetDestination (player.position);
		}
	}

	void OnTriggerStay(Collider enemy) {
		if (enemy.CompareTag ("Player")) {
			walk = true;
			Animating (walk);
			nav.SetDestination (player.position);
		}
	}

	void OnTriggerExit(Collider player) {
		if (player.CompareTag ("Player")) {
			walk = false;
			Animating (walk);
		}
	}

	void Animating (bool walking)
	{
		anim.SetBool ("isWalking", walking);	
	}
}