using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public ParticleSystem[] winParticles;
    public Animator cannon;
    public string animationToPlay;

    public UnityEngine.UI.Text[] points;
    public UnityEngine.UI.Image[] sprites;
    public GameObject[] scoreCanvasi;
    public Ship[] ships;

    public bool pause = false;
    public bool winState = false;
    private float timer = 0f;

    public float gameTimer = 120f; //Normal timer is 120f
    public UnityEngine.UI.Text[] timers;

    //public PlayerSelection playerSelection;

    public Canvas pauseCanvas;

    private bool suddenDeath = false;
    public GameObject explosion;

    public musicPlayer musicPlayer;
    public AudioSource suddenDeathMusic;
    public AudioSource winMusic;
    public AudioSource fanfare;
    public AudioSource suddenDeathSpeak;

    public SpriteRenderer winnerRendition;

    public string winner;

    public GameObject gameCanvas;
    public GameObject winCanvas;

    public Animation blingAnimation;
    public SpriteRenderer skull;
    public TextMesh[] texts;

    private void Start()
    {
        pauseCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        if (ships != null)
        {
            foreach (Ship s in ships)
            {
                if (s == null)
                {
                }
            }
            if (!winState && !pause)
            {
                foreach (UnityEngine.UI.Text t in timers)
                {
                    if (!suddenDeath)
                    {
                        t.text = ((int)gameTimer).ToString();
                    }
                    else
                    {
                        t.color = Color.red;
                        t.fontSize = 10;
                        t.text = ("SUDDEN DEATH");
                    }
                    if (gameTimer < 10.01f)
                    {
                        t.color = Color.red;
                    }
                    else
                    {
                        t.color = Color.white;
                    }
                }

                if (/*playerSelection.isPlaying &&*/ !suddenDeath)
                {
                    gameTimer -= Time.deltaTime;

                    foreach (Ship s in ships)
                    {
                        if (s.kills >= 10)
                        {
                            winState = true;
                            timer = 0f;
                            gameTimer = 90f;
                            ShowScreen();
                        }
                    }

                    if (gameTimer < 0)
                    {
                        CheckForSuddenDeath();
                    }
                }
                else if (/*playerSelection.isPlaying && */suddenDeath)
                {
                    int alive = 0;
                    Ship lastStanding = null;
                    foreach (Ship s in ships)
                    {
                        if (s.alive)
                        {
                            lastStanding = s;
                            alive++;
                        }
                    }
                    if (alive <= 1)
                    {
                        lastStanding.kills += 2;
                        winState = true;
                        timer = 0f;
                        gameTimer = 90f;
                        ShowScreen();
                    }
                }
            }
            else if (winState)
            {
                if (timer > 1f)
                {
                    timer += Time.deltaTime;

                    foreach (Ship s in ships)
                    {
                        if (Input.GetKeyDown(s.controls.controls))
                        {
                            Debug.Log(s.controls.controls + " pressed");
                            //playerSelection.MenuScreen();
                            GetComponent<Camera>().depth = 0f;
                        }
                    }
                    if (timer > 31f)
                    {
                        //playerSelection.MenuScreen();
                        GetComponent<Camera>().depth = 0f;
                    }
                }
                else
                {
                    timer += Time.deltaTime;
                }
            }
        }
        if ((Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.X)) && !pause)
        {
            Debug.Log("Pause click");
            StartCoroutine(Pause());
        }
    }

    private void CheckForSuddenDeath()
    {
        //Is the leading ships tied?
        List<Ship> shipsByPoints = new List<Ship>();
        shipsByPoints = ships.OrderBy(go => go.kills).ToList();

        if (ships.Count() > 1)
        {
            if (shipsByPoints[ships.Count() - 2].kills == shipsByPoints[ships.Count() - 1].kills)
            {
                musicPlayer.Play(suddenDeathMusic);

                /*AudioSource a = GameObject.Find ("Adventure Meme").GetComponent<AudioSource> ();
                a.Stop();
                a.clip = suddenDeathMusic;
                suddenDeathSpeak.Play ();
                a.Play();*/

                suddenDeath = true;
                PowerUpSpawner p = GameObject.FindObjectOfType<PowerUpSpawner>();
                p.enabled = false;
                List<Ship> shipAlive = new List<Ship>();

                foreach (Ship ss in shipsByPoints)
                {
                    if (ss.kills >= shipsByPoints[ships.Count() - 1].kills)
                    {
                        ss.RemovePowerUps(true);
                        shipAlive.Add(ss);
                    }
                    else
                    {
                        //Debug.Log("Sudden death explosion");
                        Instantiate(explosion, ss.transform.position, Quaternion.identity);
                        ss.StartCoroutine(ss.Die());
                    }
                }

                GameObject[] crates = GameObject.FindGameObjectsWithTag("powerUp");
                foreach (GameObject crate in crates)
                {
                    //Debug.Log("Sudden death explosion");
                    Instantiate(explosion, crate.transform.position, Quaternion.identity);
                    Destroy(crate);
                }

                foreach (Ship s in ships)
                {
                    s.suddenDeath = true;
                }

                GameStatusManager.Instance.SuddenDeathInit(shipAlive.ToArray());
            }
            else
            {
                Debug.Log("No!");
                winState = true;
                timer = 0f;
                gameTimer = 90f;
                ShowScreen();

                //musicPlayer.PlayAfterDelay (fanfare, 0.0f);
            }
        }
        else
        {
            Debug.Log("No!");
            winState = true;
            timer = 0f;
            gameTimer = 90f;
            ShowScreen();

            //musicPlayer.PlayAfterDelay (fanfare, 0.0f);
        }
    }

    /// <summary>
    /// Sort PointList
    /// </summary>
    private void SortList()
    {
        List<Ship> shipsByPoints = new List<Ship>();
        shipsByPoints = ships.OrderBy(go => go.kills).ToList();
        winnerRendition.sprite = shipsByPoints[shipsByPoints.Count - 1].WinRendition;
        winner = shipsByPoints[shipsByPoints.Count - 1].name;

        int i;
        for (i = 0; i < shipsByPoints.Count; i++)
        {
            sprites[i].sprite = shipsByPoints[i].logo;

            points[i].text = shipsByPoints[i].kills.ToString();
        }

        //animationToPlay = shipsByPoints [3].flagAnimation;
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    private void ShowScreen()
    {
        EndGame(this, winner);
    }

    public void EndGame(WinScreen winScreen, string winner)
    {
        gameCanvas.SetActive(false);
        StartCoroutine(endSequence(winScreen, winner));

        foreach (Player p in GameStatusManager.Instance.players)
        {
            p.ship.alive = false;
            p.ship.transform.position = Vector3.one * 100f;

            if (p.animator == null)
            {
                if (p.logoMenu)
                {
                    p.logoMenu.GetComponent<SpriteRenderer>().color = Color.white; //TODO: Figure out what this logoMenu is
                }
            }
        }
    }

    private IEnumerator endSequence(WinScreen winScreen, string winner)
    {
        blingAnimation.gameObject.SetActive(true);
        blingAnimation.Play("Bling");

        //float points = 0;
        Sprite skullsSprite = skull.sprite;

        List<Ship> shipsByPoints = new List<Ship>();
        shipsByPoints = ships.OrderBy(go => go.kills).ToList();
        skull.sprite = shipsByPoints[shipsByPoints.Count - 1].logo;
        winner = shipsByPoints[shipsByPoints.Count - 1].name;

        //foreach (Player p in GameStatusManager.Instance.players)
        //{
        //    if (p.ship.kills > points || points == 0)
        //    {
        //        points = p.ship.kills;
        //        winner = p.ship.name;
        //        skull.sprite = p.ship.logo;
        //    }
        //}

        yield return new WaitForSeconds(0.30f);
        GameStatusManager.Instance.fuse.fuseLight.gameObject.SetActive(false);
        GameStatusManager.Instance.fuse.transform.parent.gameObject.SetActive(false);
        foreach (TextMesh t in texts)
        {
            t.text = winner + " Wins!";
        }

        yield return new WaitForSeconds(3f);

        //while (!Input.anyKey)
        //{
        //    yield return null;
        //}

        foreach (TextMesh t in texts)
        {
            t.text = "";
        }
        skull.sprite = skullsSprite;

        blingAnimation.gameObject.SetActive(false);

        int j = 0;
        for (int i = shipsByPoints.Count - 1; i >= 0; i--)
        {
            sprites[i].sprite = shipsByPoints[j].logo;
            points[i].text = shipsByPoints[j].kills.ToString();
            scoreCanvasi[i].SetActive(true);
            j++;
        }

        winCanvas.SetActive(true);
        Camera.main.depth = 0f;

        /*AudioSource a = GameObject.Find ("Adventure Meme").GetComponent<AudioSource> ();
        a.Stop ();
        a.clip = winScreen.winMusic;
        a.Play ();*/

        winScreen.ShowForReal();

        yield return null;

        GameStatusManager.Instance.fuse.fuseLight.gameObject.SetActive(true);
        GameStatusManager.Instance.fuse.transform.parent.gameObject.SetActive(true);

        bool goBack = false;

        while (!goBack)
        {
            foreach (Player p in GameStatusManager.Instance.players)
            {
                if (Input.GetKeyDown(p.ship.controls.controls) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    goBack = true;
                }
                else if (Input.GetKeyDown(KeyCode.JoystickButton1))
                {
                    PhotonNetwork.LeaveRoom();
                }
            }

            yield return null;
        }
        MenuScreen();
        winScreen.HideForReal();
        StartCoroutine(IntroFanfare());
    }

    public void MenuScreen()
    {
        //Application.LoadLevel (Application.loadedLevel);
        //startScreen = 2;
        winCanvas.SetActive(false);
        PhotonNetwork.LoadLevel(Launcher.Instance.WaitingRoomSceneName);

        Debug.Log("Should have loaded a new scene");
        //shipMenu.bannerIn.SetActive(true);
        //shipMenu.bannerOut.SetActive(false);
    }

    private IEnumerator IntroFanfare()
    {
        Debug.Log("playerstarting music");

        //musicPlayer.Play(IntroSplash);
        //musicPlayer.GotoTime(IntroSplash, 2f);
        yield return new WaitForSeconds(17.5f);
        //if (musicPlayer.currentSource == IntroSplash)
        //{
        //    musicPlayer.PlayWithFadeIn(menuMusic, 1f);
        //    musicPlayer.GotoTime(menuMusic, 72f);
        //}
    }

    public void ShowForReal()
    {
        musicPlayer.PlayWithFadeIn(winMusic, 1.0f);

        SortList();
        GetComponent<Camera>().depth = 2f;
        foreach (Ship s in ships)
        {
            s.alive = false;
        }

        foreach (ParticleSystem p in winParticles)
        {
            p.Play();
        }

        cannon.Play(animationToPlay);
    }

    public void HideForReal()
    {
        //GetComponent<Camera>().depth = 0f;
        //Application.LoadLevel(Application.loadedLevel);
    }

    public IEnumerator Pause()
    {
        pause = true;
        pauseCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        float timescaleWas = Time.timeScale;
        //Time.timeScale = 0.0f; // RDG Commented out for online
        while (pause)
        {
            //Time.timeScale = 0.0f; // RDG Commented out for online
            bool exitGame = true;
            foreach (Ship s in ships)
            {
                if (Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.X)/*Input.GetKeyDown(s.controls.controls)*/)
                {
                    pause = false;
                }
                if ((!Input.GetKey(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.Z))/*!Input.GetKeyDown(s.controls.exit)*/ && s.kills > -1)
                {
                    exitGame = false;
                }
            }
            if (exitGame)
            {
                Time.timeScale = 1f;
                PhotonNetwork.LeaveRoom();
                //Application.LoadLevel(Application.loadedLevel);
            }
            yield return null;
        }

        pauseCanvas.gameObject.SetActive(false);

        //Time.timeScale = timescaleWas; // RDG Commented out for online
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting /*&& PhotonNetwork.isMasterClient*/)
        {
            stream.SendNext(gameTimer);
        }
        else
        {
            gameTimer = (float)stream.ReceiveNext();
        }
    }
}