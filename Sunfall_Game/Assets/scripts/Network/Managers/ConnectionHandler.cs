﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionHandler : PunBehaviourManager<ConnectionHandler>
{
    [SerializeField, Tooltip("tick to show the full debug log from before the players joined the game")]
    private bool showPUNStartup = false;

    private string levelForOneName;
    private string levelForTwoName;

    [SerializeField, Tooltip("The player")]
    private GameObject playerPrefab;

    static public ConnectionHandler CHInstance;
    private GameObject instance;

    [SerializeField]
    private Launcher launcher = Launcher.Instance;

    public Launcher Launcher { get { return launcher; } }

    public string LevelForOneName { get { return levelForOneName; } }

    public string LevelForTwoName { get { return levelForTwoName; } }

    private void Start()
    {
        //levelForOneName = launcher.GameVersion + "LevelFor1";
        //levelForTwoName = launcher.GameVersion + "LevelFor2";

        CHInstance = this;
        Debug.Log("Game Version Loaded: " + launcher.GameVersion);

        if (!PhotonNetwork.connected)
        {
            SceneManager.LoadScene("OnlineMenu");
            return;
        }
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing </a></Color>player prefab reference, Please set it up in the gameobject 'ConnectionHandler'", this);
        }
        else
        {
            if (LocalHandler.LocalIntance == null)
            {
                //if (GameStatusManager.Instance)
                {
                    Debug.Log("We are instantiating the LocalPlayer from " + SceneManagerHelper.ActiveSceneName); //TODO: write better version- (non obsoletee)
                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity, 0);
                    photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered); // lets start it UUUP!
                    //FindObjectOfType<SelectionManager>().currentPlayers.Add(go.GetComponent<Player>());
#if UNITY_EDITOR
                    if (!showPUNStartup)
                    {
                        DevMan.Instance.ClearDebug(); // Clear Debug if everything is loaded properly
                    }
#endif
                }
            }
            else
            {
                Debug.Log("Ignoring scene load for " + SceneManagerHelper.ActiveSceneName);
            }
            //playerPrefab.GetComponent<PhotonView>().RPC("SetPlayerDistinction", PhotonTargets.AllBufferedViaServer);

            // we are in a rooom. spawn the player. it gets synced by using PhotonNetwork.Instantiate
        }
    }

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity on every frame.
    /// </summary>
    private void Update()
    {
        // "back" button of phone equals "Escape". quit app if that's pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitApplication();
        }
    }

    private void LoadMatchmaking() //TODO: Rename method as level
    {
        if (!PhotonNetwork.isMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to load a level but we are not the master client");
        }

        Debug.Log("PhotonNetwork : Loading Level : " + PhotonNetwork.room.PlayerCount);
        //PhotonNetwork.LoadLevel(launcher.GameVersion + "LevelFor" + PhotonNetwork.room.PlayerCount); // TODO: Name scenes loaded by the lobby properly like this. or make it serialized
    }

    [PunRPC]
    public void AddPlayerToList()
    {
        SelectionManager.Instance.currentPlayers.Clear();
        foreach (Player p in FindObjectsOfType<Player>())
        {
            SelectionManager.Instance.currentPlayers.Add(p.gameObject);
        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("OnPhotonPlayerConnected() " + newPlayer.NickName);

        //SelectionManager.Instance.photonPlayers.Add(newPlayer);



        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient);

            //LoadMatchmaking(); //TODO: load matchmaking when the players are all ready
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log("OnPhotonPlayerDisconnected() " + otherPlayer.NickName);
        //GameStatusManager.Instance.photonView.RPC("CleanupPlayerList", PhotonTargets.All, otherPlayer.ID);
        photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered); // lets start it UUUP!
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerDisconnected isMasterClient " + PhotonNetwork.isMasterClient);
            //LoadMatchmaking();
        }
        //GameStatusManager.Instance.GetComponent<PhotonView>().RPC("CleanupPlayerList", PhotonTargets.All);
        //GameStatusManager.Instance.StartCoroutine(GameStatusManager.Instance.Delay(0f)); // check players, is the game over?
    }

    /// <summary>
    /// called when the local player left the room. We need to load the launcher scene
    /// </summary>
    public override void OnLeftRoom()
    {
        //GameStatusManager.Instance.StartCoroutine(GameStatusManager.Instance.Delay(GameStatusManager.Instance.GameOverFadeDelay)); // check players, is the game over?

        base.OnLeftRoom();

        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void QuitApplication()
    {
        //GameStatusManager.Instance.StartCoroutine(GameStatusManager.Instance.Delay(0f)); // check players, is the game over?

        //GameStatusManager.Instance.GetComponent<PhotonView>().RPC("CleanupPlayerList", PhotonTargets.All);
        Application.Quit();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new NotImplementedException();
    }

    public void hej()
    {
        Debug.Log("hej");
    }
}