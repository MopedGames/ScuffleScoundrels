using UnityEngine;
using System.Collections;

public class playAudioAfterDelay : MonoBehaviour {

	public float delay = 2f;
	private float timer;
	private AudioSource audio;


	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
		StartCoroutine (Delay ());
	}

	// Update is called once per frame
	IEnumerator Delay () {
		
		while (timer < delay) {
			timer += Time.deltaTime;
			yield return null;

		}

		audio.Play();

	}
}
