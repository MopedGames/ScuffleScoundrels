using UnityEngine;
using System.Collections;

public class moveAnimation : MonoBehaviour {

	private Vector3 startPos;

	public AnimationCurve animation;
	public float speed;
	public Vector3 distance;

	private float timer;
	// Update is called once per frame

	void Awake () {
		startPos = transform.localPosition;
	}

	void Update () {
		transform.localPosition = startPos +(distance * animation.Evaluate (speed * timer));
		timer += Time.deltaTime;
	}
}
