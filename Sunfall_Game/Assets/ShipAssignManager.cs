using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipAssignManager : MonoBehaviour
{
    public fuse fuse;
    public PowerUpSpawner powerUps;

    public List<RectTransform> playerCanvass = new List<RectTransform>();

    // Use this for initialization
    void Start()
    {
        fuse.Play();
        powerUps.enabled = true;

        foreach (Player p in FindObjectsOfType<Player>())
        {
            if (p.GetComponent<PhotonView>().isMine)
            {
                p.ship = PhotonNetwork.Instantiate(p.shipPrefab.name, p.shipPrefab.transform.position, p.shipPrefab.transform.rotation, 0).GetComponent<Ship>();
                GetComponent<PhotonView>().RPC("SpawnShip", PhotonTargets.AllBuffered, p.GetComponent<PhotonView>().ownerId);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    [PunRPC]
    private void SpawnShip(int id)
    {
        Player p = null;
        foreach (Player player in FindObjectsOfType<Player>())
        {
            if (player.GetComponent<PhotonView>().ownerId == id)
            {
                p = player;
            }
        }
        if (p != null)
        {
            foreach (Ship s in FindObjectsOfType<Ship>())
            {
                if (s.GetComponent<PhotonView>().ownerId == id)
                {
                    p.ship = s;
                }
            }
            if (p.ship != null)
            {
                p.ship.Spawn();
                p.ship.controls.axis = "Horizontal_Red";
                p.scoreBoard = playerCanvass[p.ship.id].gameObject;
                p.scoreBoard.SetActive(true);
                int counter = 0;
                foreach (Image i in p.scoreBoard.GetComponentsInChildren<Image>())
                {
                    if (i.name.Contains("coin"))
                    {
                        p.ship.coins[counter] = i;
                        i.GetComponent<RectTransform>().localScale = Vector3.zero;
                        counter++;
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("Player is null");
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
