using UnityEngine;
using System.Collections;

public class rewardCoin : MonoBehaviour {

	public Vector3 target;
	private float timer;
	private Vector3 startPos;

	public AnimationCurve animation;

	// Use this for initialization
	void Awake () {
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (timer > 1.5f) {
			transform.position = Vector3.Lerp (startPos, target, animation.Evaluate(timer - 1.5f));

		}
			

	}
}
