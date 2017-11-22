using UnityEngine;
using System.Collections;

public class PlayerSelection : MonoBehaviour
{
    public Player[] players;

    public int startScreen = 3;
    private float countdown = 3.0f;
    private float exitCountDown = 3.9f;
    public UnityEngine.UI.Text[] countDownText;
    public UnityEngine.UI.Text exitCountDownText;
    public Renderer countDownBorder;

    public GameObject menuCanvas;
    public GameObject gameCanvas;
    public GameObject winCanvas;
    public GameObject exitCanvas;

    public bool isPlaying = false;

    public PowerUpSpawner powerUps;
    public Canvas[] scoreCanvas;

    public musicPlayer musicPlayer;
    public AudioSource IntroSplash;
    public AudioSource menuMusic;
    public AudioSource gameMusic;

    public Camera shipCam;
    public bool scrollVisible;
    public Animator controlScroll;
    public Animator creditsScroll;

    public Transform exitBar;

    public fuse fuse;

    private bool keyReleased = true;

    public Animation blingAnimation;
    public SpriteRenderer skull;
    public TextMesh[] texts;

    public int playerNumber = 0;
    public int playerReady = 0;

    //private IEnumerator EnterControls()
    //{
    //    scrollVisible = true;
    //    if (controlScroll != null)
    //    {
    //        controlScroll.Play("scrollIn");
    //    }

    //    bool stop = true;
    //    yield return null;
    //    while (stop)
    //    {
    //        foreach (Player p in players)
    //        {
    //            if (Input.GetKeyDown(p.ship.controls.controls) || Input.GetKeyDown(KeyCode.Return))
    //            {
    //                stop = false;
    //            }
    //        }
    //        yield return null;
    //    }
    //    scrollVisible = false;
    //    if (controlScroll != null)
    //    {
    //        controlScroll.Play("scrollOut");
    //    }
    //}

    

    private void Start()
    {
        UnityEngine.Cursor.visible = false;

        menuCanvas.active = false;
        gameCanvas.active = false;
        winCanvas.active = false;
        exitCanvas.active = false;

        StartCoroutine(IntroFanfare());
    }

    private IEnumerator IntroFanfare()
    {
        Debug.Log("playerstarting music");

        musicPlayer.Play(IntroSplash);
        musicPlayer.GotoTime(IntroSplash, 2f);
        yield return new WaitForSeconds(17.5f);
        if (musicPlayer.currentSource == IntroSplash)
        {
            musicPlayer.PlayWithFadeIn(menuMusic, 1f);
            musicPlayer.GotoTime(menuMusic, 72f);
        }
    }

