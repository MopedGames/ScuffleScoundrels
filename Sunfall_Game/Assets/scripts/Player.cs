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

    public bool ready;

    public Animator animator;

    public void Start()
    {
        ready = false;
    }

    public void Update()
    {
        if (GetComponent<PhotonView>().isMine)
        {
            //Shoot Cannons
            Vector3 force;
            if (ship.cannons[0].active)
            {
                if (Input.GetKeyDown(KeyCode.JoystickButton5))/* || Input.GetKey(controls.shootRightAlt) || Input.GetKey(controls.shootRightKeyboard))*/
                {
                    force = transform.forward * -1 * ship.currentStats.shootForce;
                    ship.FireCannon(ship.cannons[0], force);
                }
            }
            if (ship.cannons[1].active)
            {
                if (Input.GetKey(KeyCode.JoystickButton4))/* || Input.GetKey(controls.shootLeftAlt) || Input.GetKey(controls.shootLeftKeyboard))*/
                {
                    force = transform.forward * ship.currentStats.shootForce;
                    ship.FireCannon(ship.cannons[1], force);
                }
            }

            //Move forward
            ship.transform.Translate(Vector3.right * ship.currentStats.speed * Time.fixedDeltaTime);
            ship.transform.position = new Vector3(ship.transform.position.x, 0.5f, ship.transform.position.z);
            //ship.transform.position = new Vector3(ship.transform.position.x + ship.transform.forward.x * 5, ship.transform.position.y, ship.transform.position.z);

            //Steering
            float leftInt = Convert.ToSingle((Input.GetAxis("Horizontal_Red")));
            float rightInt = Convert.ToSingle((Input.GetAxis("Horizontal_Red")));
            float horizontal = Mathf.Clamp((rightInt - leftInt) + Input.GetAxis(ship.controls.axis), -1f, 1f);
            ship.currentSteering = horizontal * ship.currentStats.steering;
            Vector3 currentRotation = new Vector3(0, ship.currentSteering, 0);
            ship.transform.Rotate(currentRotation * Time.fixedDeltaTime);
            ship.hull.localEulerAngles = Vector3.Lerp(ship.hull.localEulerAngles, new Vector3(horizontal * 13f, ship.hull.eulerAngles.y, 0), 0.5f);
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}