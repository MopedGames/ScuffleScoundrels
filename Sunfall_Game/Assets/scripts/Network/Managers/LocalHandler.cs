using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class LocalHandler : Photon.PunBehaviour, IPunObservable
{
    [SerializeField, Tooltip("The local player instance. Use thiss to know if the okayer is represented in the scene")]
    private static GameObject localIntance;

    public static GameObject LocalIntance
    {
        get { return localIntance; }
        set { localIntance = value; }
    }

    private void Awake()
    {
        // used in ConnectionHandler: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.isMine)
        {
            LocalHandler.LocalIntance = this.gameObject;
        }

        // we flag as dont destroy on load so that instance survives level synchroni´zation, thus giving a seamles experience when levels load
        DontDestroyOnLoad(this.gameObject);
    }
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        //if (photonView.isMine)
        //{
        //}
        //// Prevent control is connected to Photon and represent the localPlayer
        //if (photonView.isMine == false && PhotonNetwork.connected == true)
        //{
        //    return;
        //}
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}