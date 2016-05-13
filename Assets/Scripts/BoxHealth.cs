using UnityEngine;
using System.Collections;

public class BoxHealth : MonoBehaviour {

	public int startingHealth = 15;           
	public int currentHealth;
	public int scoreValue = 1;      
	public int healthPackChance = 25;
	public GameObject healthPackObj;
	public GameObject explodeParticles;
	public GameObject enemyToSpawn1;
	//public GameObject enemyToSpawn2;

	                         
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
		ScoreManager.score += scoreValue;

	}
		

	void DropHealthPack(){
		int rng = Random.Range (0, 100);
		if (healthPackChance >= rng) {
			GameObject healthPack = (GameObject)Instantiate (healthPackObj, transform.position, healthPackObj.transform.rotation);
		} else if (rng == 99) {
			GameObject enemyToSpawn = (GameObject)Instantiate (enemyToSpawn1, transform.position, enemyToSpawn1.transform.rotation);
		} else if (rng == 100) {
			//GameObject enemyToSpawn = (GameObject)Instantiate (enemyToSpawn2, transform.position, enemyToSpawn2.transform.rotation);
		}
	}
}
