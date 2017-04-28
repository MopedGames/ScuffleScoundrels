using UnityEngine;
using System.Collections;

public class ArcadeControlsTest : MonoBehaviour {


	public KeyCode keycode;


	void Start () {
		string[] joysticks = Input.GetJoystickNames ();
		foreach (string s in joysticks) {
			Debug.Log (s);
		}


	}

	void OnGUI(){

		Event e = Event.current;

		if (Input.anyKeyDown) {
			Debug.Log ("hey" + e.keyCode);
		}
	}

	// Update is called once per frame
	void Update () {
		if(Input.anyKeyDown){

			print(Input.inputString);

		}
		//Joystick2 = Player1 1Controller
		//Joystick1 = Player2 1Controller

		//Joystick? 4 = Player1 2Controller
		//Joystick? 3 = Player2 2Controller

		/*
		if (Input.GetKeyDown (KeyCode.Joystick2Button0)) {
			Debug.Log ("joystick 0");
		}
		if (Input.GetKeyDown (KeyCode.Joystick2Button1)) {
			Debug.Log ("joystick 1");
		}
		if (Input.GetKeyDown (KeyCode.Joystick2Button2)) {
			Debug.Log ("joystick 2");
		}
		if (Input.GetKeyDown (KeyCode.Joystick2Button3)) {
			Debug.Log ("joystick 3");
		}
		if (Input.GetKeyDown (KeyCode.Joystick1Button0)) {
			Debug.Log ("joystick 0");
		}
		if (Input.GetKeyDown (KeyCode.Joystick1Button1)) {
			Debug.Log ("joystick 1");
		}
		if (Input.GetKeyDown (KeyCode.Joystick1Button2)) {
			Debug.Log ("joystick 2");
		}
		if (Input.GetKeyDown (KeyCode.Joystick1Button3)) {
			Debug.Log ("joystick 3");
		}
		if (Input.GetKeyDown (KeyCode.Joystick3Button0)) {
			//P2 knap 9
			Debug.Log ("joystick 0");
		}
		if (Input.GetKeyDown (KeyCode.Joystick3Button1)) {
			//P2 knap 10
			Debug.Log ("joystick 1");
		}
		if (Input.GetKeyDown (KeyCode.Joystick3Button2)) {
			//P2 knap 10
			Debug.Log ("joystick 2");
		}
		if (Input.GetKeyDown (KeyCode.Joystick3Button3)) {
			//P2 knap 10
			Debug.Log ("joystick 3");
		}
		if (Input.GetKeyDown (KeyCode.Joystick4Button0)) {
			//P2 knap 10
			Debug.Log ("joystick 0");
		}
		if (Input.GetKeyDown (KeyCode.Joystick4Button1)) {
			//P2 knap 10
			Debug.Log ("joystick 1");
		}
		if (Input.GetKeyDown (KeyCode.Joystick4Button2)) {
			//P2 knap 10
			Debug.Log ("joystick 2");
		}
		if (Input.GetKeyDown (KeyCode.Joystick4Button3)) {
			//P2 knap 10
			Debug.Log ("joystick 3");
		}

		*/
	}
}
