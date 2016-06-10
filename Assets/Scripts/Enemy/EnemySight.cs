using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemySight : MonoBehaviour
{
	public bool playerInSight;                      // Whether or not the player is currently sighted.
	public Vector3 personalLastSighting;            // Last place this enemy spotted the player.
	public Vector3 resetposition = new Vector3 (1000f, 1000f, 1000f);
	public Vector3 playerposition = new Vector3 (1000f, 1000f, 1000f);
	public bool healthpackInSight;
	public Vector3 healthpackposition = new Vector3 (1000f, 1000f, 1000f);
	public bool boxInSight;
	public GameObject box;

	private float fieldOfViewAngle;           // Number of degrees, centred on forward, for the enemy see.
	private SphereCollider col;                     // Reference to the sphere collider trigger component.
	private Animator anim;                          // Reference to the Animator.
	private GameObject player;                      // Reference to the player.
	private Vector3 previousSighting;               // Where the player was sighted last frame.
	private EnemyAttackLight enemyAttack;
	private EnemyAI enemyAI;



	void Awake ()
	{
		// Setting up the references.
		col = GetComponentInChildren<SphereCollider>();
		anim = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player");
		enemyAttack = GetComponent<EnemyAttackLight> ();
		enemyAI = GetComponent<EnemyAI> ();
		healthpackInSight = false;
		boxInSight = false;

		// Set the personal sighting and the previous sighting to the reset position.
		personalLastSighting = resetposition;
		previousSighting = resetposition;

		if (SceneManager.GetActiveScene ().name == "Arcade") {
			fieldOfViewAngle = 360;
			col.radius = col.radius * 4f;
		} else {
			switch (PlayerPrefs.GetInt ("Difficulty")) {
			case 1:
				fieldOfViewAngle = 180;
				break;
			case 2:
				fieldOfViewAngle = 270;
				break;
			case 3:
				fieldOfViewAngle = 360;
				break;
			}
		}

	}


	void Update ()
	{
		// If the last global sighting of the player has changed...
		if(playerposition != previousSighting)
			// ... then update the personal sighting to be the same as the global sighting.
			personalLastSighting = playerposition;

		// Set the previous sighting to the be the sighting from this frame.
		previousSighting = playerposition;
	}


	void OnTriggerStay (Collider other)
	{

		if(other.gameObject == player){
			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);

			if (angle < fieldOfViewAngle * 0.5f) {
				playerInSight = true;
				playerposition = player.transform.position;
			} else {
				playerInSight = false;
			}
		}

		if (other.gameObject.GetComponent<HealthPack> ()) {
			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle (direction, transform.forward);

			if (angle < fieldOfViewAngle * 0.5f) {
				healthpackInSight = true;

				healthpackposition = other.gameObject.transform.position;
			} else {
				healthpackInSight = false;
			}
		}


			
		if (other.gameObject.GetComponent<BoxHealth> ()) {
			if (other.gameObject.activeSelf) {
				Vector3 direction = other.transform.position - transform.position;
				float angle = Vector3.Angle (direction, transform.forward);

				if (angle < fieldOfViewAngle * 0.5f) {
					boxInSight = true;

					box = other.gameObject;
				} else {
					boxInSight = false;
				}
			}
		}

	}


	void OnTriggerExit (Collider other)
	{
		// If the player leaves the trigger zone...
		if(other.gameObject == player)
			// ... the player is not in sight.
			playerInSight = false;
		if (other.gameObject.GetComponent<HealthPack>())
			healthpackInSight = false;
		if (other.gameObject.GetComponent<BoxHealth>())
			boxInSight = false;
	}

	public void Animating ()
	{
		if (Vector3.Distance (transform.position, enemyAI.target.transform.position) <= enemyAttack.attackRange) {
			anim.SetBool ("isWalking", false);	
			anim.SetTrigger ("Hit");
		} else {
			anim.ResetTrigger ("Hit");
			anim.SetBool ("isWalking", true);
		}
	}
}
