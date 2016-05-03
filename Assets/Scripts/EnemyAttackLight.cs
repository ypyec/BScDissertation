using UnityEngine;
using System.Collections;

public class EnemyAttackLight : MonoBehaviour
{                 
	public GameObject wave;

	public GameObject basicShot;
	public GameObject basicShotOrigin;
	public int basicShotDMG = 5;                  
	public float basicShotCD = 0.65f;

	Animator anim;
	float cooldown;  
	bool attacking;

	void Awake ()
	{
		anim = GetComponent <Animator> ();
		cooldown = basicShotCD;
	}


	void FixedUpdate ()
	{
		cooldown += Time.deltaTime;

		if(Input.GetButton ("Fire1") && cooldown >= basicShotCD)
		{
			StartCoroutine (animateAttack ());
		} else {
			anim.ResetTrigger ("Hit");
		}
	}

	IEnumerator animateAttack(){
		cooldown = 0f;
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
}