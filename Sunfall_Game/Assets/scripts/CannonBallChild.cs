using UnityEngine;
using System.Collections;

public class CannonBallChild: MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnCollisionEnter (Collision col) {
		GetComponentInParent<cannonBall> ().OnCollisionEnter(col);
	}
}
