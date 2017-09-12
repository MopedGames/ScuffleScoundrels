using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour, IPunObservable
{
    public string startInput;
    public Ship ship;
    public GameObject shipPrefab;
    public Transform logoMenu;
    public bool playing;
    public GameObject scoreBoard;
    public ParticleSystem landparticle;
    public PlaySound playsound;

    public bool joined;
    public bool ready;

    public Animator animator;

    public int playerNumber;

    PhotonView pv;

    public void Start()
    {
        //ready = false;
        pv = GetComponent<PhotonView>();
    }

    public void Update()
    {
        if (ship)
        {
            if (ship.tiltParent)
            {
                ship.tiltParent.transform.eulerAngles = new Vector3(45f, 0f, 0f);
                ship.tiltParent.GetChild(0).transform.localEulerAngles = new Vector3(0f, ship.transform.eulerAngles.y, 0f);
            }
        }
        if (GetComponent<PhotonView>().isMine)
        {
            //Shoot Cannons
            Vector3 force;
            if (ship)
            {
                //if (ship.tiltParent)
                //{
                //    ship.tiltParent.transform.eulerAngles = new Vector3(45f, 0f, 0f);
                //    ship.tiltParent.GetChild(0).transform.localEulerAngles = new Vector3(0f, ship.transform.eulerAngles.y, 0f);
                //}

                if (ship.cannons[0].active)
                {
                    if (Input.GetKeyDown(KeyCode.JoystickButton5))/* || Input.GetKey(controls.shootRightAlt) || Input.GetKey(controls.shootRightKeyboard))*/
                    {
                        force = ship.transform.forward * -1 * ship.currentStats.shootForce;
                        ship.FireCannon(ship.cannons[0], force);
                    }
                }
                if (ship.cannons[1].active)
                {
                    if (Input.GetKey(KeyCode.JoystickButton4))/* || Input.GetKey(controls.shootLeftAlt) || Input.GetKey(controls.shootLeftKeyboard))*/
                    {
                        force = ship.transform.forward * ship.currentStats.shootForce;
                        ship.FireCannon(ship.cannons[1], force);
                    }
                }

                //Move forward
                ship.transform.Translate(Vector3.right * ship.currentStats.speed * Time.deltaTime/*Time.fixedDeltaTime*/); //moves the ship //TODO: Jacobs ship was really sped up!
                ship.transform.position = new Vector3(ship.transform.position.x, 0.5f, ship.transform.position.z); //Makes sure the ship does not fly
                //ship.transform.position = new Vector3(ship.transform.position.x + ship.transform.forward.x * 5, ship.transform.position.y, ship.transform.position.z);

                //Steering
                float leftInt = Convert.ToSingle((Input.GetAxis("Horizontal_Red")));
                float rightInt = Convert.ToSingle((Input.GetAxis("Horizontal_Red")));
                float horizontal = Mathf.Clamp((rightInt - leftInt) + Input.GetAxis(ship.controls.axis), -1f, 1f);
                ship.currentSteering = horizontal * ship.currentStats.steering;
                Vector3 currentRotation = new Vector3(0, ship.currentSteering, 0);
                ship.transform.Rotate(currentRotation * Time.deltaTime/*Time.fixedDeltaTime*/); //TODO: Optimize rotation updates - Maybe send (estimated?) position, and make others lerp to it?
                /*Debug.Log*/
                ship.hull.localEulerAngles = Vector3.Lerp(ship.hull.localEulerAngles, new Vector3(horizontal * 13f, ship.hull.eulerAngles.y, 0), 0.5f);
                //TODO: BUG Random 'flick' here once in a while (Mostly when turning left?)
                //Debug.Log("h" + horizontal);
            }
        }
        else if (ship)
        { //Tilts the ship for everyone else
            ship.hull.localEulerAngles = Vector3.Lerp(ship.hull.localEulerAngles, new Vector3(ship.currentSteering / ship.currentStats.steering * 13f, ship.hull.eulerAngles.y, 0), 0.5f);
            
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(playerNumber);
            stream.SendNext(joined);
            stream.SendNext(ready);
            
        }
        else if (stream.isReading)
        {
            playerNumber = (int)stream.ReceiveNext();
            joined = (bool)stream.ReceiveNext();
            ready = (bool)stream.ReceiveNext();
            
        }
    }
}