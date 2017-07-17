using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class sargasso : spawnable {

	public float speed = 1f;
	public float steering = 20f;
	public AudioClip audio;

	private AudioSource audiosource;

	private List<Ship> ships = new List<Ship>();

    public Transform[] subSargasso;

	void Awake () {
		audiosource = Camera.main.GetComponent<AudioSource> ();

	}

    public override void OnSpawn()
    {
       
        foreach (Transform sarg in subSargasso)
        {
            sarg.eulerAngles = new Vector3(0f,Random.Range(0f,359f),0f);
        }
    }

	public override void OnDespawn(){

		foreach (Ship s in ships) {
			s.ExitSargasso ();

		}
	}

	public void OnTriggerEnter (Collider col) {

		Ship ship = col.gameObject.GetComponent<Ship>();
		if(ship != null){

			//Give ship PowerUp
			audiosource.clip = audio;
			audiosource.Play();

			ship.EnterSargasso (speed, steering);

			//add ship
			ships.Add(ship);
		}

	}

	public void OnTriggerExit (Collider col) {

		Ship ship = col.gameObject.GetComponent<Ship>();
		if(ship != null){
			ship.ExitSargasso ();

			//add ship
			ships.Remove(ship);

		}

	}

}
