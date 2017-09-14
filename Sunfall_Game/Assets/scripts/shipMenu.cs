using UnityEngine;
using System.Collections;

public class shipMenu : MonoBehaviour
{
    // which menu item is selected
    public int currentSelection = 1;

    // array if menu items
    public SpriteRenderer[] menuItems;

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
    public Transform cannon;
    public Transform visCannon;

    public GameObject cannonBall;
    public float cannonReload = 1f;

    public Ship[] ships;

    public musicPlayer musicPlayer;
    public AudioSource menuMusic;

    // Use this for initialization
    private void Awake()
    {
        ChangeSelection(0);
        targetPos = transform.localPosition;

        //launcher = Launcher.Instance;
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
        time = 0f;
        int oldSelection = currentSelection;
        currentSelection = Mathf.Clamp(currentSelection + selectionMod, 0, menuItems.Length - 1);

        if (oldSelection != currentSelection)
        {
            audioBlob.pitch = Random.Range(0.5f, 0.8f);
            audioBlob.Play();
            targetPos = new Vector3((currentSelection - 1) * -2, transform.localPosition.y, transform.localPosition.z);
            foreach (Renderer r in menuItems)
            {
                if (menuItems[currentSelection] == r)
                {
                    r.transform.localScale = Vector3.one;
                    r.material.color = Color.white;
                }
                else
                {
                    r.transform.localScale = Vector3.one * 0.5f;
                    r.material.color = unselectedColor;
                }
            }
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
    public IEnumerator ChangeSceneAnimation()
    {
        bannerIn.SetActive(false);
        bannerOut.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        panAnimation.Play("pan_panning");

        yield return new WaitForSeconds(1.5f);

        Debug.Log("hello, i want to go the next scene yo");
        //playerselection.startScreen = 1; -- go to player Selection scene
        //yield return new WaitForSeconds(0.1f); //RDG Commented out to make time for loading the next scene.
        //panAnimation.Play("pan_stopped"); //RDG Commented out to make time for loading the next scene.
        ConnectToRoom();
    }

    public void ConnectToRoom()
    {
        Launcher.Instance.Connect();
    }

    // Update is called once per frame
    private void Update()
    {
        // this shouldnt be here:
        visCannon.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, cannonReload);

        if (cannonReload < 1f)
        {
            cannonReload += Time.deltaTime;
        }

        time += Time.deltaTime;
        if (time > 2f)
        {
            time -= 2f;
        }

        if (bannerIn.activeSelf)
        {
            int i = 0;

            //foreach (Ship s in ships)
            //{
            //    if (s != null)
            //    { // KeyCode.JoystickButton0
            if (Input.GetAxis("Horizontal_All") < -0.5f || Input.GetKeyDown(KeyCode.A))
            {
                if (!buttonactive[i])
                {
                    ChangeSelection(-1);
                    buttonactive[i] = true;
                }
            }
            else if (Input.GetAxis("Horizontal_All") > 0.5f || Input.GetKeyDown(KeyCode.D))
            {
                if (!buttonactive[i])
                {
                    ChangeSelection(1);
                    buttonactive[i] = true;
                }
            }
            else {
                buttonactive[i] = false;
            }
            ++i;
            //    }
            //}
        }

        //shipCam.depth = 2;
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (currentSelection == 2)
            {
                StartCoroutine(EnterCredits());
            }
            else if (currentSelection == 1)
            {
                StartCoroutine(ChangeSceneAnimation());
            }
            else if (currentSelection == 0)
            {
                Application.Quit();
            }
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, 0.5f);
        menuItems[currentSelection].transform.localScale = bouncy.Evaluate(time) * Vector3.one;
    }

    private IEnumerator EnterCredits()
    {
        scrollVisible = true;
        if (creditsScroll != null)
        {
            creditsScroll.Play("scrollIn");
        }

        bool stop = true;
        yield return null;
        while (stop)
        {
            foreach (Player p in players)
            {
                if (Input.GetKeyDown(p.ship.controls.controls) || Input.GetKeyDown(KeyCode.Return))
                {
                    stop = false;
                }
            }
            yield return null;
        }
        scrollVisible = false;
        if (creditsScroll != null)
        {
            creditsScroll.Play("scrollOut");
        }
    }
}