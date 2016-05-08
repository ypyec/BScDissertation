using UnityEngine;
using System.Collections;

public class BoxHealth : MonoBehaviour {

	public int startingHealth = 30;           
	public int currentHealth;     
	public int healthPackChance = 25;
	public GameObject healthPackObj;
	public GameObject explodeParticles;
	                         
	bool isDestroyed;    

	void Awake ()
	{
		currentHealth = startingHealth;
		isDestroyed = false;
	}


	public void TakeDamage (int amount, float stunDuration = 0f)
	{
		if(isDestroyed)
			return;

		currentHealth -= amount;


		if(currentHealth <= 0)
		{
			Death ();
		}
	}


	void Death ()
	{
		isDestroyed = true;
		GameObject recoveryEffect = Instantiate (explodeParticles, transform.position, transform.rotation) as GameObject;
		recoveryEffect.transform.parent = transform;
		DropHealthPack ();
		Destroy (gameObject, 0.5f);


	}
		

	void DropHealthPack(){
		if (healthPackChance >= Random.Range (0, 100)) {
			GameObject healthPack = (GameObject)Instantiate (healthPackObj, transform.position, healthPackObj.transform.rotation);
		}
	}
}
