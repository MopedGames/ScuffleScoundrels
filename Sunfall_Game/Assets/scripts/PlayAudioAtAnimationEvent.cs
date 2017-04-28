using UnityEngine;
using System.Collections;

public class PlayAudioAtAnimationEvent : MonoBehaviour {

	private AudioSource source;

	// Use this for initialization
	void Start () {
		source = GetComponentInChildren<AudioSource> ();

	}

	public void PlayAudio(){
		source.Play ();
	}



}
