using UnityEngine;
using System.Collections;

public class windBehaviour : MonoBehaviour {

	public Vector3 windMin;
	public Vector3 windMax;

	public Vector2 timing;

	private float timer;
	private float chosenTime;
	private Cloth cloth;

	private Vector3 acc;

	// Use this for initialization
	void Start () {
		cloth = GetComponent<Cloth> ();
	}

	void change () {
		acc = Vector3.Lerp (windMin, windMax, Random.value);
		chosenTime = Mathf.Lerp (timing.x, timing.y, Random.value);
		timer = 0f;
	}

	// Update is called once per frame
	void Update () {

		cloth.externalAcceleration = Vector3.Lerp (cloth.externalAcceleration, acc, 0.1f);

		timer += Time.deltaTime;
		if (timer > chosenTime) {
			change ();
		}
	}
}
