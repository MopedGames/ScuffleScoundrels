using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAssignManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        foreach (Player p in FindObjectsOfType<Player>())
        {
            if (p.GetComponent<PhotonView>().isMine)
            {
                p.ship = PhotonNetwork.Instantiate(p.shipPrefab.name, p.shipPrefab.transform.position, p.shipPrefab.transform.rotation, 0).GetComponent<Ship>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
