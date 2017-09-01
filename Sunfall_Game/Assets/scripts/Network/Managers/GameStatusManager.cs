﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Overall game logic and ship assignment
/// </summary>
public class GameStatusManager : PunBehaviourManager<GameStatusManager>
{
    public fuse fuse;
    public PowerUpSpawner powerUps;
    public WinScreen winscreen;

    public List<RectTransform> playerCanvass = new List<RectTransform>();

    public bool suddenDeath;

    public float gameTimer = 120f;

    public List<Player> players = new List<Player>();
    public List<Ship> ships = new List<Ship>();

    public int readyPlayers = 0;

    // Use this for initialization
    private void Start()
    {
        if (winscreen == null)
        {
            winscreen = FindObjectOfType<WinScreen>();
        }
    }

    public void OnLevelWasLoaded()
    {
        MakeReady();
    }

    [PunRPC]
    public void MakeReady()
    {
        foreach (Player p in FindObjectsOfType<Player>())
        {
            players.Add(p);
            {
                if (p.GetComponent<PhotonView>().isMine)
                {
                    p.ship = PhotonNetwork.Instantiate(p.shipPrefab.name, p.shipPrefab.transform.position, p.shipPrefab.transform.rotation, 0).GetComponent<Ship>();
                    photonView.RPC("SpawnShip", PhotonTargets.AllBuffered, p.GetComponent<PhotonView>().ownerId);
                    p.ship.currentStats.speed = 0;
                    photonView.RPC("StartGame", PhotonTargets.AllBuffered);
                }
            }
        }
    }

    [PunRPC]
    public void StartGame()
    {
        readyPlayers++;

        if (readyPlayers == PhotonNetwork.playerList.Length)
        {
            if (PhotonNetwork.isMasterClient)
            {
                // TODO : add upstart sequence 3... 2... 1... START
                suddenDeath = false;

                //fuse.Play();
                powerUps.enabled = true;
                //Debug.Log("I'm 'ere!");

                fuse.GetComponent<PhotonView>().RPC("Play", PhotonTargets.AllBufferedViaServer);

                PhotonNetwork.room.IsOpen = false;
            }

            foreach (Player p in players)
            {
                if (p.GetComponent<PhotonView>().isMine)
                {
                    p.ship.currentStats.speed = 4;
                }
            }
            Debug.Log(readyPlayers + " players ready, set, GO!");
        }
        else
        {
            Debug.Log(readyPlayers + " players ready");
            // TODO: loading screeen
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //if (!suddenDeath)
        //{
        //    gameTimer -= Time.deltaTime;

        //    foreach (Ship s in ships)
        //    {
        //        if (s.kills >= 10)
        //        {
        //            winState = true;
        //            timer = 0f;
        //            gameTimer = 90f;
        //            ShowScreen();
        //        }
        //    }

        //    if (gameTimer < 0)
        //    {
        //        CheckForSuddenDeath();
        //    }
        //}
        //else if (suddenDeath)
        //{
        //    int alive = 0;
        //    Ship lastStanding = null;
        //    foreach (Ship s in ships)
        //    {
        //        if (s.alive)
        //        {
        //            lastStanding = s;
        //            alive++;
        //        }
        //    }
        //    if (alive <= 1)
        //    {
        //        lastStanding.kills += 2;
        //        winState = true;
        //        timer = 0f;
        //        gameTimer = 90f;
        //        ShowScreen();
        //    }
        //}
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
                ships.Add(p.ship);
                winscreen.ships = ships.ToArray();

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

    public void SuddenDeathInit(Ship[] shipsAlive)
    {
        foreach (Ship s in shipsAlive)
        {
            s.invulnerable = true;
        }
        StartCoroutine(suddenDeathEnumartor(shipsAlive));
    }

    private IEnumerator suddenDeathEnumartor(Ship[] shipsAlive)
    {
        //blingAnimation.gameObject.SetActive(true);
        //blingAnimation.Play("Bling");
        yield return new WaitForSeconds(0.30f);
        //fuse.fuseLight.gameObject.SetActive(false);
        //fuse.transform.parent.gameObject.SetActive(false);
        //foreach (TextMesh t in texts)
        //{
        //    t.text = "SUDDEN DEATH";
        //}

        //yield return new WaitForSeconds(1.7f);

        //foreach (TextMesh t in texts)
        //{
        //    t.text = "";
        //}

        //foreach (Ship s in shipsAlive)
        //{
        //    s.invulnerable = false;
        //    s.transform.position = s.startPos;
        //    s.transform.rotation = s.startRot;
        //}

        //blingAnimation.gameObject.SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if (stream.isWriting /*&& PhotonNetwork.isMasterClient*/)
        //{
        //    stream.SendNext(readyPlayers);
        //    //stream.SendNext(fuseLight.transform.position.x);
        //    //stream.SendNext(fuseLight.transform.position.y);
        //}
        //else
        //{
        //    readyPlayers = (int)stream.ReceiveNext();
        //    //fuseLight.transform.position.x = (float)stream.ReceiveNext();
        //}
    }
}