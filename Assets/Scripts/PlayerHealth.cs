using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
	public int startingHealth = 100;                            // The amount of health the player starts the game with.
	public int currentHealth;                                   // The current health the player has.
	public Slider healthSlider;
	public GameObject healingParticles;

	Animator anim;                                              // Reference to the Animator component.
	PlayerMovement playerMovement;                              // Reference to the player's movement.
	PlayerAttack playerAttack;                              // Reference to the PlayerShooting script.
	bool isDead;                                                


	void Awake ()
	{
		anim = GetComponent <Animator> ();
		playerMovement = GetComponent <PlayerMovement> ();
		playerAttack = GetComponentInChildren <PlayerAttack> ();

		// Set the initial health of the player.
		currentHealth = startingHealth;
	}


	public void TakeDamage (int amount)
	{
		
		currentHealth -= amount;
		healthSlider.value = currentHealth;

		if(currentHealth <= 0 && !isDead)
		{
			Death ();
		}

		if (!isDead) {
			anim.SetTrigger ("Damaged");
		}
	}


	public void Heal(){
		currentHealth = startingHealth;
		GameObject recoveryEffect = Instantiate (healingParticles, transform.position, transform.rotation) as GameObject;
		recoveryEffect.transform.parent = transform; 
		Destroy (recoveryEffect, 1f);
		healthSlider.value = currentHealth;
	}


	void Death ()
	{
		isDead = true;


		anim.SetTrigger ("Die");


		playerMovement.enabled = false;
		playerAttack.enabled = false;
		GetComponent<CapsuleCollider> ().enabled = false;
	}      

}