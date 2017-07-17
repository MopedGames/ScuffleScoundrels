using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class Controls {
	public string axis;
	public KeyCode leftKeyboard;
	public KeyCode rightKeyboard;
	public KeyCode shootLeftKeyboard;
	public KeyCode shootRightKeyboard;
	public KeyCode shootLeft;
	public KeyCode shootRight;
	public KeyCode shootLeftAlt;
	public KeyCode shootRightAlt;

	public KeyCode controls;
	public KeyCode exit;
	public KeyCode pause;
	public KeyCode pauseAlt;

}

[System.Serializable]
public class Cannon {

	public Transform cannonTransform;
	public Transform cannonOrigin;
	public bool active = true;

	public Vector3 startPos;
	public Vector3 targetPos;

	public ParticleSystem blast;

}

public class Ship : MonoBehaviour {

    public string name;

	public bool suddenDeath = false;

	public int id;

	public Material material;

	public Sprite logo;
	public Sprite WinRendition;

	public Controls controls;

	public UnityEngine.UI.Text pointText;
	public AnimationCurve coinAnimation;
	public UnityEngine.UI.Image[] coins;

    public Vector3 startPos;
    public Quaternion startRot;
	public bool alive = true;
	public int deaths = 0;
	public int kills = 0;
	
	public powerStats standardStats;
	public powerStats currentStats;

	private float currentSteering;
	public Cannon[] cannons;

	// X-Y-Z-W : top-bottom-right-left
	public Vector4 frameLimits;

	private LineRenderer trail;

	private float powerTimer;

	public float invulnerableTime = 1.0f;
	public bool invulnerable = false;

	private ParticleSystem particle;

	public Transform hull;

	private WinScreen winScreen;

	public Vector3 moveForce = new Vector3(0,0,0);

	public string flagAnimation;

	private bool shootalive = false;

	public Transform tiltParent;



	/*void OnCollisionEnter (){

		if (alive) {
			StartCoroutine("Die");
		}
	}*/

	public IEnumerator Slowdown () {
		Time.timeScale = 0.2f;
		Time.fixedDeltaTime = Mathf.Lerp(0.0f, 0.02f,0.2f);
		yield return new WaitForSeconds (0.15f);

		float deltaTimescale = 0f;
		while (Time.timeScale < 1.0f || Time.timeScale < deltaTimescale) {

			Time.timeScale += Time.deltaTime;
			Time.fixedDeltaTime = Mathf.Lerp(0.0f, 0.02f,Time.timeScale);
			yield return null;
			deltaTimescale = Time.timeScale;
		}
		Time.timeScale = 1.0f;
		Time.fixedDeltaTime = Mathf.Lerp(0.0f, 0.02f,1f);



	}

	public IEnumerator Die (){

		if (!invulnerable) {

			StartCoroutine (Slowdown ());

			yield return null;
			invulnerable = true;
			alive = false;
			++deaths;
			transform.position = new Vector3 (100, 100, 100);

			if(suddenDeath){


			} else {
				
				yield return new WaitForSeconds (2.0f);
				if (!winScreen.winState || suddenDeath) {
					transform.position = startPos;
					transform.rotation = startRot;
					alive = true;


					//Beep beep
					for (int i = 0; i < 3; ++i) {
						yield return new WaitForSeconds (invulnerableTime / 8f);
						material.SetColor ("_Color", new Color (0.5f, 0.5f, 0.5f, 0f));
						yield return new WaitForSeconds (invulnerableTime / 8f);
						material.SetColor ("_Color", new Color (0f, 0f, 0f, 0f));
					}
					//Bipbipbipbip
					for (int i = 0; i < 5; ++i) {
						yield return new WaitForSeconds (invulnerableTime / 16f);
						material.SetColor ("_Color", new Color (0.5f, 0.5f, 0.5f, 0f));
						yield return new WaitForSeconds (invulnerableTime / 16f);
						material.SetColor ("_Color", new Color (0f, 0f, 0f, 0f));
					}
				}
				invulnerable = false;

			}

		}
	}



	public void Spawn () {
		Debug.Log ("spawn");
		deaths = 0;
		kills = 0;
		transform.position = startPos;
		transform.rotation = startRot;
		alive = true;
		material.SetColor("_Color", new Color(0f,0f,0f,0f));

	}


