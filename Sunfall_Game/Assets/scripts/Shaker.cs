using UnityEngine;
using System.Collections;

public class Shaker : MonoBehaviour {

	private Vector3 startPos;

	public void Shake () {
		StartCoroutine (Shaking ());
	}

	IEnumerator Shaking () {

		yield return new WaitForSeconds (0.2f);

		float shake = 0f;
		//Vector3 startpos = transform.parent.position;
		while (shake < 1f) {
			transform.position = startPos + Random.insideUnitSphere * 0.2f;
			shake += Time.deltaTime * 3f;

			yield return null;
		}

		transform.position = startPos;
	}

	// Use this for initialization
	void Start () {
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
