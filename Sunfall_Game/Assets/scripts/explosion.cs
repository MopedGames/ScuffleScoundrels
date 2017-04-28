using UnityEngine;
using System.Collections;

public class explosion : MonoBehaviour {

	private Shaker shaker;

	// Use this for initialization
	void Awake () {
		shaker = FindObjectOfType<Shaker> ();
		shaker.Shake ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
