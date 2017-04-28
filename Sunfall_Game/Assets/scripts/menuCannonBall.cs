using UnityEngine;
using System.Collections;

public class menuCannonBall : MonoBehaviour {

	private Vector3 startPos;

	public AnimationCurve curve;
	public Vector3 distance;
	public float duration;
	private float timer;

	// Use this for initialization
	void Awake () {
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (startPos, startPos + distance, timer / duration) + new Vector3 (0f, curve.Evaluate (timer / duration), 0f);
		if (timer > duration) {
			Destroy (this.gameObject);
		}

		timer += Time.deltaTime;
	}
}
