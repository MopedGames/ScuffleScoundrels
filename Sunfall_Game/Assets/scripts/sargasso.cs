using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class sargasso : spawnable
{

    public float speed = 1f;
    public float steering = 20f;
    public AudioClip audio;

    private AudioSource audiosource;

    private List<Ship> ships = new List<Ship>();

    public Transform[] subSargasso;

    void Awake()
    {
        audiosource = Camera.main.GetComponent<AudioSource>();

    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        if (PhotonNetwork.isMasterClient)
        {
            int counter = 0;
            foreach (Transform sarg in subSargasso)
            {
                float rndFloat = Random.Range(0f, 359f);
                sarg.eulerAngles = new Vector3(0f, rndFloat, 0f);
                GetComponent<PhotonView>().RPC("SetSubSargs", PhotonTargets.Others, rndFloat, counter);
                counter++;
            }
        }
    }

    [PunRPC]
    public void SetSubSargs(float rndFloat, int counter, PhotonMessageInfo info)
    {
        if (info.photonView.gameObject == this.gameObject)
        {
            subSargasso[counter].eulerAngles = new Vector3(0f, rndFloat, 0f);
        }
    }

    public override void OnDespawn()
    {

        foreach (Ship s in ships)
        {
            s.ExitSargasso();
        }
        base.OnDespawn();
    }

    public override void DisableThings()
    {
        base.DisableThings();

    }


    public void OnTriggerEnter(Collider col)
    {

        Ship ship = col.gameObject.GetComponent<Ship>();
        if (ship != null)
        {

            //Give ship PowerUp
            audiosource.clip = audio;
            audiosource.Play();

            ship.EnterSargasso(speed, steering);

            //add ship
            ships.Add(ship);
        }

    }

    public void OnTriggerExit(Collider col)
    {

        Ship ship = col.gameObject.GetComponent<Ship>();
        if (ship != null)
        {
            ship.ExitSargasso();

            //add ship
            ships.Remove(ship);

        }

    }

}
