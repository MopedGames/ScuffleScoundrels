using UnityEngine;
using System.Collections;

public class cameraAdjust : MonoBehaviour {

	public Vector2 aspectRatio;
	private Camera camera;

	void Awake () {
		camera = GetComponent<Camera> ();
	}

	// Update is called once per frame
	void Update () {
		Vector2 screenSize = new Vector2 (Screen.width, Screen.height);

		//greater Width = greater number;
		float desiredRatio = aspectRatio.x / aspectRatio.y;
		float currentRatio = screenSize.x / screenSize.y;
		float altRatio = screenSize.y / screenSize.x;
		float altDesired = aspectRatio.y / aspectRatio.x;

		//positive difference = desired is wider than current;
		//negative difference = desired is taller than current;
		float difference = desiredRatio - currentRatio;
		float altDifference = altDesired - altRatio;

		if (difference > 0) {
			float height = 1 + (altDifference / altRatio);
			camera.rect = new Rect(0f,(1f-height)/2f,1f,height);
			

		} else if (difference < 0) {
			float width = 1f + (difference / currentRatio);
			camera.rect = new Rect((1f-width)/2f,0f,width,1f);
		} else {
			camera.rect = new Rect(0f,0f,1f,1f);
		}

	}
}
