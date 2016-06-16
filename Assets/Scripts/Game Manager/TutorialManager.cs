using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour {

	public Canvas mainCanvas;
	public RectTransform tutorial;
	public RectTransform basicShot;
	public RectTransform bigShot;
	public RectTransform shield;
	public RectTransform superCharge;
	public RectTransform health;
	public RectTransform key;
	public RectTransform keyText;
	public Text tutorialText;
	public RectTransform cursor;

	private int index;
	private Transform previousParent;
	private Vector2 activeSkillSize;
	private Vector2 unActiveSkillSize;
	private bool passed;
	private GameObject player;
	private Vector2 defaultPosition;

	void Awake () {
		player = GameObject.FindGameObjectWithTag ("Player");
		mainCanvas = mainCanvas.GetComponent<Canvas> ();
		tutorial = tutorial.GetComponent<RectTransform> ();
		basicShot = basicShot.GetComponent<RectTransform> ();
		bigShot = bigShot.GetComponent<RectTransform> ();
		shield = shield.GetComponent<RectTransform> ();
		superCharge = superCharge.GetComponent<RectTransform> ();
		health = health.GetComponent<RectTransform> ();
		key = key.GetComponent<RectTransform> ();
		keyText = keyText.GetComponent<RectTransform> ();
		tutorialText = tutorialText.GetComponent<Text> ();
		cursor = cursor.GetComponent<RectTransform> ();

		defaultPosition = cursor.anchoredPosition;

		passed = false;
		index = 0;
		previousParent = basicShot.parent;
		activeSkillSize = new Vector2 (60, 60);
		unActiveSkillSize = new Vector2 (50, 50);

		Time.timeScale = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
		switch (index) {
		case 0:
			Movement ();
			break;
		case 1:
			Skills ();
			break;
		case 2:
			Shoot ();
			break;
		}
	}

	void Movement() {
		if(Input.GetJoystickNames().Length > 0)
			tutorialText.text = "Utiliza el analogo izquierdo/L3 para moverte en cualquier dirección";
		else
			tutorialText.text = "Utiliza las teclas w/s para moverte para el frente/atras, las teclas a/d para moverte hacia la derecha/izquierda";
		if (Input.GetAxisRaw ("Vertical") == -1 || Input.GetAxisRaw ("Vertical") == 1 || Input.GetAxisRaw ("Horizontal") == -1 || Input.GetAxisRaw ("Horizontal") == 1) {
			Time.timeScale = 1;
			tutorial.gameObject.SetActive (false);
			passed = true;

		} else if (passed && !Input.anyKey) {
			Time.timeScale = 0;
			tutorial.gameObject.SetActive (true);
			index++;
			passed = false;
		}
	}

	void Skills() {
		
		if(Input.GetJoystickNames().Length > 0)
			tutorialText.text = "Utiliza los botones equis, circulo, cuadrado y triángulo para seleccionar la habilidad que deseas usar";
		else
			tutorialText.text = "Utiliza las teclas del 1 al 4 para seleccionar la habilidad que deseas usar";
		if (Input.GetAxisRaw ("Skill 1") == 1) {
			basicShot.sizeDelta = activeSkillSize;
			bigShot.sizeDelta = unActiveSkillSize;
			shield.sizeDelta = unActiveSkillSize;
			superCharge.sizeDelta = unActiveSkillSize;
			basicShot.SetParent (tutorial.gameObject.transform);
			bigShot.SetParent (previousParent);
			shield.SetParent (previousParent);
			superCharge.SetParent (previousParent);
			tutorial.SetAsLastSibling ();
		} else if (Input.GetAxisRaw ("Skill 2") == 1) {
			basicShot.sizeDelta = unActiveSkillSize;
			bigShot.sizeDelta = activeSkillSize;
			shield.sizeDelta = unActiveSkillSize;
			superCharge.sizeDelta = unActiveSkillSize;
			basicShot.SetParent (previousParent);
			bigShot.SetParent (tutorial.gameObject.transform);
			shield.SetParent (previousParent);
			superCharge.SetParent (previousParent);
			tutorial.SetAsLastSibling ();
		} else if (Input.GetAxisRaw ("Skill 3") == 1) {
			basicShot.sizeDelta = unActiveSkillSize;
			bigShot.sizeDelta = unActiveSkillSize;
			shield.sizeDelta = activeSkillSize;
			superCharge.sizeDelta = unActiveSkillSize;
			basicShot.SetParent (previousParent);
			bigShot.SetParent (previousParent);
			shield.SetParent (tutorial.gameObject.transform);
			superCharge.SetParent (previousParent);
			tutorial.SetAsLastSibling ();
		} else if (Input.GetAxisRaw ("Skill 4") == 1) {
			basicShot.sizeDelta = unActiveSkillSize;
			bigShot.sizeDelta = unActiveSkillSize;
			shield.sizeDelta = unActiveSkillSize;
			superCharge.sizeDelta = activeSkillSize;
			basicShot.SetParent (previousParent);
			bigShot.SetParent (previousParent);
			shield.SetParent (previousParent);
			superCharge.SetParent (tutorial.gameObject.transform);
			tutorial.SetAsLastSibling ();
			passed = true;
		} else if (passed && !Input.anyKey) {
			superCharge.SetParent (previousParent);
			tutorial.SetAsLastSibling ();
			Time.timeScale = 0;
			index++;
			passed = false;
		}
	}

	void Shoot() {
		basicShot.sizeDelta = activeSkillSize;
		bigShot.sizeDelta = unActiveSkillSize;
		shield.sizeDelta = unActiveSkillSize;
		superCharge.sizeDelta = unActiveSkillSize;
		if(Input.GetJoystickNames().Length > 0)
			tutorialText.text = "Utiliza el análogo derecho/R3 para mover la mira de disparo del jugador y el botón R2 para usar la habilidad";
		else
			tutorialText.text = "Utiliza el ratón para mover la mira de disparo del jugador y click izquierdo para usar la habilidad";
		if (Input.GetAxisRaw ("Fire") == 1) {
			tutorial.gameObject.SetActive (false);
			Time.timeScale = 1;
			index++;
			passed = false;
		}
	}	
}
