using UnityEngine;
using System.Collections;
using Photon;

public class fuse : PunBehaviour
{
    public float totalTime = 120f;
    public float timer = 120f;

    public int currentTarget = 1;
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

    private WinScreen winscreen;

    [PunRPC]
    public void Play()
    {
        bomb.gameObject.SetActive(true);
        play = true;
        timer = totalTime;
        fuseLight.position = fusePoints[0] + transform.position;
        currentTarget = 1;
        lerpTimer = 0;
        currentDistance = Vector3.Distance(fusePoints[currentTarget], fusePoints[currentTarget - 1]);
        done = false;
    }

    // Use this for initialization
    private void Start()
    {
        renderer = GetComponent<Renderer>();
        calculateLength();
        winscreen = FindObjectOfType<WinScreen>();
    }

    private void calculateLength()
    {
        Vector3 prevPoint = fusePoints[0];
        float currentLength = 0f;

        foreach (Vector3 p in fusePoints)
        {
            if (p != fusePoints[0])
            {
                currentLength += Vector3.Distance(prevPoint, p);
                prevPoint = p;
            }
        }

        totalDistance = currentLength;
    }
    /// <summary>
    /// TODO: make this an RPC run by the masterclient on game over
    /// </summary>
    private void Explode()
    {
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("fuse explosion!");
            PhotonNetwork.Instantiate(Explosion.name, bomb.position, Quaternion.identity, 0);
        }
        //GameObject.Instantiate(Explosion, bomb.position, Quaternion.identity);
        Debug.Log("gameover");
        bomb.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (play && timer > 0)
        {
            //timer = winscreen.gameTimer;// FUse fix?
            timer -= Time.deltaTime;

            renderer.material.SetFloat("_CutOff", (totalTime - timer) / totalTime);

            if (currentTarget < fusePoints.Length)
            {
                lerpTimer += (1 / (totalTime * (currentDistance / totalDistance))) * Time.deltaTime;
                fuseLight.position = transform.position + Vector3.Lerp(fusePoints[currentTarget - 1], fusePoints[currentTarget], lerpTimer);

                if (lerpTimer >= 1f)
                {
                    ++currentTarget;
                    lerpTimer = 0f;
                    if (currentTarget > 0)
                    {
                        currentDistance = Vector3.Distance(fusePoints[currentTarget], fusePoints[currentTarget - 1]);
                    }
                }
            }
        }
        else if (timer <= 0 && !done)
        {
            Explode();
            done = true;
        }
    }

    private void OnDrawGizmos()
    {
        foreach (Vector3 p in fusePoints)
        {
            Gizmos.DrawSphere(transform.position + p, 0.2f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting /*&& PhotonNetwork.isMasterClient*/)
        {
            stream.SendNext(timer);
            stream.SendNext(currentTarget);
            //stream.SendNext(fuseLight.transform.position.x);
            //stream.SendNext(fuseLight.transform.position.y);
        }
        else
        {
            timer = (float)stream.ReceiveNext();
            int newTarget = (int)stream.ReceiveNext();
            if (newTarget > currentTarget)
            {
                currentTarget = newTarget;
            }
            //fuseLight.transform.position.x = (float)stream.ReceiveNext();
        }
    }
}