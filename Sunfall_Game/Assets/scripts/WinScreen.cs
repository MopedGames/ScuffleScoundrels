using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WinScreen : MonoBehaviour {

	public ParticleSystem[] winParticles;
	public Animator cannon;
	public string animationToPlay;

	public UnityEngine.UI.Text[] points;
	public UnityEngine.UI.Image[] sprites;
	public Ship[] ships;

	public bool pause = false;
	public bool winState = false;
	private float timer = 0f;

	private float gameTimer = 120f;
	public UnityEngine.UI.Text[] timers;

	public PlayerSelection playerSelection;

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



	void Start () {
		pauseCanvas.gameObject.SetActive(false);

	}

	void CheckForSuddenDeath () {
		//Is the leading ships tied?
		List<Ship> shipsByPoints = new List<Ship>();
		shipsByPoints = ships.OrderBy(go=>go.kills).ToList();

		if (shipsByPoints [2].kills == shipsByPoints [3].kills) {

			musicPlayer.Play (suddenDeathMusic);

			/*AudioSource a = GameObject.Find ("Adventure Meme").GetComponent<AudioSource> ();
			a.Stop();
			a.clip = suddenDeathMusic;
			suddenDeathSpeak.Play ();
			a.Play();*/

			suddenDeath = true;
			PowerUpSpawner p = GameObject.FindObjectOfType<PowerUpSpawner>();
			p.enabled = false;
            List<Ship> shipAlive = new List<Ship>();

			foreach(Ship ss in shipsByPoints){
				if(ss.kills >= shipsByPoints [3].kills){
					ss.GivePowerUp (ss.standardStats, Color.black, 0.0f);
                    shipAlive.Add(ss);
				} else {
					Instantiate(explosion,ss.transform.position,Quaternion.identity);
					ss.StartCoroutine(ss.Die());
				}
			}

			GameObject[] crates = GameObject.FindGameObjectsWithTag("powerUp");
			foreach(GameObject crate in crates){
				Instantiate(explosion,crate.transform.position,Quaternion.identity);
				Destroy(crate);
			}

			foreach (Ship s in ships) {
				s.suddenDeath = true;
			}

            playerSelection.SuddenDeathInit(shipAlive.ToArray());

		} else {
			Debug.Log ("No!");
			winState = true;
			timer = 0f;
			gameTimer = 90f; 
			ShowScreen ();

			//musicPlayer.PlayAfterDelay (fanfare, 0.0f);
		}
	}

	//Sort PointList
	void SortList () {

		List<Ship> shipsByPoints = new List<Ship>();
		shipsByPoints = ships.OrderBy(go=>go.kills).ToList();
		winnerRendition.sprite = shipsByPoints[3].WinRendition;
        winner = shipsByPoints[3].name;

		int i;
		for(i = 0; i < shipsByPoints.Count; i++){

			sprites[i].sprite = shipsByPoints[i].logo;

			points[i].text = shipsByPoints[i].kills.ToString();
		}

		//animationToPlay = shipsByPoints [3].flagAnimation;
	}

	// Use this for initialization
	void ShowScreen () {
        playerSelection.EndGame(this, winner);

	}

	public void ShowForReal () {

		musicPlayer.PlayWithFadeIn (winMusic, 1.0f);

		SortList ();
		GetComponent<Camera>().depth = 2f;
		foreach (Ship s in ships) {
			s.alive = false;
		}

		foreach (ParticleSystem p in winParticles) {
			p.Play();
		}

		cannon.Play(animationToPlay);
	}

    public void HideForReal () {
        //GetComponent<Camera>().depth = 0f;
		Application.LoadLevel (Application.loadedLevel);
    }

	public IEnumerator Pause (){

		pause = true;
		pauseCanvas.gameObject.SetActive(true);
        float timescaleWas = Time.timeScale;
		Time.timeScale = 0.0f;
		while (pause) {
            Time.timeScale = 0.0f;
            bool exitGame = true;
            foreach (Ship s in ships){
				if(Input.GetKeyDown(s.controls.controls)){
					pause = false;
				}
                if (!Input.GetKeyDown(s.controls.exit) && s.kills > -1)
                {
                    exitGame = false;
                }
			}
            if (exitGame)
            {
                Time.timeScale = 1f;
                Application.LoadLevel(Application.loadedLevel);
            }
			yield return null;
		}

        pauseCanvas.gameObject.SetActive(false);

        Time.timeScale = timescaleWas;

	}
	
	// Update is called once per frame
	void Update () {



		if (!winState && !pause) {

			foreach (UnityEngine.UI.Text t in timers){
				if(!suddenDeath){
					t.text = ((int)gameTimer).ToString ();
				} else {
					/*t.color = Color.red;
					t.fontSize = 10;
					t.text = ("SUDDEN DEATH");*/
                    


				}
				if(gameTimer < 10.01f){
					t.color = Color.red;
				} else {
					t.color = Color.white;
				}
			}

			if(playerSelection.isPlaying && !suddenDeath){
				gameTimer -= Time.deltaTime;
			
				foreach (Ship s in ships) {
					if (s.kills >= 10) {
						winState = true;
						timer = 0f;
						gameTimer = 90f; 
						ShowScreen ();
					}
				}

				if(gameTimer < 0){
					CheckForSuddenDeath();
				}
			} else if (playerSelection.isPlaying && suddenDeath) {
				int alive = 0;
				Ship lastStanding = null;
				foreach (Ship s in ships) {
					if (s.alive) {
						lastStanding = s;
						alive++;
					}
				}
				if(alive <= 1){
					lastStanding.kills += 2;
					winState = true;
					timer = 0f;
					gameTimer = 90f; 
					ShowScreen ();
				}
			}
		} else if (winState) {

			/*if(timer > 1f){

				timer += Time.deltaTime;

				foreach(Ship s in ships){
					if(Input.GetKeyDown(s.controls.controls)){

						playerSelection.MenuScreen();
						GetComponent<Camera>().depth = 0f;
					}
				}
				if (timer > 31f) {
					playerSelection.MenuScreen ();
					GetComponent<Camera> ().depth = 0f;
				}

			}else{

				timer += Time.deltaTime;
			}*/
		}
	}
}
