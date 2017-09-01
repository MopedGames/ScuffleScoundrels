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
                        p.animator.Play("Show");
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
                            p.animator.Play("ActivatePlayer");
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
                        p.animator.Play("DeactivatePlayer");
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

        switch (currentPlayers.Count)
        {
            case 1:
                decks[0].SetActive(true);
                decks[1].SetActive(false);
                decks[2].SetActive(false);
                decks[3].SetActive(false);
                break;

            case 2:
                decks[0].SetActive(true);
                decks[1].SetActive(true);
                decks[2].SetActive(false);
                decks[3].SetActive(false);
                break;

            case 3:
                decks[0].SetActive(true);
                decks[1].SetActive(true);
                decks[2].SetActive(true);
                decks[3].SetActive(false);
                break;

            case 4:
                decks[0].SetActive(true);
                decks[1].SetActive(true);
                decks[2].SetActive(true);
                decks[3].SetActive(true);
                break;
        }

        ReadyUp();

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

    private void ReadyUp() // players say they are ready
    {
        if (Input.GetKeyUp(KeyCode.JoystickButton0))
        {
            photonView.RPC("PlayerReady", PhotonTargets.AllBuffered, PhotonNetwork.player.ID);
            DealCards();

            foreach (GameObject go in currentPlayers)
            {
                if (go.GetComponent<PhotonView>().ownerId == PhotonNetwork.player.ID)
                {
                    go.GetComponent<Player>().shipPrefab = shipPrefabs[PhotonNetwork.player.ID - 1];
                }
            }
        }
    }

    [PunRPC]
    public void PlayerReady(int playerID)
    {
        Debug.Log("Player" + playerID + " ready");
        foreach (GameObject go in currentPlayers)
        {
            if (go.GetComponent<PhotonView>().ownerId == playerID)
            {
                go.GetComponent<Player>().ready = true;
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
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new NotImplementedException();
    }
}