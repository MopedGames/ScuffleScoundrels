using UnityEngine;
using System.Collections;

public class shipMenu : MonoBehaviour {

    public int currentSelection = 1;

	public SpriteRenderer[] menuItems;

	private Vector3 targetPos;

    //public Vector3 unselectedPos;
    public Color unselectedColor;

	public AnimationCurve bouncy;

    public AudioSource audioBlob;
	public AudioSource audioPling;

    public Animation panAnimation;
	public GameObject bannerIn;
	public GameObject bannerOut;

	public float time;

	public Transform cannon;
	public Transform visCannon;
	public GameObject cannonBall;
	public float cannonReload = 1f;

	public Ship[] ships;

	private bool activeMenu = true;

    private bool[] buttonactive = new bool[4];

	// Use this for initialization
	void Awake () {
		ChangeSelection (0);
		targetPos = transform.localPosition;

	}

	public IEnumerator Shakecam(){

		yield return new WaitForSeconds (1.9f);
		float shake = 0f;
		//Vector3 startpos = transform.parent.position;
		while (shake < 1f) {
			transform.parent.localPosition = Random.insideUnitSphere * 0.1f;
			shake += Time.deltaTime * 3f;

			yield return null;
		}
	}
	
	void ChangeSelection (int selectionMod){
		time = 0f;
        int oldSelection = currentSelection;
		currentSelection = Mathf.Clamp (currentSelection + selectionMod, 0, menuItems.Length - 1);

        if (oldSelection != currentSelection) {
			audioBlob.pitch = Random.Range (0.5f, 0.8f);
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

	public void startBanner (){
		bannerIn.SetActive (true);
		bannerOut.SetActive (false);
	}

	public IEnumerator ChangeSceneAnimation (PlayerSelection playerselection){

		bannerIn.SetActive (false);
		bannerOut.SetActive (true);

		yield return new WaitForSeconds (0.1f);

        panAnimation.Play("pan_panning");

		yield return new WaitForSeconds (1.5f);

		playerselection.startScreen = 1;
		yield return new WaitForSeconds (0.1f);
		panAnimation.Play("pan_stopped");


    }



	// Update is called once per frame
	void Update () {
		
		visCannon.localScale = Vector3.Lerp (Vector3.zero, Vector3.one, cannonReload);

		if (cannonReload < 1f) {
			cannonReload += Time.deltaTime;
		}

		time += Time.deltaTime;
		if (time > 2f) {
			time -= 2f;
		}

		if (bannerIn.activeSelf) {

            int i = 0;

			foreach (Ship s in ships) {
				if (Input.GetAxis (s.controls.axis) < -0.5f || Input.GetKeyDown (s.controls.leftKeyboard)) {
                    if(!buttonactive[i]){
                        ChangeSelection (-1);
                        buttonactive[i] = true;

                    }
                } else if (Input.GetAxis (s.controls.axis) > 0.5f || Input.GetKeyDown (s.controls.rightKeyboard)) {
                    if(!buttonactive[i]){
                        ChangeSelection (1);
                        buttonactive[i] = true;
                    } 
                } else {
                    buttonactive[i] = false;
                }
                ++i;
			}
		}
		transform.localPosition = Vector3.Lerp (transform.localPosition, targetPos, 0.5f);
		menuItems [currentSelection].transform.localScale = bouncy.Evaluate (time) * Vector3.one;

	}
}
