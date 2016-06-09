using UnityEngine;
using System.Collections;

public class EnemyAttackLight : MonoBehaviour
{                 
	public GameObject wave;

	public GameObject basicShot;
	public GameObject basicShotOrigin;
	public int basicShotDMG = 5;                  
	public float basicShotCD = 1.2f;
	public float attackRange;
	public bool attacking;
	public bool isMelee = false;
	public float bounceForce = 0f;

	private NavMeshAgent nav;
	private GameObject player;
	private GameObject box;
	private EnemyHealth enemyHealth;
	private Ray shootRay;                                   
	private RaycastHit shootHit;
	private AudioSource attackAudio;
	private EnemyAI enemyAI;

	void Awake ()
	{
		switch (PlayerPrefs.GetInt ("Difficulty")) {
		case 1:
			attackRange = 3f;
			break;
		case 2:
			attackRange = 4f;
			break;
		case 3:
			attackRange = 5f;
			break;
		}
		enemyAI = GetComponent <EnemyAI> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		attacking = false;
		enemyHealth = GetComponent <EnemyHealth> ();
		nav = GetComponent<NavMeshAgent> ();
		attackAudio = GetComponent<AudioSource> ();
	}
		
	public IEnumerator animateAttack(){
		attacking = true;
		enemyAI.cooldown = 0f;
		Vector3 playerDirection = enemyAI.target - transform.position;
		float angleBetween = Vector3.Angle(transform.forward, playerDirection);
		if (angleBetween > 1)
			transform.forward = playerDirection;
		enemyAI.anim.SetTrigger ("Hit");

		yield return new WaitForSeconds (0.27f);

		attack ();

	}

	void attack(){
		
		GameObject s1 = (GameObject)Instantiate(basicShot, basicShotOrigin.transform.position, this.transform.rotation);
		s1.GetComponent<BeamParam>().SetBeamParam(this.GetComponent<BeamParam>());

		GameObject wav = (GameObject)Instantiate(wave, this.transform.position, this.transform.rotation);
		wav.transform.localScale *= 0.25f;
		wav.transform.Rotate(Vector3.left, 90.0f);
		wav.GetComponent<BeamWave>().col = this.GetComponent<BeamParam>().BeamColor;

		attackAudio.Play ();

		MakeDamage (basicShotOrigin, basicShotDMG, this.GetComponent <BeamParam> ().MaxLength);
	}

	void OnCollisionStay(Collision collision) {
		if (collision.gameObject == player && isMelee) {
			GetComponent <Rigidbody> ().AddForce (transform.forward * (-bounceForce), ForceMode.Impulse);
			if (enemyAI.cooldown >= basicShotCD && !enemyHealth.dead () && !enemyHealth.stuned && player.GetComponent<PlayerHealth> ().currentHealth > 0f) {
				
				collision.gameObject.GetComponent <PlayerHealth> ().TakeDamage (basicShotDMG);
				attackAudio.Play ();
				enemyAI.cooldown = 0;
			}
		} else if (collision.gameObject.GetComponent <BoxHealth> () && isMelee) {
			GetComponent <Rigidbody> ().AddForce (transform.forward * (-bounceForce*3), ForceMode.Impulse);
			if (enemyAI.cooldown >= basicShotCD && !enemyHealth.dead () && !enemyHealth.stuned && collision.gameObject.GetComponent <BoxHealth> ().currentHealth > 0f) {

				attackAudio.Play ();
				collision.gameObject.GetComponent <BoxHealth> ().TakeDamage (basicShotDMG);
				enemyAI.cooldown = 0;
			}
		}
	}

	void MakeDamage(GameObject origin, int dmg, float distance){

		shootRay.origin = origin.transform.position;
		shootRay.direction = transform.forward;


		if (Physics.Raycast (shootRay, out shootHit, distance)) {

			if (shootHit.collider.GetComponent <PlayerHealth> () != null) {
				shootHit.collider.GetComponent <PlayerHealth> ().TakeDamage (dmg);
			} else if (shootHit.collider.GetComponent <BoxHealth> () != null) {
				shootHit.collider.GetComponent <BoxHealth> ().TakeDamage (dmg);
			}
		}
	}
}