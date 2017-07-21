using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour, IPunObservable
{
    public string startInput;
    public Ship ship;
    public Transform logoMenu;
    public bool playing;
    public GameObject scoreBoard;
    public ParticleSystem landparticle;
    public PlaySound playsound;

    public Animator animator;

    public void Update()
    {
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}