	public IEnumerator GivePowerUp (powerStats powerUp, Color color, float duration){

		powerTimer = 0f;
	
		currentStats.projectile = powerUp.projectile;
		currentStats.reloadTime = powerUp.reloadTime;
		particle.startColor = color;
		ParticleSystem.EmissionModule em = particle.emission;
		em.enabled = true;
	
		//Wait
		while (powerTimer <= duration && alive && !suddenDeath) {
			yield return null;

			powerTimer += Time.deltaTime;
		}

		//Remove Powers
		currentStats.projectile = standardStats.projectile;
		currentStats.reloadTime = standardStats.reloadTime;
		em.enabled = false;

	}

	public void EnterSargasso(float speed, float steering){

		powerStats newStats = currentStats;
		newStats.speed = speed;
		newStats.steering = steering;

		//currentStats = newStats;
	}

	public void ExitSargasso(){
		currentStats.speed = standardStats.speed;
		currentStats.steering = standardStats.steering;
	}

	void Awake() {

	

		startPos = transform.position;
		startRot = transform.rotation;

		trail = GetComponentInChildren<LineRenderer> ();
		particle = GetComponent<ParticleSystem> ();

		winScreen = FindObjectOfType<WinScreen> ();

		hideAllCoins ();

		foreach (Cannon c in cannons) {
			c.startPos = c.cannonTransform.localPosition;
		}
	}

	void CheckFrame () {
		//Check Top-Bottom Frame
		if (transform.position.z > frameLimits.x) {
			trail.enabled = false;
			transform.position = new Vector3(transform.position.x,
			                                 transform.position.y,
			                                 frameLimits.y);
			trail.enabled = true;
		} else if (transform.position.z < frameLimits.y) {
			trail.enabled = false;
			transform.position = new Vector3(transform.position.x,
			                                 transform.position.y,
			                                 frameLimits.x);
			trail.enabled = true;
		}
		//Check Right-Left Frame
		if (transform.position.x > frameLimits.z) {
			trail.enabled = false;
			transform.position = new Vector3(frameLimits.w,
			                                 transform.position.y,
			                                 transform.position.z);
			trail.enabled = true;
		} else if (transform.position.x < frameLimits.w) {
			trail.enabled = false;
			transform.position = new Vector3(frameLimits.z,
			                                 transform.position.y,
			                                 transform.position.z);
			trail.enabled = true;
		}
	}

	void TrailTurn(){
			
		trail.SetPosition(1, new Vector3(-0.5f, 0f, -0.1f*currentSteering/currentStats.steering));
		trail.SetPosition(2, new Vector3(-1f, 0f, -0.25f*currentSteering/currentStats.steering));
		trail.SetPosition(3, new Vector3(-1.5f, 0f, -0.5f*currentSteering/currentStats.steering));
		trail.SetPosition(4, new Vector3(-2f, 0f, -0.75f*currentSteering/currentStats.steering));

	}

	void FireCannon(Cannon cannon, Vector3 force){
		if (cannon.blast != null) {
			cannon.blast.Play ();
		}

		if (currentStats.projectile != null) {
			cannonBall thisProjectile;
			if (!cannon.cannonOrigin) {
				thisProjectile = Instantiate (currentStats.projectile, cannon.cannonTransform.position, transform.rotation) as cannonBall;
			} else {
				thisProjectile = Instantiate (currentStats.projectile, cannon.cannonOrigin.position, transform.rotation) as cannonBall;
			}
			thisProjectile.owner = this;
			thisProjectile.GetComponent<Rigidbody>().velocity = force;

			StartCoroutine ("ReloadCannon", cannon);
		}
	}

	IEnumerator ReloadCannon (Cannon cannon){

		if (tiltParent) {
			cannon.cannonTransform.localScale = Vector3.one * 0.1f;
		} else {
			cannon.targetPos = new Vector3 (cannon.startPos.x,
											cannon.startPos.y,
											0.0f);
		}
		cannon.active = false;
		float t = 0f;
		while(t < currentStats.reloadTime){
			yield return null;
			t += Time.deltaTime;
			if (tiltParent) {
				cannon.cannonTransform.localScale = Vector3.one * (t/currentStats.reloadTime);
			} else {
				cannon.cannonTransform.localPosition = Vector3.Lerp (cannon.cannonTransform.localPosition,
																	cannon.targetPos,
																	0.1f);
			}
		}

		if (tiltParent) {
			cannon.cannonTransform.localScale = Vector3.one;
		} else {
			cannon.targetPos = cannon.startPos;
		}

		/*while(Vector3.Distance(cannon.cannonTransform.localPosition, cannon.startPos) < 0.01f){
			yield return null;
			cannon.cannonTransform.localPosition = Vector3.Lerp (cannon.cannonTransform.localPosition,
			                                                     cannon.startPos,
			                                                     0.1f);
		}*/
		cannon.cannonTransform.localPosition = cannon.startPos;
		cannon.active = true;
	}