    // Update is called once per frame - beastmode
    private void Update()
    {
        ////screen 3 - premenu - premenyu might mean main menu.. not sure...
        ////if (startScreen == 3)
        ////{
        ////    foreach (Player p in currentPlayers)
        ////    {
        ////        p.ship.alive = false;
        ////        p.ship.transform.position = Vector3.one * 100f; // this position is so weird
        ////        if (p.animator == null)
        ////        {
        ////            p.logoMenu.GetComponent<SpriteRenderer>().color = Color.white;
        ////        }
        ////    }

        ////    //shipCam.depth = 2;
        ////    if (Input.anyKeyDown)
        ////    {
        ////        shipMenu.gameObject.SetActive(true);
        ////        startScreen = 2;
        ////    }
        ////}

        ////Main Menu funcions
        ////if (shipMenu.currentSelection == 2 && keyPressed)
        ////{
        ////    StartCoroutine(EnterCredits());
        ////    keyPressed = false;
        ////}
        ////else if (shipMenu.currentSelection == 1 && keyPressed)
        ////{
        ////    StartCoroutine(shipMenu.ChangeSceneAnimation(this));
        ////    keyPressed = false;
        ////}
        ////else if (shipMenu.currentSelection == 0 && keyPressed)
        ////{
        ////    Application.Quit();
        ////    keyPressed = false;

        //// Character Select

        //if (startScreen == 1) //
        //{
        //    //shipCam.depth = 0; // tror det her depth hejs blev brugt til at vise de forskellige dele af scenen
        //    //bool exit = false; // so much BS just to exit
        //    //foreach (Player p in players)
        //    //{
        //    //    if (Input.GetKey(p.ship.controls.exit) || Input.GetKeyDown(KeyCode.Escape))
        //    //    {
        //    //        exit = true;
        //    //    }
        //    //}
        //    //if (exit == true || Input.GetKey(KeyCode.Escape))
        //    //{
        //    //    startScreen = 3;

        //    //    //shipMenu.startBanner();
        //    //    foreach (Player p in players)

        //    //    {
        //    //        p.playing = false;
        //    //        playerNumber = 0;
        //    //        playerReady = 0;
        //    //        p.animator.Play("Hidden");
        //    //        p.ship.kills = -1;
        //    //    }

        //        /*exitBar.gameObject.SetActive (true);
        //        foreach (Player p in players) {
        //            p.logoMenu.GetComponent<SpriteRenderer> ().enabled = false;
        //        }

        //        exitCountDown -= Time.deltaTime;
        //        exitCanvas.SetActive (true);
        //        exitCountDownText.text = ((int)exitCountDown).ToString ();*/
        //    } /*else if (Input.GetKeyUp (KeyCode.Escape)) {
        //	exitCountDown = 3.9f;
        //	exitCanvas.SetActive (false);

        //	exitBar.gameObject.SetActive (false);
        //	foreach (Player p in players) {
        //		p.logoMenu.GetComponent<SpriteRenderer> ().enabled = true;
        //	}
        //}

        //if (exitCountDown <= 0f) {
        //	Application.Quit ();
        //}*/

        //    menuCanvas.active = true;
        //    gameCanvas.active = false;

        //    // this seems to be used at runtime
        //    foreach (Player p in players)
        //    {
        //        //if (Input.GetKeyDown(p.ship.controls.controls) || Input.GetKeyDown(KeyCode.Return))
        //        //{
        //        //    StartCoroutine(EnterControls());
        //        //}

        //        if (p.animator.GetCurrentAnimatorStateInfo(0).IsName("Hidden"))
        //        {
        //            p.ship.kills = -1;

        //            if (Input.GetKey(p.ship.controls.shootLeft) || Input.GetKey(p.ship.controls.shootLeftAlt) || Input.GetKey(p.ship.controls.shootLeftKeyboard) ||
        //               Input.GetKey(p.ship.controls.shootRight) || Input.GetKey(p.ship.controls.shootRightAlt) || Input.GetKey(p.ship.controls.shootRightKeyboard))
        //            {
        //                p.animator.Play("Show");
        //                p.landparticle.Play();
        //                p.playsound.Play(1);
        //                ++playerNumber;
        //                //++playerNumber; - lulwut? look above
        //            }
        //        }
        //        else if (p.animator.GetCurrentAnimatorStateInfo(0).IsName("Shown"))
        //        {
        //            if (!p.playing)
        //            {
        //                if (Input.GetKey(p.ship.controls.shootLeft) || Input.GetKey(p.ship.controls.shootLeftAlt) || Input.GetKey(p.ship.controls.shootLeftKeyboard) ||
        //                   Input.GetKey(p.ship.controls.shootRight) || Input.GetKey(p.ship.controls.shootRightAlt) || Input.GetKey(p.ship.controls.shootRightKeyboard))
        //                {
        //                    p.animator.Play("ActivatePlayer");
        //                    p.playsound.Play(0);
        //                    p.playing = true;

        //                    ++playerReady;
        //                }
        //            }
        //        }
        //        else if (p.animator.GetCurrentAnimatorStateInfo(0).IsName("Active"))
        //        {
        //            if (Input.GetKey(p.ship.controls.exit))
        //            {
        //                p.animator.Play("DeactivatePlayer");
        //                p.playsound.Play(0);
        //                p.playing = false;
        //                p.ship.kills = -1; // what ? how can the player loose kills at this point?=
        //                --playerReady;
        //            }
        //        }

        //        /* {
        //            p.playing = false;

        //            if (p.animator == null) {
        //                p.logoMenu.localScale = Vector3.one * 0.75f;
        //                p.logoMenu.GetComponent<SpriteRenderer> ().color = new Color (0.3f, 0.3f, 0.3f, 1f);
        //            } else if(p.animator.GetCurrentAnimatorStateInfo(0).IsName("Active")) {
        //                p.animator.Play ("DeactivatePlayer");
        //            }

        //            p.ship.kills = -1;
        //        }*/
        //    }

        //    // not sure wtf this bit is...
        //    int i;
        //    for (i = 0; i < scoreCanvas.Length; i++)
        //    {
        //        if (i < playerNumber)
        //        {
        //            scoreCanvas[i].enabled = true;
        //        }
        //        else {
        //            scoreCanvas[i].enabled = false;
        //        }
        //    }

        //    // when there are more than one player that is set to ready
        //    if (playerNumber >= 2 && playerNumber == playerReady)
        //    {
        //        countdown -= Time.deltaTime;
        //        if (countDownBorder != null)
        //        {
        //            countDownBorder.material.SetFloat("_CutOff", countdown / 3.0f);
        //        }
        //        else {
        //            foreach (UnityEngine.UI.Text t in countDownText)
        //            {
        //                t.enabled = true;
        //                t.text = ((int)countdown).ToString();
        //            }
        //        }
        //        if (countdown <= 0f)
        //        {
        //            startScreen = 0;
        //            foreach (Player p in players)
        //            {
        //                if (p.playing)
        //                {
        //                    p.ship.kills = 0;
        //                }
        //            }
        //        }
        //    }
        //    // or if the pllayer is still on his own
        //    else {
        //        countdown = Mathf.Lerp(countdown, 3.0f, 0.3f);
        //        if (countDownBorder != null)
        //        {
        //            countDownBorder.material.SetFloat("_CutOff", countdown / 3.0f);
        //        }
        //        else {
        //            foreach (UnityEngine.UI.Text t in countDownText)
        //            {
        //                t.enabled = false;
        //            }
        //        }
        //    }
        //}
        ////}
        //else if (!isPlaying && startScreen == 0)
        //{
        //    isPlaying = true;
        //    StartGame();
        //}
    }

