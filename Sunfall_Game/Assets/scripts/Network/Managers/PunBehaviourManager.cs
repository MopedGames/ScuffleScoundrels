using System.Collections;
using UnityEngine;

public class PunBehaviourManager<T> : Photon.PunBehaviour where T : PunBehaviourManager<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        Instance = this as T;
    }
}