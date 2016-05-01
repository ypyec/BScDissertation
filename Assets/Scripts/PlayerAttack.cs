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


	Animator anim;
	float [] cooldowns;                                    


	private GameObject NowShot;

	void Awake ()
	{
		NowShot = null;
		anim = GetComponent <Animator> ();
		cooldowns = new float[4];
		cooldowns [0] = basicShotCD;
		cooldowns [1] = skillTwoCD;
	}


	void FixedUpdate ()
	{
		for (int i = 0; i < 4; i++) {
			cooldowns [i] += Time.deltaTime;
		}


		if(Input.GetButton ("Fire1") && cooldowns[0] >= basicShotCD){
			StartCoroutine (animateAttack ());
		} else if (Input.GetButton ("Fire2") && cooldowns[1] >= skillTwoCD) {
			StartCoroutine (animateSkillTwo ());
		} else if (Input.GetAxisRaw ("Fire3") != 0) {
			anim.SetTrigger ("Hit3");
		} else if (Input.GetAxisRaw ("Fire4") != 0) {
			anim.SetTrigger ("Block");
		} else {
			anim.ResetTrigger ("Hit");
			anim.ResetTrigger ("Hit2");
			anim.ResetTrigger ("Hit3");
			anim.ResetTrigger ("Block");
		}
	}

	IEnumerator animateAttack(){
		cooldowns[0] = 0f;
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


	}

	IEnumerator animateSkillTwo(){
		cooldowns[1] = 0f;
		anim.SetTrigger ("Hit2");

		yield return new WaitForSeconds (0.55f);

		useSkillTwo ();

	}

	void useSkillTwo(){

		GameObject s1 = (GameObject)Instantiate(skillTwo, skillTwoOrigin.transform.position, this.transform.rotation);
		s1.GetComponent<BeamParam>().SetBeamParam(this.GetComponent<BeamParam>());

		NowShot = (GameObject)Instantiate(wave, this.transform.position, this.transform.rotation);
		NowShot.transform.localScale *= 0.25f;
		NowShot.transform.Rotate(Vector3.left, 90.0f);
		NowShot.GetComponent<BeamWave>().col = this.GetComponent<BeamParam>().BeamColor;

	}
}