using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class shipMenu : MonoBehaviour
{
    // which menu item is selected
    public int currentSelection = 1;
    private int currentLevel = 0;

    // array if menu items
    public SpriteRenderer[] menuItems;

    // array of play menu items
    public SpriteRenderer[] playModes;

    // array of numberofplayers
    public SpriteRenderer[] numberOfPlayers;

    private Vector3 targetPos;

    //public Vector3 unselectedPos;
    public Color unselectedColor;

    public AudioSource audioBlob;
    public AudioSource audioPling;

    public AnimationCurve bouncy;
    public Animation panAnimation;

    public GameObject bannerIn;
    public GameObject bannerOut;

    private bool activeMenu = true;

    private bool[] buttonactive = new bool[4];

    public float time;

    public Ship[] ships;

    public musicPlayer musicPlayer;
    public AudioSource menuMusic;

    public Animator creditsScroll;
    public bool scrollShown;

    public Text[] playerTexts;
    public GameObject playerPrefab;


    // Use this for initialization
    private void Awake()
    {
        ChangeSelection(0);
        targetPos = menuItems[0].transform.parent.localPosition;

        //launcher = Launcher.Instance;
    }

    private void MenuColors()
    {
        if (currentLevel == 0)
        {
            foreach (SpriteRenderer r in menuItems)
            {
                if (menuItems[currentSelection] == r)
                {
                    r.transform.localScale = Vector3.one;
                    r.color = Color.white;
                }
                else
                {
                    r.transform.localScale = Vector3.one * 0.5f;
                    r.color = unselectedColor;
                }
            }
        }
        else if (currentLevel == 1)
        {
            foreach (SpriteRenderer r in playModes)
            {
                if (playModes[currentSelection] == r)
                {
                    r.transform.localScale = Vector3.one;
                    r.color = Color.white;
                }
                else
                {
                    r.transform.localScale = Vector3.one * 0.5f;
                    r.color = unselectedColor;
                }
            }
        }
    }

    private void Start()
    {
        musicPlayer.Play(menuMusic);
    }

    public IEnumerator Shakecam()
    {
        yield return new WaitForSeconds(1.9f);
        float shake = 0f;
        //Vector3 startpos = transform.parent.position;
        while (shake < 1f)
        {
            transform.parent.localPosition = Random.insideUnitSphere * 0.1f;
            shake += Time.deltaTime * 3f;

            yield return null;
        }


    }

    private void ChangeSelection(int selectionMod)
    {
        if (scrollShown != true)
        {
            time = 0f;
            currentSelection += selectionMod;

                audioBlob.pitch = Random.Range(0.5f, 0.8f);
                audioBlob.Play();
                targetPos = new Vector3((currentSelection - 1) * -2, menuItems[0].transform.parent.localPosition.y, menuItems[0].transform.parent.localPosition.z);

                MenuColors();
        }
    }

    private IEnumerator ChangeLevel(int selectionMod){
        currentLevel = selectionMod;
        currentSelection = 1;
        float i = 0f;
        MenuColors();
        targetPos = new Vector3((currentSelection - 1) * -2, selectionMod, menuItems[0].transform.parent.localPosition.z);

        while (i < 1f)
        {
            i += Time.deltaTime * 5f;
            

            float a = selectionMod + (0.5f - selectionMod) * i * 2f;

            //fadeMainmenu
            foreach (SpriteRenderer sprite in menuItems)
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, a);
            }

            //fadePlaymenu
            foreach (SpriteRenderer sprite in playModes)
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f - a);
            }

            yield return null;
        }
        

    }

    public void startBanner()
    {
        bannerIn.SetActive(true);
        bannerOut.SetActive(false);
    }

    /// <summary>
    /// move to the next part of the game animation
    /// </summary>
    /// <param name="playerselection"></param>
    /// <returns></returns>

    public IEnumerator ChangeSceneAnimation(bool offline)
    {
        currentLevel = 2;
        Launcher.Instance.OfflineMode(offline);
        bannerIn.SetActive(false);
        bannerOut.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        if (offline)
        {

            panAnimation.Play("pan_panning");

            yield return new WaitForSeconds(1.5f);

            //while players connect!

            Debug.Log("hello, i want to go the next scene yo");
            //playerselection.startScreen = 1; -- go to player Selection scene
            //yield return new WaitForSeconds(0.1f); //RDG Commented out to make time for loading the next scene.
            //panAnimation.Play("pan_stopped"); //RDG Commented out to make time for loading the next scene.
            ConnectToRoom();
        }
        else
        {
            panAnimation.Play("pan_window");
            yield return new WaitForSeconds(17f/60f);
            bool buttonPressed = false;
            numberOfPlayers[0].transform.parent.gameObject.SetActive(true);

            while (!buttonPressed)
            {
                
                foreach (SpriteRenderer sprite in numberOfPlayers)
                {
                    if (sprite != numberOfPlayers[currentSelection])
                    {
                        if (numberOfPlayers[currentSelection] == sprite)
                        {
                            sprite.transform.localScale = bouncy.Evaluate(time) * Vector3.one;
                            sprite.color = Color.white;
                        }
                        else
                        {
                            sprite.transform.localScale = Vector3.one * 0.5f;
                            sprite.color = unselectedColor;
                        }
                    }
                }

                if (Input.GetAxis("Horizontal_All") < -0.5f || Input.GetButtonDown("MenuLeft"))
                {
                    if (currentSelection > 0)
                    {
                        ChangeSelection(-1);

                    }
                }
                else if (Input.GetAxis("Horizontal_All") > 0.5f || Input.GetButtonDown("MenuRight"))
                {
                    if (currentSelection < numberOfPlayers.Length - 1)
                    {
                        ChangeSelection(1);
                    }
                }
                if (Input.GetButtonDown("Submit"))
                {
                    buttonPressed = true;
                }

                yield return null;
            }
            panAnimation.Play("pan_continue");
            yield return new WaitForSeconds(73f / 60f);
            ConnectToRoom(currentSelection + 1);
        }
    }

    public void ConnectToRoom()
    {
        int count = 0;
        foreach (Text t in playerTexts)
        {
            if (t.gameObject.GetActive() == true)
            {
                count++;
            }
        }
        Launcher.Instance.localPlayerAmount = count;
        Launcher.Instance.Connect();
    }

    public void ConnectToRoom(int count)
    {
        Launcher.Instance.localPlayerAmount = count;
        Launcher.Instance.Connect();
    }

    // Update is called once per frame
    private void Update()
    {

        time += Time.deltaTime;
        if (time > 2f)
        {
            time -= 2f;
        }

        if (bannerIn.activeSelf)
        {
            int i = 0;

            if (Input.GetAxis("Horizontal_All") < -0.5f || Input.GetButtonDown("MenuLeft"))
            {
                if (currentSelection > 0)
                {
                    ChangeSelection(-1);
                    
                }
            }
            else if (Input.GetAxis("Horizontal_All") > 0.5f || Input.GetButtonDown("MenuRight"))
            {
                if (currentSelection < menuItems.Length-1 && currentLevel == 0)
                {
                    ChangeSelection(1);

                } else if (currentSelection < playModes.Length - 1 && currentLevel == 1)
                {
                    ChangeSelection(1);
                } 
            }
            else
            {
            }
            ++i;

        }

        //shipCam.depth = 2;

        //Checking in!
        int counter = 0;
        foreach (Text t in playerTexts)
        {
            if (t.gameObject.GetActive() == true)
            {
                switch (counter)
                {
                    case 0:

                        break;
                    case 1:
                        if ((Input.GetButtonDown("Fire1_2") || Input.GetButtonDown("Fire2_2")) || Input.GetKeyDown(KeyCode.W) /*&& playerTexts[1].gameObject.GetActive()*/)
                        {
                            playerTexts[1].gameObject.SetActive(false);
                        }
                        break;
                    case 2:
                        if ((Input.GetButtonDown("Fire1_3") || Input.GetButtonDown("Fire2_3")) || Input.GetKeyDown(KeyCode.E)) /*&& playerTexts[2].gameObject.GetActive())*/
                        {
                            playerTexts[2].gameObject.SetActive(false);
                        }
                        break;
                    case 3:
                        if ((Input.GetButtonDown("Fire1_4") || Input.GetButtonDown("Fire2_4")) || Input.GetKeyDown(KeyCode.R)) /*&& playerTexts[3].gameObject.GetActive())*/
                        {
                            playerTexts[3].gameObject.SetActive(false);
                        }
                        break;
                }
            }
            else
            {
                switch (counter)
                {
                    case 0:

                        break;
                    case 1:
                        if (Input.GetButtonDown("Fire1_2") || Input.GetButtonDown("Fire2_2") || Input.GetKeyDown(KeyCode.Alpha2))
                        {
                            playerTexts[1].gameObject.SetActive(true);
                        }
                        break;
                    case 2:
                        if (Input.GetButtonDown("Fire1_3") || Input.GetButtonDown("Fire2_3") || Input.GetKeyDown(KeyCode.Alpha3))
                        {
                            playerTexts[2].gameObject.SetActive(true);
                        }
                        break;
                    case 3:
                        if (Input.GetButtonDown("Fire1_4") || Input.GetButtonDown("Fire2_4") || Input.GetKeyDown(KeyCode.Alpha4))
                        {
                            playerTexts[3].gameObject.SetActive(true);
                        }
                        break;
                }
            }
            counter++;
        }

        if (Input.GetButtonDown("Submit"))
        {
            if(currentLevel == 0) {
                if(currentSelection == 3)
                {
                    //Settings
                }
                else if (currentSelection == 2)
                {
                    //Credits
                    if (creditsScroll.GetComponent<Animator>().GetBool("ScrollShown"))
                    {
                        StartCoroutine(ExitCredits());
                    }
                    else
                    {
                        scrollShown = true;
                        creditsScroll.gameObject.SetActive(true);
                        creditsScroll.GetComponent<Animator>().SetBool("ScrollShown", true);
                    }
                }
                else if (currentSelection == 1)
                {
                    //play
                    //StartCoroutine(ChangeSceneAnimation());
                    StartCoroutine(ChangeLevel(1));
                }
                else if (currentSelection == 0)
                {
                    //Exit
                    Application.Quit();
                }
            } else if (currentLevel == 1)
            {

                if (currentSelection == 2)
                {
                    //Training
                    
                }
                else if (currentSelection == 1)
                {
                    //Local Scuffle
                    StartCoroutine(ChangeSceneAnimation(true));
                }
                else if (currentSelection == 0)
                {
                    //Online Scuffle
                    StartCoroutine(ChangeSceneAnimation(false));
                }
                
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if (creditsScroll.GetComponent<Animator>().GetBool("ScrollShown"))
            {
                StartCoroutine(ExitCredits());
            }
            else if (currentLevel == 1)
            {
                StartCoroutine(ChangeLevel(0));
            }

        }

        menuItems[0].transform.parent.localPosition = Vector3.Lerp(menuItems[0].transform.parent.localPosition, targetPos, 0.5f);
        if (currentLevel == 0) { 
            menuItems[currentSelection].transform.localScale = bouncy.Evaluate(time) * Vector3.one;
        } else if (currentLevel == 1)
        {
            playModes[currentSelection].transform.localScale = bouncy.Evaluate(time) * Vector3.one;
        }
    }

    private IEnumerator ExitCredits()
    {
        creditsScroll.GetComponent<Animator>().SetBool("ScrollShown", false);
        yield return new WaitForSeconds(0.3f);
        creditsScroll.gameObject.SetActive(false);
        scrollShown = false;
    }

    //private IEnumerator EnterCredits()
    //{
    //    //scrollVisible = true;
    //    if (creditsScroll != null)
    //    {
    //        creditsScroll.Play("scrollIn");
    //    }

    //    bool stop = true;
    //    yield return null;
    //    while (stop)
    //    {
    //        if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.Return))
    //        {
    //            stop = false;
    //        }
    //        yield return null;
    //    }
    //    //scrollVisible = false;
    //    if (creditsScroll != null)
    //    {
    //        creditsScroll.Play("scrollOut");
    //    }
    //}
}