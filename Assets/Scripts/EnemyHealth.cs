using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
	public int startingHealth = 100;           
	public int currentHealth;                  
	public int scoreValue = 10;      
	public bool stuned = false;


	Animator anim;                             
	CapsuleCollider capsuleCollider;           
	bool isDead;    

	void Awake ()
	{
		anim = GetComponent <Animator> ();
		capsuleCollider = GetComponent <CapsuleCollider> ();


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
		yield return new WaitForSeconds (duration);
		stuned = false;
	}


	void Death ()
	{
		isDead = true;
		anim.SetTrigger ("Dead");
		GetComponent <NavMeshAgent> ().enabled = false;
		Destroy (gameObject, 2f);

	}

	public bool dead(){
		return isDead;
	}
}