	void hideAllCoins(){
		foreach (UnityEngine.UI.Image c in coins) {
			RectTransform cRect = c.rectTransform;
			cRect.localScale = Vector3.zero;
		}
	}

	public IEnumerator GetPoint (){
		yield return new WaitForSeconds (2.5f);
		if (kills < coins.Length) {
			RectTransform cRect = coins [kills].rectTransform;
			++kills;
			float i = 0;
			while (i < 1) {
				cRect.localScale = Vector3.one * coinAnimation.Evaluate (i);
				i += Time.deltaTime * 4;
				yield return null;
			}
		}
	}

	public IEnumerator RemovePoint (){
		if (kills > 0) {
			RectTransform cRect = coins [kills-1].rectTransform;
			--kills;
			float i = 0;
			while (i < 1f) {
				cRect.localScale = Vector3.one - (Vector3.one * coinAnimation.Evaluate (i));
				i += Time.deltaTime * 4;
				yield return null;
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		if (tiltParent) {
			tiltParent.transform.eulerAngles = new Vector3 (45f,0f, 0f);
			tiltParent.GetChild(0).transform.localEulerAngles = new Vector3(0f,transform.eulerAngles.y,0f);
		}

		if (coins.Length == 0) {
			pointText.text = kills.ToString ();
		}
		if (!winScreen.pause && kills > -1 && Input.GetKeyDown (controls.pause)) {
			StartCoroutine(winScreen.Pause ());

		} 

		if (alive) {

			if (Input.GetKeyDown (controls.shootRight) || Input.GetKeyDown (controls.shootRightAlt) || Input.GetKeyDown (controls.shootRightKeyboard) ||
				Input.GetKeyDown (controls.shootLeft) || Input.GetKeyDown (controls.shootLeftAlt) || Input.GetKeyDown (controls.shootLeftKeyboard)) {
				shootalive = true;
			}

			//TrailOffset
			trail.material.SetTextureOffset ("_MainTex", new Vector2 (-1.25f * Time.time * currentStats.speed, 0f));
			TrailTurn ();

			//Shoot Cannons
			Vector3 force;
			if (cannons [0].active && shootalive) {
				if (Input.GetKey (controls.shootRight) || Input.GetKey (controls.shootRightAlt) || Input.GetKey (controls.shootRightKeyboard)) {
					force = transform.forward * -1 * currentStats.shootForce;
					FireCannon (cannons [0], force);
				}
			}
			if (cannons [1].active && shootalive) {
				if (Input.GetKey (controls.shootLeft) || Input.GetKey (controls.shootLeftAlt) || Input.GetKey (controls.shootLeftKeyboard)) {
					force = transform.forward * currentStats.shootForce;
					FireCannon (cannons [1], force);
				}
			}
		

			//Move forward
			transform.Translate (Vector3.right * currentStats.speed * Time.fixedDeltaTime);
			transform.position = new Vector3 (transform.position.x, 0.5f, transform.position.z);

			//Steering
			float leftInt = Convert.ToSingle (Input.GetKey (controls.leftKeyboard));
			float rightInt = Convert.ToSingle (Input.GetKey (controls.rightKeyboard));
			float horizontal = Mathf.Clamp ((rightInt - leftInt) + Input.GetAxis (controls.axis), -1f, 1f);
			currentSteering = horizontal * currentStats.steering;
			Vector3 currentRotation = new Vector3 (0, currentSteering, 0);
			transform.Rotate (currentRotation * Time.fixedDeltaTime);
			hull.localEulerAngles = Vector3.Lerp (hull.localEulerAngles, new Vector3 (horizontal * 13f, hull.eulerAngles.y, 0), 0.5f);

			//If you leave the frame, reappear in the other side
			CheckFrame ();
		} else {
			shootalive = false;
		}

	}
}
