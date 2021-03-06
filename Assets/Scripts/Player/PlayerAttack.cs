﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;


 
public class PlayerAttack : MonoBehaviour
{                 
	public GameObject cursor;

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
	public float superChargeStunDuration = 2f;
	public Image superChargeUI;

	public bool empowered;
	public float empowerTime = 5f;
	public GameObject empowerParticles;

	int floorMask;                      
	float camRayLength = 100f;          
	Animator anim;
	float [] cooldowns;
	float [] activeCooldowns;
	bool attacking;
	bool blocking;
	PlayerMovement pm;
	PlayerHealth ph;
	int activeSkill;
	Vector2 activeSkillSize;
	Vector2 unActiveSkillSize;
	Ray shootRay;                                   
	RaycastHit shootHit;
	float originalSpeed;
	AudioSource basicShotAudio;
	AudioSource bigShotAudio;


	void Awake ()
	{
		floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent <Animator> ();
		pm = GetComponent <PlayerMovement> ();
		ph = GetComponent <PlayerHealth> ();
		basicShotAudio = GetComponents<AudioSource> ()[0];
		bigShotAudio = GetComponents<AudioSource> ()[1];
		originalSpeed = pm.speed;

		blocking = false;

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

		if (blocking) {
			pm.speed = originalSpeed*2;
		} else {
			pm.speed = originalSpeed;
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
			StartCoroutine (Turning (3.5f));
			StartCoroutine (animateBlock ());
			break;
		case 3: 
			StartCoroutine (Turning (1.4f));
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

		basicShotAudio.Play ();

		StartCoroutine(MakeDamage (basicShotOrigin, basicShotDMG));


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

		GameObject wav = (GameObject)Instantiate(wave, skillTwoOrigin.transform.position, this.transform.rotation);
		wav.transform.localScale *= 0.25f;
		wav.transform.Rotate(Vector3.left, 90.0f);
		wav.GetComponent<BeamWave>().col = this.GetComponent<BeamParam>().BeamColor;

		bigShotAudio.Play ();

		StartCoroutine(MakeDamage (skillTwoOrigin, skillTwoDMG));


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
		ph.blocking = pm.blocking = blocking = true;
		Vector3 bPos = new Vector3 ();
		bPos = this.transform.position;
		bPos.y = 1.4f;
		GameObject b = (GameObject)Instantiate (blockObj, bPos, this.transform.rotation);
		b.transform.parent = transform; 
		Destroy (b, 3.3f);
	}

	IEnumerator animateSuperCharge(){
		cooldowns [3] = 0f;
		anim.SetTrigger ("Hit3");

		superChargeUI.GetComponentsInChildren <Image> () [1].fillAmount = 1;
		StartCoroutine(skillCoolDown (superChargeUI, superChargeCD, 0));

		yield return new WaitForSeconds (1.1f);

		superCharge ();

	}

	void superCharge(){

		this.GetComponent <SphereCollider> ().enabled = true;
		Vector3 scPos = new Vector3 ();
		scPos = this.transform.position;
		scPos.y = 1;
		GameObject sc = (GameObject)Instantiate (superChargeObj, scPos, this.transform.rotation);
		Destroy (sc, 0.5f);
	}

	IEnumerator Turning (float duration){
		Ray camRay = Camera.main.ScreenPointToRay (cursor.GetComponent<RectTransform> ().anchoredPosition);
		RaycastHit floorHit;

		if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)){
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;
			transform.forward = playerToMouse;
		}

		yield return new WaitForSeconds (duration);
		this.GetComponent <SphereCollider> ().enabled = ph.blocking = pm.blocking = blocking = this.attacking = pm.attacking = false;
		bigShotAudio.Stop ();
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

	void OnTriggerStay(Collider other) {
		if (other.GetComponent <EnemyHealth> ()) {
			other.GetComponent <EnemyHealth> ().TakeDamage (superChargeDMG, superChargeStunDuration);     
		}
	}

	IEnumerator MakeDamage(GameObject origin, int dmg){
		while (attacking) {
			shootRay.origin = origin.transform.position;
			shootRay.direction = transform.forward;


			if (Physics.Raycast (shootRay, out shootHit, this.GetComponent <BeamParam> ().MaxLength)) {

				if (shootHit.collider.GetComponent <EnemyHealth> () != null) {
					shootHit.collider.GetComponent <EnemyHealth> ().TakeDamage (dmg);
				} else if (shootHit.collider.GetComponent <BoxHealth> () != null) {
					shootHit.collider.GetComponent <BoxHealth> ().TakeDamage (dmg);
				}
			}
			yield return new WaitForSeconds(0.25f);
		}
	}

	public void Empower(){
		basicShotDMG = basicShotDMG * 2;
		skillTwoDMG = skillTwoDMG * 2;
		superChargeDMG = superChargeDMG * 2;
		empowered = true;

		GameObject empowerEffect = Instantiate (empowerParticles, transform.position, transform.rotation) as GameObject;
		empowerEffect.transform.parent = transform; 
		Destroy (empowerEffect, empowerTime);
		Invoke ("UnEmpower", empowerTime);
	}


	public void UnEmpower(){
		basicShotDMG = basicShotDMG / 2;
		skillTwoDMG = skillTwoDMG / 2;
		superChargeDMG = superChargeDMG / 2;
		empowered = false;
	}
}