using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionHandler : PunBehaviourManager<ConnectionHandler>
{
    [SerializeField, Tooltip("tick to show the full debug log from before the players joined the game")]
    private bool showPUNStartup = false;

    [SerializeField, Tooltip("The player")]
    private GameObject playerPrefab;

    [SerializeField]
    private Launcher launcher = Launcher.Instance;

    private void Start()
    {
        Debug.Log("Game Version Loaded: " + launcher.GameVersion);

        if (!PhotonNetwork.connected)
        {
            if (launcher.TestVersion)
            {
                SceneManager.LoadScene(launcher.GameVersion + launcher.LauncherSceneName);
                return;
            }
            else
            {
                SceneManager.LoadScene(launcher.LauncherSceneName);
                return;
            }
        }
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing </a></Color>player prefab reference, Please set it up in the gameobject 'ConnectionHandler'", this);
        }
        else
        {
            if (LocalHandler.LocalIntance == null)
            {
                Debug.Log("We are instantiating the LocalPlayer from " + SceneManagerHelper.ActiveSceneName); //TODO: write better version- (non obsoletee)
                for (int i = 0; i < Launcher.Instance.localPlayerAmount; i++)
                {
                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity, 0);
                }
                photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered); // lets start it UUUP!
#if UNITY_EDITOR
                if (!showPUNStartup)
                {
                    DevMan.Instance.ClearDebug(); // Clear Debug if everything is loaded properly
                }
#endif
            }
            else
            {
                Debug.Log("Ignoring scene load for " + SceneManagerHelper.ActiveSceneName);
            }
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
            //QuitApplication(); //CMT: if leaving the whole game is what you want, this should only be available in the main menu
            if (launcher.TestVersion)
            {
                /*PhotonNetwork.LoadLevel(launcher.GameVersion + launcher.LauncherSceneName);*/ //
                Time.timeScale = 1f;
                PhotonNetwork.LeaveRoom();
            }
            else
            {
                /*  PhotonNetwork.LoadLevel(launcher.LauncherSceneName);*/ //
                Time.timeScale = 1f;
                PhotonNetwork.LeaveRoom();
            }
        }
    }

    private void LoadMatchmaking() //TODO: Rename method as level
    {
        if (!PhotonNetwork.isMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to load a level but we are not the master client");
        }

        //Debug.Log("PhotonNetwork : Loading Level : " + PhotonNetwork.room.PlayerCount); // debug log meant to show which level is being loaded
        //PhotonNetwork.LoadLevel(launcher.GameVersion + "LevelFor" + PhotonNetwork.room.PlayerCount); // TODO: Name scenes loaded by the lobby properly like this. or make it serialized
    }

    /// <summary>
    /// add the players that have joined to a list of players in the selection manager so we can give them their ship
    /// </summary>
    [PunRPC]
    public void AddPlayerToList()
    {
        //int playerID = 1;
        SelectionManager.Instance.currentPlayers.Clear();
        foreach (Player p in FindObjectsOfType<Player>())
        {
            SelectionManager.Instance.currentPlayers.Add(p.gameObject);
            //playerID++;
        }
        //Debug.Log("Current players " + SelectionManager.Instance.currentPlayers.Count);
        //Debug.Log("Photon players " + PhotonNetwork.playerList.Length);
        //if (PhotonNetwork.playerList.Length <= SelectionManager.Instance.currentPlayers.Count)
        //{
        //foreach (GameObject p in SelectionManager.Instance.currentPlayers)
        //{
        //    if (p.GetComponent<PhotonView>().isMine)
        //    {
        //        SelectionManager.Instance.AddPlayer(p.gameObject);
        //    }
        //}
        //}
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("OnPhotonPlayerConnected() " + newPlayer.NickName);



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
        if (photonView != null)
        {
            photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered); // lets start it UUUP! //TODO: Null here when a player quits.
        }
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

        if (launcher.TestVersion)
        {
            SceneManager.LoadScene(launcher.GameVersion + launcher.LauncherSceneName);
            return;
        }
        else
        {
            SceneManager.LoadScene(launcher.LauncherSceneName);
            return;
        }
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
}