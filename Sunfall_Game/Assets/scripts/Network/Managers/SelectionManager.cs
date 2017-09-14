using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : PunBehaviourManager<SelectionManager>
{
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

    public GameObject controls;

    public musicPlayer musicPlayer;
    public AudioSource selectionMusic;

    public void Start()
    {
        //Debug.Log("Selection manager start " + currentPlayers.Count);
        ConnectionHandler.Instance.photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered);
        musicPlayer.Play(selectionMusic);
    }

    public void PlayersReady()
    {
        // when there are more than one player that is set to ready
        if (playerNumber >= 2 && playerNumber == playerReady)
        {
            countdown -= Time.deltaTime;
            if (countDownBorder != null)
            {
                countDownBorder.material.SetFloat("_CutOff", countdown / 3.0f);
            }
            else
            {
                foreach (UnityEngine.UI.Text t in countDownText)
                {
                    t.enabled = true;
                    t.text = ((int)countdown).ToString();
                }
            }
            if (countdown <= 0f)
            {
                startScreen = 0;  // startscreen 0 means in game?
                foreach (GameObject go in currentPlayers)
                {
                    Player p = go.GetComponent<Player>();
                    if (p.playing)
                    {
                        p.ship.kills = 0;
                    }
                }
            }
        }
        // or if the pllayer is still on his own
        else
        {
            countdown = Mathf.Lerp(countdown, 3.0f, 0.3f);
            if (countDownBorder != null)
            {
                countDownBorder.material.SetFloat("_CutOff", countdown / 3.0f);
            }
            else
            {
                foreach (UnityEngine.UI.Text t in countDownText)
                {
                    t.enabled = false;
                }
            }
        }
    }

    public void DealCards()
    {
        foreach (GameObject go in currentPlayers)
        {
            Player p = go.GetComponent<Player>();
            //if (Input.GetKeyDown(p.ship.controls.controls) || Input.GetKeyDown(KeyCode.Return))
            //{
            //    StartCoroutine(EnterControls());
            //}

            if (p.animator.GetCurrentAnimatorStateInfo(0).IsName("Hidden")) // hidden as in the players deck is there but the character hasnt been selected
            {
                p.ship.kills = -1; // is this to reset the kills? - that wont be necessary with a proper scene setup
                if (Input.GetKeyDown(KeyCode.A))
                {
                    Debug.Log("hej");
                    // this seems to add a player to the scene when the player pushes their fire button... this is unnessecary as each deck should be added when the number of players is increased
                    //if (Input.GetKey(p.ship.controls.shootLeft) || Input.GetKey(p.ship.controls.shootLeftAlt) || Input.GetKey(p.ship.controls.shootLeftKeyboard) ||
                    //   Input.GetKey(p.ship.controls.shootRight) || Input.GetKey(p.ship.controls.shootRightAlt) || Input.GetKey(p.ship.controls.shootRightKeyboard))
                    {
                        //p.animator.Play("Show");
                        p.landparticle.Play();
                        p.playsound.Play(1);
                        ++playerNumber;
                        //++playerNumber; - lulwut? look above
                    }
                }
                else if (p.animator.GetCurrentAnimatorStateInfo(0).IsName("Shown")) // the card shows the character the player will play
                {
                    if (!p.playing)
                    {
                        //    if (Input.GetKey(p.ship.controls.shootLeft) || Input.GetKey(p.ship.controls.shootLeftAlt) || Input.GetKey(p.ship.controls.shootLeftKeyboard) ||
                        //       Input.GetKey(p.ship.controls.shootRight) || Input.GetKey(p.ship.controls.shootRightAlt) || Input.GetKey(p.ship.controls.shootRightKeyboard))
                        {
                            //p.animator.Play("ActivatePlayer");
                            p.playsound.Play(0);
                            p.playing = true;

                            ++playerReady;
                        }
                    }
                }
                else if (p.animator.GetCurrentAnimatorStateInfo(0).IsName("Active"))
                {
                    if (Input.GetKey(p.ship.controls.exit))
                    {
                        //p.animator.Play("DeactivatePlayer");
                        p.playsound.Play(0);
                        p.playing = false;
                        p.ship.kills = -1; // what ? how can the player loose kills at this point?=
                        --playerReady;
                    }
                }
            }
        }
    }

    public void Update()
    {
        //DealCards();
        //PlayersReady();

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


        //switch (currentPlayers.Count)
        //{
        //    case 1:
        //        if (decks[0].activeSelf != true)
        //        {
        //            decks[0].SetActive(true);
        //            decks[0].GetComponent<Animator>().SetTrigger("Show");
        //            foreach (GameObject go in currentPlayers)
        //            {
        //                if (go.GetComponent<Player>().playerNumber == 1)
        //                {
        //                    if (go.GetComponent<Player>().ready == true)
        //                    {
        //                        decks[0].GetComponent<Animator>().SetTrigger("Aktivate");
        //                    }
        //                }
        //            }

        //            //decks[PhotonNetwork.player.ID - 1].landparticle.Play();
        //            //decks[PhotonNetwork.player.ID - 1].GetComponent<PlaySound>().Play(1);
        //        }
        //        decks[1].SetActive(false);
        //        decks[2].SetActive(false);
        //        decks[3].SetActive(false);
        //        break;

        //    case 2:
        //        if (decks[0].activeSelf != true)
        //        {
        //            decks[0].SetActive(true);
        //            decks[0].GetComponent<Animator>().SetTrigger("Show");
        //            foreach (GameObject go in currentPlayers)
        //            {
        //                if (go.GetComponent<Player>().playerNumber == 1)
        //                {
        //                    if (go.GetComponent<Player>().ready == true)
        //                    {
        //                        decks[0].GetComponent<Animator>().SetTrigger("Aktivate");
        //                    }
        //                }
        //            }
        //            //decks[PhotonNetwork.player.ID - 1].landparticle.Play();
        //            //decks[PhotonNetwork.player.ID - 1].GetComponent<PlaySound>().Play(1);
        //        }
        //        if (decks[1].activeSelf != true)
        //        {
        //            decks[1].SetActive(true);
        //            decks[1].GetComponent<Animator>().SetTrigger("Show");
        //            //decks[PhotonNetwork.player.ID - 1].landparticle.Play();
        //            //decks[PhotonNetwork.player.ID - 1].GetComponent<PlaySound>().Play(1);
        //        }
        //        decks[2].SetActive(false);
        //        decks[3].SetActive(false);
        //        break;

        //    case 3:
        //        if (decks[0].activeSelf != true)
        //        {
        //            decks[0].SetActive(true);
        //            decks[0].GetComponent<Animator>().SetTrigger("Show");
        //            foreach (GameObject go in currentPlayers)
        //            {
        //                if (go.GetComponent<Player>().playerNumber == 1)
        //                {
        //                    if (go.GetComponent<Player>().ready == true)
        //                    {
        //                        decks[0].GetComponent<Animator>().SetTrigger("Aktivate");
        //                    }
        //                }
        //            }
        //            //decks[PhotonNetwork.player.ID - 1].landparticle.Play();
        //            //decks[PhotonNetwork.player.ID - 1].GetComponent<PlaySound>().Play(1);
        //        }
        //        if (decks[1].activeSelf != true)
        //        {
        //            decks[1].SetActive(true);
        //            decks[1].GetComponent<Animator>().SetTrigger("Show");
        //            //decks[PhotonNetwork.player.ID - 1].landparticle.Play();
        //            //decks[PhotonNetwork.player.ID - 1].GetComponent<PlaySound>().Play(1);
        //        }
        //        if (decks[2].activeSelf != true)
        //        {
        //            decks[2].SetActive(true);
        //            decks[2].GetComponent<Animator>().SetTrigger("Show");
        //            //decks[PhotonNetwork.player.ID - 1].landparticle.Play();
        //            //decks[PhotonNetwork.player.ID - 1].GetComponent<PlaySound>().Play(1);
        //        }
        //        decks[3].SetActive(false);
        //        break;

        //    case 4:
        //        if (decks[0].activeSelf != true)
        //        {
        //            decks[0].SetActive(true);
        //            decks[0].GetComponent<Animator>().SetTrigger("Show");
        //            foreach (GameObject go in currentPlayers)
        //            {
        //                if (go.GetComponent<Player>().playerNumber == 1)
        //                {
        //                    if (go.GetComponent<Player>().ready == true)
        //                    {
        //                        decks[0].GetComponent<Animator>().SetTrigger("Aktivate");
        //                    }
        //                }
        //            }
        //            //decks[PhotonNetwork.player.ID - 1].landparticle.Play();
        //            //decks[PhotonNetwork.player.ID - 1].GetComponent<PlaySound>().Play(1);
        //        }
        //        if (decks[1].activeSelf != true)
        //        {
        //            decks[1].SetActive(true);
        //            decks[1].GetComponent<Animator>().SetTrigger("Show");
        //            //decks[PhotonNetwork.player.ID - 1].landparticle.Play();
        //            //decks[PhotonNetwork.player.ID - 1].GetComponent<PlaySound>().Play(1);
        //        }
        //        if (decks[2].activeSelf != true)
        //        {
        //            decks[2].SetActive(true);
        //            decks[2].GetComponent<Animator>().SetTrigger("Show");
        //            //decks[PhotonNetwork.player.ID - 1].landparticle.Play();
        //            //decks[PhotonNetwork.player.ID - 1].GetComponent<PlaySound>().Play(1);
        //        }
        //        if (decks[3].activeSelf != true)
        //        {
        //            decks[3].SetActive(true);
        //            decks[3].GetComponent<Animator>().SetTrigger("Show");
        //            //decks[PhotonNetwork.player.ID - 1].landparticle.Play();
        //            //decks[PhotonNetwork.player.ID - 1].GetComponent<PlaySound>().Play(1);
        //        }
        //        break;
        //}

        Inputs();

        //if (!isPlaying && startScreen == 0)
        //{
        //    isPlaying = true;
        //StartGame(); // run game omg

        /* {
            p.playing = false;

            if (p.animator == null) {
                p.logoMenu.localScale = Vector3.one * 0.75f;
                p.logoMenu.GetComponent<SpriteRenderer> ().color = new Color (0.3f, 0.3f, 0.3f, 1f);
            } else if(p.animator.GetCurrentAnimatorStateInfo(0).IsName("Active")) {
                p.animator.Play ("DeactivatePlayer");
            }

            p.ship.kills = -1;
        }*/
        //}

        //// not sure wtf this bit is...
        //int i;

        //for (i = 0; i<scoreCanvas.Length; i++)
        //{
        //    if (i<playerNumber)
        //    {
        //        scoreCanvas[i].enabled = true;
        //    }
        //    else {
        //        scoreCanvas[i].enabled = false;
        //    }
        //}

        // or if the pllayer is still on his own
        //else
        //{
        //    countdown = Mathf.Lerp(countdown, 3.0f, 0.3f);
        //    if (countDownBorder != null)
        //    {
        //        countDownBorder.material.SetFloat("_CutOff", countdown / 3.0f);
        //    }
        //    else
        //    {
        //        foreach (UnityEngine.UI.Text t in countDownText)
        //        {
        //            t.enabled = false;
        //        }
        //    }
        //}

        //}
        //else if (!isPlaying && startScreen == 0)
        //{
        //    isPlaying = true;
        //    //StartGame();  // start the game
        //}
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

    private void Inputs() // players say they are ready
    {
        if (Input.GetKeyUp(KeyCode.JoystickButton0))
        {
            Debug.Log("Click");
            //DealCards();

            foreach (GameObject go in currentPlayers)
            {
                if (go.GetComponent<PhotonView>().isMine/* == PhotonNetwork.player.ID*/)
                {
                    Debug.Log("Has photonView");
                    if (go.GetComponent<Player>().joined && !go.GetComponent<Player>().ready)
                    {
                        Debug.Log("Ready");
                        photonView.RPC("PlayerReady", PhotonTargets.All, /*player.playerNumber */PhotonNetwork.player.ID);
                    }
                    else
                    {
                        Debug.Log("Joining");
                        AddPlayer(go);
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.JoystickButton1))
        {
            foreach (GameObject go in currentPlayers)
            {
                if (go.GetComponent<PhotonView>().isMine/* == PhotonNetwork.player.ID*/)
                {
                    if (go.GetComponent<Player>().joined && !go.GetComponent<Player>().ready)
                    {
                        Debug.Log("Removing");
                        RemovePlayer(go);
                    }
                    else if (go.GetComponent<Player>().ready)
                    {
                        photonView.RPC("PlayerUnReady", PhotonTargets.All, /*player.playerNumber */PhotonNetwork.player.ID);
                    }
                    else
                    {
                        PhotonNetwork.LeaveRoom(/*Launcher.Instance.LauncherSceneName*/);
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.JoystickButton2))
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
                            if (p.GetComponent<PhotonView>().isMine != true)
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
            Debug.Log(go.GetComponent<PhotonView>().ownerId);
            if (go.GetComponent<PhotonView>().ownerId == playerID)
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
            Debug.Log(go.GetComponent<PhotonView>().ownerId);
            if (go.GetComponent<PhotonView>().ownerId == playerID)
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