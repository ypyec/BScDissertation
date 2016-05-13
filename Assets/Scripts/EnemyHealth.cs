using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
	public int startingHealth = 100;           
	public int currentHealth;                  
	public int scoreValue = 10;      
	public bool stuned = false;
	public GameObject healingParticles;
	public GameObject destroyParticles;
	public GameObject stunParticles;


	Animator anim;                           
	bool isDead;    

	void Awake ()
	{
		anim = GetComponent <Animator> ();


		currentHealth = startingHealth;
		isDead = false;
	}


	public void TakeDamage (int amount, float stunDuration = 0f)
	{
		if(isDead)
			return;

		if (stunDuration == 0) {
			currentHealth -= amount;
		} else if (!stuned) {
			currentHealth -= amount;
			if(currentHealth > 0)
				StartCoroutine (Stun (stunDuration));
		}


		if(currentHealth <= 0)
		{
			Death ();
		}
	}

	IEnumerator Stun (float duration)
	{
		stuned = true;
		anim.SetTrigger ("Stun");
		GameObject stunEffect = Instantiate (stunParticles, transform.position, transform.rotation) as GameObject;
		stunEffect.transform.parent = transform; 
		Destroy (stunEffect, duration);
		yield return new WaitForSeconds (duration);
		stuned = false;
	}


	void Death ()
	{
		isDead = true;
		anim.SetTrigger ("Dead");
		GetComponent <NavMeshAgent> ().enabled = false;
		Destroy (gameObject, 2f);
		GameObject destroyEffect = Instantiate (destroyParticles, transform.position, transform.rotation) as GameObject;
		destroyEffect.transform.parent = transform; 
		ScoreManager.score += scoreValue;

	}


	public bool dead(){
		return isDead;
	}

	public void Heal(){
		currentHealth = startingHealth;
		GameObject recoveryEffect = Instantiate (healingParticles, transform.position, transform.rotation) as GameObject;
		recoveryEffect.transform.parent = transform; 
		Destroy (recoveryEffect, 1f);
	}
}