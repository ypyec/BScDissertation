using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{                 
	public GameObject wave;

	public GameObject basicShot;
	public GameObject basicShotOrigin;
	public int basicShotDMG = 5;                  
	public float basicShotCD = 0.65f;

	public GameObject skillTwo;
	public GameObject skillTwoOrigin;
	public int skillTwoDMG = 10;                  
	public float skillTwoCD = 10f;


	public GameObject blockObj;
	public float blockCD = 10f;

	public GameObject superChargeObj;
	public int superChargeDMG = 10;
	public float superChargeCD = 10f;

	Animator anim;
	float [] cooldowns;                                    
	bool attacking;

	void Awake ()
	{
		anim = GetComponent <Animator> ();
		cooldowns = new float[4];
		cooldowns [0] = basicShotCD;
		cooldowns [1] = skillTwoCD;
		cooldowns [2] = blockCD;
		cooldowns [3] = superChargeCD;
	}


	void FixedUpdate ()
	{
		for (int i = 0; i < 4; i++) {
			cooldowns [i] += Time.deltaTime;
		}

		if(	this.anim.GetCurrentAnimatorStateInfo(0).IsName("bigHit") ||
			this.anim.GetCurrentAnimatorStateInfo(0).IsName("block") ||
			this.anim.GetCurrentAnimatorStateInfo(0).IsName("SuperCharge"))
		{
			// Avoid any reload.
			this.attacking = true;
		}
		else if (this.attacking)
		{
			this.attacking = false;
			// You have just leaved your state!
		}

		if (!this.attacking) {
			if(Input.GetButton ("Fire1") && cooldowns[0] >= basicShotCD)
			{
				StartCoroutine (animateAttack ());
			} else if (Input.GetButton ("Fire2") && cooldowns[1] >= skillTwoCD) {
				StartCoroutine (animateSkillTwo ());
			} else if (Input.GetButton ("Fire3") && cooldowns[2] >= skillTwoCD) {
				StartCoroutine(animateBlock ());
			} else if (Input.GetButton ("Fire4") && cooldowns[3] >= superChargeCD) {
				StartCoroutine(animateSuperCharge ());
			} else {
				anim.ResetTrigger ("Hit");
				anim.ResetTrigger ("Hit2");
				anim.ResetTrigger ("Hit3");
				anim.ResetTrigger ("Block");
			}
		}


	}

	IEnumerator animateAttack(){
		cooldowns[0] = 0f;
		anim.SetTrigger ("Hit");

		yield return new WaitForSeconds (0.27f);

		attack ();

	}

	void attack(){

		Vector3 shotPosition = new Vector3 ();
		shotPosition = basicShotOrigin.transform.position;
		shotPosition.y = 0.2f;

		GameObject s1 = (GameObject)Instantiate(basicShot, shotPosition, this.transform.rotation);
		s1.GetComponent<BeamParam>().SetBeamParam(this.GetComponent<BeamParam>());

		GameObject wav = (GameObject)Instantiate(wave, this.transform.position, this.transform.rotation);
		wav.transform.localScale *= 0.25f;
		wav.transform.Rotate(Vector3.left, 90.0f);
		wav.GetComponent<BeamWave>().col = this.GetComponent<BeamParam>().BeamColor;


	}

	IEnumerator animateSkillTwo(){
		cooldowns[1] = 0f;
		anim.SetTrigger ("Hit2");

		yield return new WaitForSeconds (0.55f);

		useSkillTwo ();

	}

	void useSkillTwo(){

		Vector3 shotPosition = new Vector3 ();
		shotPosition = skillTwoOrigin.transform.position;
		shotPosition.y = 0.2f;

		GameObject s1 = (GameObject)Instantiate(skillTwo, shotPosition, this.transform.rotation);
		s1.GetComponent<BeamParam>().SetBeamParam(this.GetComponent<BeamParam>());

		GameObject NowShot = (GameObject)Instantiate(wave, this.transform.position, this.transform.rotation);
		NowShot.transform.localScale *= 0.25f;
		NowShot.transform.Rotate(Vector3.left, 90.0f);
		NowShot.GetComponent<BeamWave>().col = this.GetComponent<BeamParam>().BeamColor;

	}

	IEnumerator animateBlock(){
		cooldowns [2] = 0f;
		anim.SetTrigger ("Block");

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
}