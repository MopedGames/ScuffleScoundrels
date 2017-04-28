using UnityEngine;
using System.Collections;

public class rotateAnimation : MonoBehaviour {

	private Vector3 startRot;

	public AnimationCurve animation;
	public float speed;
	public Vector3 distance;

	private float timer;
	// Update is called once per frame

	void Awake () {
		startRot = transform.localEulerAngles;
	}

	void Update () {
		transform.localEulerAngles = startRot +(distance * animation.Evaluate (speed * timer));
		timer += Time.deltaTime;
	}
}
