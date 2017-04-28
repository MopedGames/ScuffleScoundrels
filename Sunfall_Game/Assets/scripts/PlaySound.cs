using UnityEngine;
using System.Collections;

public class PlaySound : MonoBehaviour {

	AudioSource source;
	Animator animator;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		animator = GetComponent<Animator> ();
	}

	public void Play (){
		source.pitch = Random.Range (0.6f, 1.4f);
		source.Play ();
		animator.SetTrigger ("animationDone");
	}

	// Update is called once per frame
	void Update () {
	
	}
}
