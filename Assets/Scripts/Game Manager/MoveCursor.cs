using UnityEngine;
using System.Collections;

public class MoveCursor : MonoBehaviour {

	public int joystickSensitivity = 300;

	RectTransform rf;

	void Start () {
		rf = this.GetComponent <RectTransform> ();
		Cursor.visible = false;
		rf.anchoredPosition = new Vector2 (Screen.width / 2, Screen.height / 2);
	}

	void Update () {

		Vector2 newPosition = new Vector2 ();

		if (Input.GetJoystickNames ().Length > 0) {
			newPosition.x = rf.anchoredPosition.x + Input.GetAxis ("RightJoystickX") * joystickSensitivity;
			newPosition.y = rf.anchoredPosition.y + Input.GetAxis ("RightJoystickY") * joystickSensitivity;

			if (newPosition.x >= Screen.width)
				newPosition.x = Screen.width - 16;
			else if (newPosition.x <= 0)
				newPosition.x = 0;

			if (newPosition.y >= Screen.height)
				newPosition.y = Screen.height - 16;
			else if (newPosition.y <= 0)
				newPosition.y = 0;

		} else {
			newPosition.Set (Input.mousePosition.x - 16, Input.mousePosition.y - 16);
		}
		rf.anchoredPosition = newPosition;
	}
}
