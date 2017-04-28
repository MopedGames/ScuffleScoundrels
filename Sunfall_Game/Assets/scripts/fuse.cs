using UnityEngine;
using System.Collections;

public class fuse : MonoBehaviour {

	public float totalTime = 120f;
    public float timer = 120f;

	private int currentTarget = 1;
	public Transform fuseLight;
    public Vector3[] fusePoints;
    public float Speed;

    private Renderer renderer;

	private bool play = false;
	public float totalDistance = 0f;
	private float currentDistance = 0f;

	private float lerpTimer;

    public GameObject Explosion;
    public Transform bomb;

    private bool done = false;
	public void Play(){
        
        bomb.gameObject.SetActive(true);
        play = true;
		timer = totalTime;
		fuseLight.position = fusePoints [0]+transform.position;
		currentTarget = 1;
		lerpTimer = 0;
		currentDistance = Vector3.Distance (fusePoints [currentTarget], fusePoints [currentTarget - 1]);
        done = false;
	}

	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
		calculateLength ();
	}

	void calculateLength () {
		Vector3 prevPoint = fusePoints [0];
		float currentLength = 0f;

		foreach (Vector3 p in fusePoints) {
			if (p != fusePoints [0]) {
				currentLength += Vector3.Distance(prevPoint,p);
				prevPoint = p;
			}
		}

		totalDistance = currentLength;

	}
	
    void Explode () {
        GameObject.Instantiate(Explosion, bomb.position, Quaternion.identity);
        bomb.gameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
		if (play && timer > 0) {
			timer -= Time.deltaTime;


			renderer.material.SetFloat ("_CutOff", (totalTime - timer) / totalTime);

			if (currentTarget < fusePoints.Length) {

				lerpTimer += (1 / (totalTime * (currentDistance / totalDistance))) * Time.deltaTime;
				fuseLight.position = transform.position + Vector3.Lerp (fusePoints [currentTarget - 1], fusePoints [currentTarget], lerpTimer);

				if (lerpTimer >= 1f) {
					++currentTarget;
					lerpTimer = 0f;
                    if (currentTarget > 0)
                    {
                        currentDistance = Vector3.Distance(fusePoints[currentTarget], fusePoints[currentTarget - 1]);
                    }
				}
			}
		} else if (timer <= 0 && !done) {
            Explode();
            done = true;
		}

	}

	void OnDrawGizmos () {

		foreach (Vector3 p in fusePoints) {
			Gizmos.DrawSphere (transform.position + p, 0.2f);
		}

	}
}
