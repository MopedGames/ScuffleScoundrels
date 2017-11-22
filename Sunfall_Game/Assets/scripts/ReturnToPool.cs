using UnityEngine;
using System.Collections;

public class ReturnToPool : MonoBehaviour
{

    public float timeToReturn;
    public float timeToFade;
    private float fadeTime;
    private float timer;
    private PowerUpSpawner spawner;

    void Start()
    {
        spawner = FindObjectOfType<PowerUpSpawner>();
    }

    // Use this for initialization
    public void StartTimer()
    {
        GetComponent<PhotonView>().RPC("PUNStartTimer", PhotonTargets.All);
    }

    [PunRPC]
    public void PUNStartTimer(PhotonMessageInfo info)
    {
        if (info.photonView.gameObject == this.gameObject)
        {
            timer = timeToReturn;
            fadeTime = timeToFade;
            StartCoroutine(Timer());
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(timeToReturn);
        if (fadeTime > 0.01f)
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            //Beep beep
            for (int i = 0; i < 3; ++i)
            {
                yield return new WaitForSeconds(fadeTime / 8f);
                foreach (Renderer r in renderers)
                {
                    r.enabled = false;
                }
                yield return new WaitForSeconds(fadeTime / 8f);
                foreach (Renderer r in renderers)
                {
                    r.enabled = true;
                }
            }
            //Bipbipbipbip
            for (int i = 0; i < 5; ++i)
            {
                yield return new WaitForSeconds(fadeTime / 16f);
                foreach (Renderer r in renderers)
                {
                    r.enabled = false;
                }
                yield return new WaitForSeconds(fadeTime / 16f);
                foreach (Renderer r in renderers)
                {
                    r.enabled = true;
                }
            }

        }

        spawnable spawned = gameObject.GetComponent<spawnable>();
        if (spawned != null)
        {
            spawner.Despawn(gameObject, spawned.hazard);
            spawned.OnDespawn();
        }
    }
}
