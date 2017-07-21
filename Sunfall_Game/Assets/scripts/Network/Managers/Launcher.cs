using System.Collections;
using UnityEngine;
using System;

public class Launcher : PunBehaviourManager<Launcher>
{
    /// <summary>
    /// the version of the game the client is running, users are seperated by game version
    /// </summary>
    [Header("Game settings ")]
    [SerializeField, Tooltip("The version of the game the client connects to, change to not interfere with working versions or testing without interference from others")]
    private string gameVersion = "1";

    /// <summary>
    /// the maximum number of players per room, when a room is full it cant be joined by new players and a new room will be created.
    /// </summary>
    [SerializeField, Tooltip("the maximum number of players per room, when a room is full it cant be joined by new players and a new room will be created.")]
    private byte maxPlayersPerRoom = 2;

    /// <summary>
    /// The PUN loglevel
    /// </summary>
    [SerializeField, Tooltip("How much information is kept by the network")]
    private PhotonLogLevel logLevel = PhotonLogLevel.Informational;

    //[SerializeField, Tooltip("The ui panel to let the user enter name, connect to player")]
    //private GameObject controlPanel;

    //[SerializeField, Tooltip("The ui label to inform the user that the connection is in progress")]
    //private GameObject progressLabel;

    /// <summary>
    /// keep track of the current process. Since connection is asynchronous and is based on several callbacksfrom photon
    /// we need to keep track of this to properly adjust the behaviour when we recieve call back by photon
    /// Typically this is used for OnConnectedToMaster() callback
    /// </summary>
    private bool isConnecting;

    public string GameVersion { get { return gameVersion; } }

    protected override void Awake()
    {
        base.Awake();
        //not important
        //force full loglevel
        PhotonNetwork.logLevel = logLevel;
        // we dont need to join the lobby to get the list of rooms
        PhotonNetwork.autoJoinLobby = false;
        // this makes sure we can use photonnetwork.loadlevel on master client and all clients in the same room sync level automatically
        PhotonNetwork.automaticallySyncScene = true;
    }

    private void Start()
    {
        //PhotonNetwork.offlineMode = true; //CMT

        //progressLabel.SetActive(false);
        //controlPanel.SetActive(true);,

        Cursor.visible = true;
    }

    /// <summary>
    /// start the connnection process
    /// - if already connected we attempt to join a random room
    ///  - if not yet conected, connect this application instance to the photon network cloud
    /// </summary>
    public void Connect()
    {
        // keep track of the will to join a rooom, because when we come back from a game we will get a callback that we are connected, so we need to know what to do then..
        isConnecting = true;
        //progressLabel.SetActive(true);
        //controlPanel.SetActive(false);

        if (PhotonNetwork.connected || PhotonNetwork.offlineMode)
        {
            // we need this point to attempt joining a random room. if it fails we'll get notified in OnPhotonRandomJoinFailed(), and we'll create a room
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // we must first connect to the photon online server
            PhotonNetwork.ConnectUsingSettings(gameVersion);
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() called by PUN");

        // we dont want to do anything if we are not attempting to join a room
        // this case where isConnecting is false is typically when you list our quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
        // we dont do anything
        if (isConnecting || PhotonNetwork.offlineMode)
        {
            // the first thing we try to do is to join a potential existing room, if there is one, good , else we'll be called back with OnPhotonRandomJoinFailed()
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("OnDisconnectedFromPhoton() was called by PUN");

        //progressLabel.SetActive(false);
        //controlPanel.SetActive(true);
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("OnPhotonRoomJoinFailed() was called by PUN. No room available, so we create one. \nCalling: PhotonNetwork.Createroom(null, new RoomOptions() {maxPlayers = 2},null;");
        //PhotonNetwork.offlineMode = true; // CMT
        // we failed to join the random room, maybe none exist or they are all full. No worries, we'll create a new one
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = maxPlayersPerRoom }, null); //TODO; moar players
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN, now this client is in a room");

        //we only load if we are the first player , else we rely on photonnetwork.automaticallySyncScene to sync our instance scene
        if (PhotonNetwork.room.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel("Selection");
            //PhotonNetwork.LoadLevel(gameVersion + "LevelFor1"); // change if scene name changes
        }
        if (PhotonNetwork.room.PlayerCount == 2)
        {
            //PhotonNetwork.LoadLevel(gameVersion + "LevelFor2");
        }
    }

    public void hej()
    {
        Debug.Log("hej");
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
        {
            Connect();
        }
    }
}