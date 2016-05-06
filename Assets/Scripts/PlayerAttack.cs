using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{                 
	public GameObject wave;

	public GameObject basicShot;
	public GameObject basicShotOrigin;
	public int basicShotDMG = 5;                  
	public float basicShotCD = 0.95f;
	public Image basicShotUI;

	public GameObject skillTwo;
	public GameObject skillTwoOrigin;
	public int skillTwoDMG = 10;                  
	public float skillTwoCD = 10f;
	public Image skillTwoUI;

	public GameObject blockObj;
	public float blockCD = 10f;
	public Image blockUI;

	public GameObject superChargeObj;
	public int superChargeDMG = 10;
	public float superChargeCD = 10f;
	public Image superChargeUI;


	Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
	int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
	float camRayLength = 100f;          // The length of the ray from the camera into the scene.
	Animator anim;
	float [] cooldowns;
	float [] activeCooldowns;
	bool attacking;
	PlayerMovement pm;
	int activeSkill;
	Vector2 activeSkillSize;
	Vector2 unActiveSkillSize;
	Ray shootRay;                                   // A ray from the gun end forwards.
	RaycastHit shootHit;

	void Awake ()
	{
		floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
		pm = GetComponent <PlayerMovement> ();

		activeSkill = 0;

		activeSkillSize = new Vector2 (60, 60);
		unActiveSkillSize = new Vector2 (50, 50);

		cooldowns = new float[4];
		cooldowns [0] = basicShotCD;
		cooldowns [1] = skillTwoCD;
		cooldowns [2] = blockCD;
		cooldowns [3] = superChargeCD;

		activeCooldowns = new float[4];
		activeCooldowns [0] = basicShotCD;
		activeCooldowns [1] = skillTwoCD;
		activeCooldowns [2] = blockCD;
		activeCooldowns [3] = superChargeCD;
	}


	void FixedUpdate ()
	{
		for (int i = 0; i < 4; i++) {
			cooldowns [i] += Time.deltaTime;
		}



		if (Input.GetButton ("Skill 1")) {
			activeSkill = 0;
			basicShotUI.rectTransform.sizeDelta = activeSkillSize;
			skillTwoUI.rectTransform.sizeDelta = unActiveSkillSize;
			blockUI.rectTransform.sizeDelta = unActiveSkillSize;
			superChargeUI.rectTransform.sizeDelta = unActiveSkillSize;
		} else if (Input.GetButton ("Skill 2")) {
			activeSkill = 1;
			basicShotUI.rectTransform.sizeDelta = unActiveSkillSize;
			skillTwoUI.rectTransform.sizeDelta = activeSkillSize;
			blockUI.rectTransform.sizeDelta = unActiveSkillSize;
			superChargeUI.rectTransform.sizeDelta = unActiveSkillSize;
		} else if (Input.GetButton ("Skill 3")) {
			activeSkill = 2;
			basicShotUI.rectTransform.sizeDelta = unActiveSkillSize;
			skillTwoUI.rectTransform.sizeDelta = unActiveSkillSize;
			blockUI.rectTransform.sizeDelta = activeSkillSize;
			superChargeUI.rectTransform.sizeDelta = unActiveSkillSize;
		} else if (Input.GetButton ("Skill 4")) {
			activeSkill = 3;
			basicShotUI.rectTransform.sizeDelta = unActiveSkillSize;
			skillTwoUI.rectTransform.sizeDelta = unActiveSkillSize;
			blockUI.rectTransform.sizeDelta = unActiveSkillSize;
			superChargeUI.rectTransform.sizeDelta = activeSkillSize;
		}


		if (!this.attacking) {


			if(Input.GetButton ("Fire") && cooldowns[activeSkill] >= activeCooldowns[activeSkill]){
				attack (activeSkill);
			} else {
				anim.ResetTrigger ("Hit");
				anim.ResetTrigger ("Hit2");
				anim.ResetTrigger ("Hit3");
				anim.ResetTrigger ("Block");
			}
		}


	}

	void attack(int skill){
		this.attacking = pm.attacking = true;
		switch (skill) {
		case 0:
			StartCoroutine (Turning (0.5f));
			StartCoroutine (animateSkillOne ());
			break;
		case 1:
			StartCoroutine (Turning (1.8f));
			StartCoroutine (animateSkillTwo ());
			break;
		case 2:
			StartCoroutine (Turning (3.3f));
			StartCoroutine (animateBlock ());
			break;
		case 3: 
			StartCoroutine (Turning (2.5f));
			StartCoroutine (animateSuperCharge ());
			break;
		default:
			break;
		}
	}


	IEnumerator animateSkillOne(){

		cooldowns[0] = 0f;
		anim.SetTrigger ("Hit");

		basicShotUI.GetComponentsInChildren <Image> () [1].fillAmount = 1;
		StartCoroutine(skillCoolDown (basicShotUI, basicShotCD, 0));

		yield return new WaitForSeconds (0.27f);

		useSkillOne();

	}

	void useSkillOne(){

		GameObject s1 = (GameObject)Instantiate(basicShot, basicShotOrigin.transform.position, this.transform.rotation);
		s1.GetComponent<BeamParam>().SetBeamParam(this.GetComponent<BeamParam>());

		GameObject wav = (GameObject)Instantiate(wave, basicShotOrigin.transform.position, this.transform.rotation);
		wav.transform.localScale *= 0.25f;
		wav.transform.Rotate(Vector3.left, 90.0f);
		wav.GetComponent<BeamWave>().col = this.GetComponent<BeamParam>().BeamColor;

		shootRay.origin = basicShotOrigin.transform.position;
		shootRay.direction = transform.forward;


		if (Physics.Raycast (shootRay, out shootHit, this.GetComponent <BeamParam> ().MaxLength)) {

			EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();

			if (enemyHealth != null) {
				enemyHealth.TakeDamage (basicShotDMG);
			}
		}


	}

	IEnumerator animateSkillTwo(){
		cooldowns[1] = 0f;
		anim.SetTrigger ("Hit2");

		skillTwoUI.GetComponentsInChildren <Image> () [1].fillAmount = 1;
		StartCoroutine(skillCoolDown (skillTwoUI, skillTwoCD, 0));

		yield return new WaitForSeconds (0.55f);

		useSkillTwo ();

	}

	void useSkillTwo(){

		GameObject s1 = (GameObject)Instantiate(skillTwo, skillTwoOrigin.transform.position, this.transform.rotation);
		s1.GetComponent<BeamParam>().SetBeamParam(this.GetComponent<BeamParam>());

		GameObject NowShot = (GameObject)Instantiate(wave, skillTwoOrigin.transform.position, this.transform.rotation);
		NowShot.transform.localScale *= 0.25f;
		NowShot.transform.Rotate(Vector3.left, 90.0f);
		NowShot.GetComponent<BeamWave>().col = this.GetComponent<BeamParam>().BeamColor;

		shootRay.origin = basicShotOrigin.transform.position;
		shootRay.direction = transform.forward;


		if (Physics.Raycast (shootRay, out shootHit, this.GetComponent <BeamParam> ().MaxLength)) {

			EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();

			if (enemyHealth != null) {
				enemyHealth.TakeDamage (skillTwoDMG);
			}
		}

	}

	IEnumerator animateBlock(){
		cooldowns [2] = 0f;
		anim.SetTrigger ("Block");

		blockUI.GetComponentsInChildren <Image> () [1].fillAmount = 1;
		StartCoroutine(skillCoolDown (blockUI, blockCD, 0));

		yield return new WaitForSeconds (0.1f);

		block ();

	}

	void block(){

		Vector3 bPos = new Vector3 ();
		bPos = this.transform.position;
		bPos.y = 1;
		GameObject b = (GameObject)Instantiate (blockObj, bPos, this.transform.rotation);
		Destroy (b, 3.3f);
	}

	IEnumerator animateSuperCharge(){
		cooldowns [3] = 0f;
		anim.SetTrigger ("Hit3");

		superChargeUI.GetComponentsInChildren <Image> () [1].fillAmount = 1;
		StartCoroutine(skillCoolDown (superChargeUI, superChargeCD, 0));

		yield return new WaitForSeconds (2.2f);

		superCharge ();

	}

	void superCharge(){

		Vector3 scPos = new Vector3 ();
		scPos = this.transform.position;
		scPos.y = 1;
		GameObject sc = (GameObject)Instantiate (superChargeObj, scPos, this.transform.rotation);
		Destroy (sc, 0.5f);
	}

	IEnumerator Turning (float duration)
	{
		// Create a ray from the mouse cursor on screen in the direction of the camera.
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		// Create a RaycastHit variable to store information about what was hit by the ray.
		RaycastHit floorHit;

		// Perform the raycast and if it hits something on the floor layer...
		if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
		{
			// Create a vector from the player to the point on the floor the raycast from the mouse hit.
			Vector3 playerToMouse = floorHit.point - transform.position;

			// Ensure the vector is entirely along the floor plane.
			playerToMouse.y = 0f;

			transform.forward = playerToMouse;


		}



		yield return new WaitForSeconds (duration);

		this.attacking = pm.attacking = false;
	}

	IEnumerator skillCoolDown(Image skill, float cd, float time) {

		Image skillCD = skill.GetComponentsInChildren <Image> () [1];

		while (skillCD.fillAmount > 0) {
			time += Time.deltaTime / cd;
			if (skill.Equals (basicShotUI)) {
				skillCD.fillAmount = Mathf.Lerp (1, 0, time + 0.1f);
			} else {
				skillCD.fillAmount = Mathf.Lerp (1, 0, time);
			}
			yield return null;
		}
	}
}