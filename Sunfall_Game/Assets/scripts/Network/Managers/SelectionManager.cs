using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : PunBehaviourManager<SelectionManager>
{
    #region Field

    [SerializeField, Tooltip("Players currently active in the scene")]
    public List<GameObject> currentPlayers;

    [SerializeField]
    public List<PhotonPlayer> photonPlayers = new List<PhotonPlayer>();

    public List<GameObject> decks = new List<GameObject>();

    public List<GameObject> shipPrefabs = new List<GameObject>();

    public int startScreen = 3;

    private float countdown = 3.0f;
    private float exitCountDown = 3.9f;
    public UnityEngine.UI.Text[] countDownText;
    public UnityEngine.UI.Text exitCountDownText;
    public Renderer countDownBorder;

    public int playerNumber = 0;
    public int playerReady = 0;

    public bool isPlaying = false;

    public GameObject getReadyUI;
    public GameObject wannaLeaveUI;

    public GameObject controls;

    public musicPlayer musicPlayer;
    public AudioSource selectionMusic;

    #endregion Field

    public void Start()
    {
        //Debug.Log("Selection manager start " + currentPlayers.Count);
        ConnectionHandler.Instance.photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered);
        musicPlayer.Play(selectionMusic);
    }

    public void Update()
    {
        //Debug.Log(photonPlayers.Count);

        foreach (GameObject p in currentPlayers)
        {
            if (p != null)
            {
                Player player = p.GetComponent<Player>();
                DoDeckAnimations(player);
            }
            else
            {
                currentPlayers.Remove(p);
                break;
            }
        }

        //foreach deck that is aktive, check if player is there
        foreach (GameObject deck in decks)
        {
            if (deck.activeSelf)
            {
                bool isOkay = false;
                int deckIndex = decks.IndexOf(deck);
                foreach (GameObject player in currentPlayers)
                {
                    if (player.GetComponent<Player>().playerNumber - 1 == deckIndex)
                    {
                        isOkay = true;
                    }
                }
                if (isOkay == false)
                {
                    deck.GetComponent<Animator>().SetTrigger("Hidden");
                    deck.SetActive(false);
                }
            }
        }
        Inputs();
    }

    public void DoDeckAnimations(Player player)
    {
        if (player.playerNumber > 0)
        {
            if (decks[player.playerNumber - 1].activeSelf != true)
            {
                decks[player.playerNumber - 1].SetActive(true);
                decks[player.playerNumber - 1].GetComponent<Animator>().SetTrigger("Show");
                foreach (GameObject go in currentPlayers)
                {
                    if (go.GetComponent<Player>().playerNumber == player.playerNumber)
                    {
                        if (go.GetComponent<Player>().ready == true)
                        {
                            decks[player.playerNumber - 1].GetComponent<Animator>().SetTrigger("Aktivate");
                        }
                    }
                }

                //decks[PhotonNetwork.player.ID - 1].landparticle.Play();
                //decks[PhotonNetwork.player.ID - 1].GetComponent<PlaySound>().Play(1);
            }
        }
    }

    private void Inputs()
    {
        int counter = 0;
        foreach (GameObject go in currentPlayers)
        {
            if (go.GetComponent<PhotonView>().isMine/* == PhotonNetwork.player.ID*/)
            {
                counter++;
                switch (counter)
                {
                    case 1:
                        if (Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetButtonUp("Fire1_1"))
                        {
                            if (wannaLeaveUI.GetActive() == true)
                            {
                                PhotonNetwork.LeaveRoom(/*Launcher.Instance.LauncherSceneName*/);
                            }
                            else
                            {
                                if (go.GetComponent<Player>().joined && !go.GetComponent<Player>().ready)
                                {
                                    Debug.Log("Ready");
                                    photonView.RPC("PlayerReady", PhotonTargets.All, /*player.playerNumber */go.GetComponent<PhotonView>().viewID);
                                }
                                else
                                {
                                    Debug.Log("Joining");
                                    AddPlayer(go);
                                    go.GetComponent<Player>().joystickNumber = 1;
                                }
                            }
                        }
                        if (Input.GetKeyUp(KeyCode.Joystick1Button1) || Input.GetButtonUp("Fire2_1"))
                        {
                            if (go.GetComponent<PhotonView>().isMine/* == PhotonNetwork.player.ID*/)
                            {
                                if (wannaLeaveUI.GetActive() == true)
                                {
                                    wannaLeaveUI.SetActive(false);
                                }
                                else
                                {
                                    if (go.GetComponent<Player>().joined && !go.GetComponent<Player>().ready)
                                    {
                                        Debug.Log("Removing");
                                        RemovePlayer(go);
                                    }
                                    else if (go.GetComponent<Player>().ready)
                                    {
                                        photonView.RPC("PlayerUnReady", PhotonTargets.All, /*player.playerNumber */go.GetComponent<PhotonView>().viewID);
                                    }
                                    else
                                    {
                                        wannaLeaveUI.SetActive(true);
                                    }
                                }
                            }
                        }
                        break;

                    case 2:
                        if (Input.GetKeyUp(KeyCode.Joystick2Button0) || Input.GetKeyUp(KeyCode.Alpha2) || Input.GetButtonUp("Fire1_2"))
                        {
                            if (wannaLeaveUI.GetActive() == true)
                            {
                                PhotonNetwork.LeaveRoom(/*Launcher.Instance.LauncherSceneName*/);
                            }
                            else
                            {
                                if (go.GetComponent<Player>().joined && !go.GetComponent<Player>().ready)
                                {
                                    Debug.Log("Ready");
                                    photonView.RPC("PlayerReady", PhotonTargets.All, /*player.playerNumber */go.GetComponent<PhotonView>().viewID);
                                }
                                else
                                {
                                    Debug.Log("Joining");
                                    AddPlayer(go);
                                    go.GetComponent<Player>().joystickNumber = 2;
                                }
                            }
                        }
                        if (Input.GetKeyUp(KeyCode.Joystick2Button1) || Input.GetKeyUp(KeyCode.C) || Input.GetButtonUp("Fire2_2"))
                        {
                            if (go.GetComponent<PhotonView>().isMine/* == PhotonNetwork.player.ID*/)
                            {
                                if (wannaLeaveUI.GetActive() == true)
                                {
                                    wannaLeaveUI.SetActive(false);
                                }
                                else
                                {
                                    if (go.GetComponent<Player>().joined && !go.GetComponent<Player>().ready)
                                    {
                                        Debug.Log("Removing");
                                        RemovePlayer(go);
                                    }
                                    else if (go.GetComponent<Player>().ready)
                                    {
                                        photonView.RPC("PlayerUnReady", PhotonTargets.All, /*player.playerNumber */go.GetComponent<PhotonView>().viewID);
                                    }
                                    else
                                    {
                                        wannaLeaveUI.SetActive(true);
                                    }
                                }
                            }
                        }
                        break;

                    case 3:
                        if (Input.GetKeyUp(KeyCode.Joystick3Button0) || Input.GetKeyUp(KeyCode.Alpha3) || Input.GetButtonUp("Fire1_3"))
                        {
                            if (wannaLeaveUI.GetActive() == true)
                            {
                                PhotonNetwork.LeaveRoom(/*Launcher.Instance.LauncherSceneName*/);
                            }
                            else
                            {
                                if (go.GetComponent<Player>().joined && !go.GetComponent<Player>().ready)
                                {
                                    Debug.Log("Ready");
                                    photonView.RPC("PlayerReady", PhotonTargets.All, /*player.playerNumber */go.GetComponent<PhotonView>().viewID);
                                }
                                else
                                {
                                    Debug.Log("Joining");
                                    AddPlayer(go);
                                    go.GetComponent<Player>().joystickNumber = 3;
                                }
                            }
                        }
                        if (Input.GetKeyUp(KeyCode.Joystick3Button1) || Input.GetKeyUp(KeyCode.V) || Input.GetButtonUp("Fire2_3"))
                        {
                            if (go.GetComponent<PhotonView>().isMine/* == PhotonNetwork.player.ID*/)
                            {
                                if (wannaLeaveUI.GetActive() == true)
                                {
                                    wannaLeaveUI.SetActive(false);
                                }
                                else
                                {
                                    if (go.GetComponent<Player>().joined && !go.GetComponent<Player>().ready)
                                    {
                                        Debug.Log("Removing");
                                        RemovePlayer(go);
                                    }
                                    else if (go.GetComponent<Player>().ready)
                                    {
                                        photonView.RPC("PlayerUnReady", PhotonTargets.All, /*player.playerNumber */go.GetComponent<PhotonView>().viewID);
                                    }
                                    else
                                    {
                                        wannaLeaveUI.SetActive(true);
                                    }
                                }
                            }
                        }
                        break;

                    case 4:
                        if (Input.GetKeyUp(KeyCode.Joystick4Button0) || Input.GetKeyUp(KeyCode.Alpha4) || Input.GetButtonUp("Fire1_4"))
                        {
                            if (wannaLeaveUI.GetActive() == true)
                            {
                                PhotonNetwork.LeaveRoom(/*Launcher.Instance.LauncherSceneName*/);
                            }
                            else
                            {
                                if (go.GetComponent<Player>().joined && !go.GetComponent<Player>().ready)
                                {
                                    Debug.Log("Ready");
                                    photonView.RPC("PlayerReady", PhotonTargets.All, /*player.playerNumber */go.GetComponent<PhotonView>().viewID);
                                }
                                else
                                {
                                    Debug.Log("Joining");
                                    AddPlayer(go);
                                    go.GetComponent<Player>().joystickNumber = 4;
                                }
                            }
                        }
                        if (Input.GetKeyUp(KeyCode.Joystick4Button1) || Input.GetKeyUp(KeyCode.B) || Input.GetButtonUp("Fire2_4"))
                        {
                            if (go.GetComponent<PhotonView>().isMine/* == PhotonNetwork.player.ID*/)
                            {
                                if (wannaLeaveUI.GetActive() == true)
                                {
                                    wannaLeaveUI.SetActive(false);
                                }
                                else
                                {
                                    if (go.GetComponent<Player>().joined && !go.GetComponent<Player>().ready)
                                    {
                                        Debug.Log("Removing");
                                        RemovePlayer(go);
                                    }
                                    else if (go.GetComponent<Player>().ready)
                                    {
                                        photonView.RPC("PlayerUnReady", PhotonTargets.All, /*player.playerNumber */go.GetComponent<PhotonView>().viewID);
                                    }
                                    else
                                    {
                                        wannaLeaveUI.SetActive(true);
                                    }
                                }
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton2) || Input.GetKeyUp(KeyCode.X))
        {
            if (controls.GetComponent<Animator>().GetBool("ScrollShown"))
            {
                controls.GetComponent<Animator>().SetBool("ScrollShown", false);
            }
            else
            {
                controls.GetComponent<Animator>().SetBool("ScrollShown", true);
            }
        }
    }

    public void AddPlayer(GameObject playerObject)
    {
        Debug.Log("Adding player");
        Player player = playerObject.GetComponent<Player>();
        if (player.GetComponent<PhotonView>().isMine)
        {
            if (player.playerNumber == 0/* && PhotonNetwork.playerList.Length <= currentPlayers.Count*/)
            {
                int number = 1;
                bool foundOne = true;
                int amount = currentPlayers.Count;
                if (currentPlayers.Count > 1)
                {
                    while (foundOne)
                    {
                        foundOne = false;
                        foreach (GameObject p in currentPlayers)
                        {
                            Debug.Log(p.GetComponent<Player>().playerNumber);
                            if (player.GetComponent<PhotonView>().viewID != p.GetComponent<PhotonView>().viewID)
                            {
                                Debug.Log(p.GetComponent<Player>().playerNumber);
                                if (number == p.GetComponent<Player>().playerNumber)
                                {
                                    number++;
                                    Debug.Log("Number increased" + p.GetComponent<Player>().playerNumber);
                                    foundOne = true;
                                }
                            }
                        }
                    }
                }

                player.playerNumber = number;
                Debug.Log("PLayer number sat: " + number);
            }
            player.joined = true;
            player.shipPrefab = shipPrefabs[player.playerNumber - 1];
        }
    }

    public void RemovePlayer(GameObject playerObject)
    {
        Player player = playerObject.GetComponent<Player>();
        player.playerNumber = 0;
        player.joined = false;
        player.shipPrefab = null;
    }

    [PunRPC]
    public void PlayerReady(int playerID)
    {
        Debug.Log("Player" + playerID + " ready");

        Debug.Log(currentPlayers.Count);
        foreach (GameObject go in currentPlayers)
        {
            Debug.Log(go.GetComponent<PhotonView>().viewID);
            if (go.GetComponent<PhotonView>().viewID == playerID)
            {
                go.GetComponent<Player>().ready = true;
                decks[go.GetComponent<Player>().playerNumber - 1].GetComponent<Animator>().SetTrigger("Aktivate");
                Debug.Log(go + " is ready");
            }
        }

        bool allReady = true;
        foreach (GameObject go in currentPlayers)
        {
            if (go.GetComponent<Player>().ready != true)
            {
                allReady = false;
            }
        }

        if (allReady)
        {
            StartCoroutine(CountDown());
        }
    }

    public IEnumerator CountDown()
    {
        getReadyUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        bool allReady = true;
        foreach (GameObject go in currentPlayers)
        {
            if (go.GetComponent<Player>().ready != true)
            {
                allReady = false;
            }
        }
        if (allReady /*&& currentPlayers.Count > 1*/)
        {
            PhotonNetwork.LoadLevel(Launcher.Instance.InGameSceneName);
            //PhotonNetwork.room.IsOpen = false;
        }
        else if (allReady && Launcher.Instance.TestVersion/*&& currentPlayers.Count > 1*/)
        {
            PhotonNetwork.LoadLevel(Launcher.Instance.InGameSceneName);
            //PhotonNetwork.room.IsOpen = false;
        }
        else
        {
            getReadyUI.SetActive(false);
        }
    }

    [PunRPC]
    public void PlayerUnReady(int playerID)
    {
        foreach (GameObject go in currentPlayers)
        {
            Debug.Log(go.GetComponent<PhotonView>().viewID);
            if (go.GetComponent<PhotonView>().viewID == playerID)
            {
                go.GetComponent<Player>().ready = false;
                decks[go.GetComponent<Player>().playerNumber - 1].GetComponent<Animator>().SetTrigger("Deactivate");
                Debug.Log("Player" + playerID + " undready!");
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new NotImplementedException();
    }
}