    //public void EndGame(WinScreen winScreen, string winner)
    //{
    //    gameCanvas.active = false;
    //    StartCoroutine(endSequence(winScreen, winner));

    //    foreach (Player p in players)
    //    {
    //        p.ship.alive = false;
    //        p.ship.transform.position = Vector3.one * 100f;

    //        if (p.animator == null)
    //        {
    //            p.logoMenu.GetComponent<SpriteRenderer>().color = Color.white;
    //        }
    //    }
    //}

    public void SuddenDeathInit(Ship[] shipsAlive)
    {
        foreach (Ship s in shipsAlive)
        {
            s.invulnerable = true;
        }
        StartCoroutine(suddenDeath(shipsAlive));
    }

    private IEnumerator suddenDeath(Ship[] shipsAlive)
    {
        blingAnimation.gameObject.SetActive(true);
        blingAnimation.Play("Bling");
        yield return new WaitForSeconds(0.30f);
        fuse.fuseLight.gameObject.SetActive(false);
        fuse.transform.parent.gameObject.SetActive(false);
        foreach (TextMesh t in texts)
        {
            t.text = "SUDDEN DEATH";
        }

        yield return new WaitForSeconds(1.7f);

        foreach (TextMesh t in texts)
        {
            t.text = "";
        }

        foreach (Ship s in shipsAlive)
        {
            s.invulnerable = false;
            s.transform.position = s.startPos;
            s.transform.rotation = s.startRot;
        }

        blingAnimation.gameObject.SetActive(false);
    }

    //private IEnumerator endSequence(WinScreen winScreen, string winner)
    //{
    //    blingAnimation.gameObject.SetActive(true);
    //    blingAnimation.Play("Bling");

    //    float points = 0;
    //    Sprite skullsSprite = skull.sprite;
    //    foreach (Player p in players)
    //    {
    //        if (p.ship.kills > points)
    //        {
    //            points = p.ship.kills;
    //            winner = p.ship.name;
    //            skull.sprite = p.ship.logo;
    //        }
    //    }

    //    yield return new WaitForSeconds(0.30f);
    //    fuse.fuseLight.gameObject.SetActive(false);
    //    fuse.transform.parent.gameObject.SetActive(false);
    //    foreach (TextMesh t in texts)
    //    {
    //        t.text = winner + " Wins!";
    //    }

    //    yield return new WaitForSeconds(1.7f);

    //    while (!Input.anyKey)
    //    {
    //        yield return null;
    //    }

    //    foreach (TextMesh t in texts)
    //    {
    //        t.text = "";
    //    }
    //    skull.sprite = skullsSprite;

    //    blingAnimation.gameObject.SetActive(false);

    //    winCanvas.active = true;
    //    Camera.main.depth = 0f;

    //    /*AudioSource a = GameObject.Find ("Adventure Meme").GetComponent<AudioSource> ();
    //    a.Stop ();
    //    a.clip = winScreen.winMusic;
    //    a.Play ();*/

    //    winScreen.ShowForReal();

    //    yield return null;

    //    fuse.fuseLight.gameObject.SetActive(true);
    //    fuse.transform.parent.gameObject.SetActive(true);

    //    bool goBack = false;

    //    while (!goBack)
    //    {
    //        foreach (Player p in players)
    //        {
    //            if (Input.GetKeyDown(p.ship.controls.controls) || Input.GetKeyDown(KeyCode.Return))
    //            {
    //                goBack = true;
    //            }
    //        }

    //        yield return null;
    //    }
    //    MenuScreen();
    //    winScreen.HideForReal();
    //    StartCoroutine(IntroFanfare());
    //}

    public void MenuScreen()
    {
        //Application.LoadLevel (Application.loadedLevel);
        startScreen = 2;
        winCanvas.active = false;
        //shipMenu.bannerIn.SetActive(true);
        //shipMenu.bannerOut.SetActive(false);
    }

    //private void StartGame()
    //{
    //    /*AudioSource a = GameObject.Find ("Adventure Meme").GetComponent<AudioSource> ();
    //    a.Play();*/
    //    if (musicPlayer.currentSource != gameMusic)
    //    {
    //        musicPlayer.PlayWithFadeIn(gameMusic, 0.1f);
    //    }

    //    fuse.Play();

    //    //PlayAudioAtInterval playA = FindObjectOfType<PlayAudioAtInterval> ();
    //    //playA.enabled = false;
    //    //audio.Stop();

    //    menuCanvas.active = false;
    //    gameCanvas.active = true;
    //    Camera.main.depth = 2f;
    //    Camera.main.GetComponent<Animation>().Play();
    //    powerUps.enabled = true;

    //    foreach (Player p in players)
    //    {
    //        p.scoreBoard.active = p.playing;
    //        if (p.playing)
    //        {
    //            p.ship.Spawn();
    //        }
    //    }
    //}
}