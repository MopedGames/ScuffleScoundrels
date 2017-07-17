using UnityEngine;
using System.Collections;

public class PlaySound : MonoBehaviour {

    public AudioClip[] clips;
	AudioSource source;
	Animator animator;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		animator = GetComponent<Animator> ();
	}

	public void Play (){
        if(clips.Length > 0) { 
		    source.pitch = Random.Range (0.6f, 1.4f);
            source.clip = clips[0];
		    source.Play ();
		    animator.SetTrigger ("animationDone");
        }
    }

    public void Play(int clipNumber)
    {
        if (clips.Length > clipNumber)
        {
            source.pitch = Random.Range(0.6f, 1.4f);
            source.clip = clips[clipNumber];
            source.Play();
            animator.SetTrigger("animationDone");
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
