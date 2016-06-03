using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class KeyManager : MonoBehaviour
{
	public static int keys;

	Text text;

	void Awake (){
		text = GetComponent <Text> ();
		keys = 0;
	}
		
	void Update (){
		text.text = "x " + keys;
	}
}