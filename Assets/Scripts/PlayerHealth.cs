using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
	public int startingHealth = 100;
	public int currentHealth;
	public Slider healthSlider;
	public GameObject healingParticles;
	public Image damageImage;
	public float flashSpeed = 5f;
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
	public bool blocking;

	Animator anim;
	PlayerMovement playerMovement;
	PlayerAttack playerAttack;
	bool isDead;                                                
	bool damaged; 

	void Awake ()
	{
		anim = GetComponent <Animator> ();
		playerMovement = GetComponent <PlayerMovement> ();
		playerAttack = GetComponentInChildren <PlayerAttack> ();

		currentHealth = startingHealth;
	}

	void Update ()
	{
		if(damaged){
			damageImage.color = flashColour;
		}
		else{
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}

		damaged = false;
	}

	public void TakeDamage (int amount)
	{
		if (!blocking) {
			currentHealth -= amount;
			healthSlider.value = currentHealth;
			damaged = true;
		}

		if(currentHealth <= 0 && !isDead){
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


	void Death (){
		isDead = true;

		anim.SetTrigger ("Die");

		playerMovement.enabled = false;
		playerAttack.enabled = false;
		GetComponent<CapsuleCollider> ().enabled = false;
	}      

}