using UnityEngine;
using System.Collections;

public class PlayAudioAtInterval : MonoBehaviour {

	public float interval = 2f;
	private float timer;
	private AudioSource audio;


	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;
		if (timer >= interval) {
			audio.Play();
			timer = 0f;
		}

	}
}
