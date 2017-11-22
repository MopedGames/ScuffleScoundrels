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

    /// <summary>
    /// keep track of the current process. Since connection is asynchronous and is based on several callbacksfrom photon
    /// we need to keep track of this to properly adjust the behaviour when we recieve call back by photon
    /// Typically this is used for OnConnectedToMaster() callback
    /// </summary>
    private bool isConnecting;

    [Header("Network options")]
    [SerializeField, Tooltip("set photon to join rooms offline rather than trying to join online multiplayer")]
    public bool manualOfflineMode;

    [SerializeField, Tooltip("Set this to true if you want to load specific scenes based on your personal game version ex. AGSInGameLevel (AGS = the gameversion) - must rename your scene as AGSInGameLevel to correspond to the loaded level ")]
    private bool testVersion;

    [Header("Scenes To Load")]
    [SerializeField, Tooltip("the MainMenu - the starting point for any interaction with the network")]
    private string launcherSceneName;

    [SerializeField, Tooltip("The scene we show the players who are waiting to join a game")]
    private string waitingRoomSceneName;

    [SerializeField, Tooltip("the main in game scene - where the players play the game")]
    private string inGameSceneName;// alternately if different maps are added, this could be a list of "ingamescenes" picked at random

    public int localPlayerAmount;

    public string GameVersion { get { return gameVersion; } }

    public bool TestVersion { get { return testVersion; } }

    public string LauncherSceneName { get { return launcherSceneName; } }

    public string WaitingRoomSceneName { get { return waitingRoomSceneName; } }

    public string InGameSceneName { get { return inGameSceneName; } }

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

    public void OfflineMode (bool offlineMode)
    {
        manualOfflineMode = offlineMode;
        PhotonNetwork.offlineMode = offlineMode;
    }

    private void Start()
    {
        if (manualOfflineMode)
        {
            PhotonNetwork.offlineMode = true; //CMT
        }

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

        if (PhotonNetwork.connected /*|| PhotonNetwork.offlineMode*/) //CMT
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
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("OnPhotonRoomJoinFailed() was called by PUN. No room available, so we create one. \nCalling: PhotonNetwork.Createroom(null, new RoomOptions() {maxPlayers = 2},null;");

        if (manualOfflineMode)
        {
            PhotonNetwork.offlineMode = true; //CMT
        }

        // we failed to join the random room, maybe none exist or they are all full. No worries, we'll create a new one
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = maxPlayersPerRoom }, null); //TODO; moar players
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN, now this client is in a room");

        if (testVersion)
        {
            PhotonNetwork.LoadLevel(gameVersion + waitingRoomSceneName);
        }
        else
        {
            PhotonNetwork.LoadLevel(waitingRoomSceneName); // load selection scene "waitingforplayersoom
        }

        // alternatively insert direct load of ingame level if the PhotonNetwork.Room.PlayerCount == maxplayers
        // that way there will be no "ready check" but games will start when there are enough players
    }

    public void Update()
    {
        //if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
        //{
        //    Connect();
        //}
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Application.Quit();
        //}
    }
}