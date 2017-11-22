using UnityEngine;
using System.Collections;

public class CalibrateControls : MonoBehaviour
{
    //Red - white - green - black
    public GameObject[] buttonPicture;

    public Controls[] controls;

    public bool calibrate = true;

    /*
	Horizontal_Color
	Fire1_Color
	Fire2_Color
	*/

    // Use this for initialization
    private void Awake()
    {
        Cursor.visible = false;
        Screen.SetResolution(1440, 900, true);
        DontDestroyOnLoad(transform.gameObject);
        if (calibrate)
        {
            StartCoroutine(CheckButtons());
        }
        else {
            SetButtons();
        }
    }
    //Red-white-green-black
    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            GameObject[] ships = GameObject.FindGameObjectsWithTag("ship");
            foreach (GameObject g in ships)
            {
                Ship ship = g.GetComponent<Ship>();
                if (ship != null)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (ship.id == i)
                        {
                            ship.controls = controls[i];
                        }
                    }
                }
            }
        }
    }

    private void SetButtons()
    {
        Debug.Log("joystick 1");

        controls[0].shootRight = KeyCode.Joystick1Button0;
        controls[0].shootLeft = KeyCode.Joystick1Button1;
        controls[0].axis = "Horizontal_Red";

        Debug.Log("joystick 2");

        controls[1].shootRight = KeyCode.Joystick2Button0;
        controls[1].shootLeft = KeyCode.Joystick2Button1;
        controls[1].axis = "Horizontal_White";

        Debug.Log("joystick 3");

        controls[2].shootRight = KeyCode.Joystick3Button0;
        controls[2].shootLeft = KeyCode.Joystick3Button1;
        controls[2].axis = "Horizontal_Green";

        Debug.Log("joystick 4");

        controls[3].shootRight = KeyCode.Joystick4Button0;
        controls[3].shootLeft = KeyCode.Joystick4Button1;
        controls[3].axis = "Horizontal_Black";

        Application.LoadLevel(1);
    }

    private IEnumerator CheckButtons()
    {
        int i = 0;

        Debug.Log("Check Buttons");

        foreach (Controls c in controls)
        {
            Debug.Log("listening");
            buttonPicture[i].SetActive(true);

            bool buttonPressed = false;
            while (!buttonPressed)
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button0))
                {
                    Debug.Log("joystick 1");
                    buttonPressed = true;
                    c.shootRight = KeyCode.Joystick1Button0;
                    c.shootLeft = KeyCode.Joystick1Button1;
                    c.rightKeyboard = KeyCode.Joystick1Button2;
                    c.leftKeyboard = KeyCode.Joystick1Button3;
                }
                if (Input.GetKeyDown(KeyCode.Joystick2Button0))
                {
                    Debug.Log("joystick 2");
                    buttonPressed = true;
                    c.shootRight = KeyCode.Joystick2Button0;
                    c.shootLeft = KeyCode.Joystick2Button1;
                    c.rightKeyboard = KeyCode.Joystick2Button2;
                    c.leftKeyboard = KeyCode.Joystick2Button3;
                }
                if (Input.GetKeyDown(KeyCode.Joystick3Button0))
                {
                    Debug.Log("joystick 3");
                    buttonPressed = true;
                    c.shootRight = KeyCode.Joystick3Button0;
                    c.shootLeft = KeyCode.Joystick3Button1;
                    c.rightKeyboard = KeyCode.Joystick3Button2;
                    c.leftKeyboard = KeyCode.Joystick3Button3;
                }
                if (Input.GetKeyDown(KeyCode.Joystick4Button0))
                {
                    Debug.Log("joystick 4");
                    buttonPressed = true;
                    c.shootRight = KeyCode.Joystick4Button0;
                    c.shootLeft = KeyCode.Joystick4Button1;
                    c.rightKeyboard = KeyCode.Joystick4Button2;
                    c.leftKeyboard = KeyCode.Joystick4Button3;
                }

                yield return null;
            }

            buttonPicture[i].SetActive(false);
            i++;
        }

        Application.LoadLevel(1);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log(Input.inputString);
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            Debug.Log("joystick 1");
        }
        if (Input.GetKeyDown(KeyCode.Joystick2Button0))
        {
            Debug.Log("joystick 2");
        }
        if (Input.GetKeyDown(KeyCode.Joystick3Button0))
        {
            Debug.Log("joystick 3");
        }
        if (Input.GetKeyDown(KeyCode.Joystick4Button0))
        {
            Debug.Log("joystick 4");
        }
    }
}