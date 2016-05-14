using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
	public int startingHealth = 100;                            // The amount of health the player starts the game with.
	public int currentHealth;                                   // The current health the player has.
	public Slider healthSlider;
	public GameObject healingParticles;
	public Image damageImage;
	public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f); 

	Animator anim;                                              // Reference to the Animator component.
	PlayerMovement playerMovement;                              // Reference to the player's movement.
	PlayerAttack playerAttack;                              // Reference to the PlayerShooting script.
	bool isDead;                                                
	bool damaged; 

	void Awake ()
	{
		anim = GetComponent <Animator> ();
		playerMovement = GetComponent <PlayerMovement> ();
		playerAttack = GetComponentInChildren <PlayerAttack> ();

		// Set the initial health of the player.
		currentHealth = startingHealth;
	}

	void Update ()
	{
		// If the player has just been damaged...
		if(damaged)
		{
			// ... set the colour of the damageImage to the flash colour.
			damageImage.color = flashColour;
		}
		// Otherwise...
		else
		{
			// ... transition the colour back to clear.
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}

		// Reset the damaged flag.
		damaged = false;
	}

	public void TakeDamage (int amount)
	{
		
		currentHealth -= amount;
		healthSlider.value = currentHealth;
		damaged = true;

		if(currentHealth <= 0 && !isDead)
		{
			Death ();
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