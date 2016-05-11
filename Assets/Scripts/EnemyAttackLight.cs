using UnityEngine;
using System.Collections;

public class EnemyAttackLight : MonoBehaviour
{                 
	public GameObject wave;

	public GameObject basicShot;
	public GameObject basicShotOrigin;
	public int basicShotDMG = 5;                  
	public float basicShotCD = 1.2f;
	public bool attacking;
	public float attackRange = 4f;
	public bool isMelee = false;
	public Vector3 attackposition;

	private NavMeshAgent nav;
	private Animator anim;
	private Transform player;
	private float cooldown;  
	private EnemyHealth enemyHealth;
	Ray shootRay;                                   // A ray from the gun end forwards.
	RaycastHit shootHit;

	void Awake ()
	{
		anim = GetComponent <Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		cooldown = basicShotCD;
		attacking = false;
		enemyHealth = GetComponent <EnemyHealth> ();
		nav = GetComponent<NavMeshAgent> ();
	}


	void Update ()
	{
		cooldown += Time.deltaTime;

		if (!attacking && cooldown >= basicShotCD && Vector3.Distance (transform.position, player.position) < attackRange && !enemyHealth.dead () && !enemyHealth.stuned) {
			if (isMelee) {
				attackMelee ();
			} else {
				StartCoroutine (animateAttack ());
			}

		} else if (attacking && Vector3.Distance (transform.position, attackposition) != 0 && isMelee) {
			attacking = false;
			anim.ResetTrigger ("Hit");
		} else if (!isMelee) {
			attacking = false;
			anim.ResetTrigger ("Hit");
		}
	}

	IEnumerator animateAttack(){
		attacking = true;
		cooldown = 0f;
		Vector3 playerDirection = player.position - transform.position;
		float angleBetween = Vector3.Angle(transform.forward, playerDirection);
		if (angleBetween > 1)
			transform.forward = playerDirection;
		anim.SetTrigger ("Hit");

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

		MakeDamage (basicShotOrigin, basicShotDMG, this.GetComponent <BeamParam> ().MaxLength);
	}

	void attackMelee(){
		attackposition = player.position;
		attacking = true;
		cooldown = 0;
		Vector3 playerDirection = player.position - transform.position;
		float angleBetween = Vector3.Angle(transform.forward, playerDirection);
		if (angleBetween > 1)
			transform.forward = playerDirection;
		nav.speed = 20f;
		print (nav.speed);
		nav.SetDestination (attackposition);

		MakeDamage (basicShotOrigin, basicShotDMG, 1f);
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