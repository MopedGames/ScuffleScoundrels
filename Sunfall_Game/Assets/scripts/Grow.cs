using UnityEngine;
using System.Collections;

public class Grow : MonoBehaviour {

	public AnimationCurve growAnimation;
	public float speed;
	public float size;

	private float timer;
	// Update is called once per frame
	void Update () {
		transform.localScale = Vector3.one * size * growAnimation.Evaluate (speed * timer);
		timer += Time.deltaTime;
	}
}
