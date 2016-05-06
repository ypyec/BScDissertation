using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	public int startingHealth = 100;           
	public int currentHealth;                  
	public int scoreValue = 10;                


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


	public void TakeDamage (int amount)
	{
		if(isDead)
			return;

		currentHealth -= amount;

		if(currentHealth <= 0)
		{
			Death ();
		}
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