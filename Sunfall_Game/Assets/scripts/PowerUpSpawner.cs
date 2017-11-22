using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PowerUpSettings
{
    public GameObject powerup;
    public int amount;
    public bool fades;
    public float activeTime;
    public float acceptableDistance;
}

public class PowerUpSpawner : MonoBehaviour
{
    public PowerUpSettings[] powerUps;
    public List<GameObject> pool;
    public List<GameObject> active;
    public PowerUpSettings[] hazards;
    public List<GameObject> hazardPool;
    public List<GameObject> hazardActive;

    public Vector3 poolPos;

    //public GameObject[] powerUps;
    public int maxCrates = 5;

    public Vector4 frame;

    public float waitTime = 6.0f;
    public float hazardTime = 8.0f;
    public float timer = 0f;
    public float hazardTimer = 0f;

    public GameObject bubbles;

    public PlayerSelection selection;

    private IEnumerator Spawn(Vector3 spawnPos, bool hazard)
    {
        if (pool.Count > 0)
        {
            PhotonNetwork.Instantiate(bubbles.name, spawnPos, bubbles.transform.rotation, 0);
            //GetComponent<PhotonView>().RPC("PUNShowBubbles", PhotonTargets.All, spawnPos);
            yield return new WaitForSeconds(2f);
            //GetComponent<PhotonView>().RPC("PUNHideBubbles", PhotonTargets.All);

            int randomCrate;
            GameObject crate = null;

            if (hazard)
            {
                randomCrate = Random.Range(0, hazardPool.Count);
                if (hazardPool[randomCrate] != null)
                {
                    crate = hazardPool[randomCrate];
                }
            }
            else
            {
                randomCrate = Random.Range(0, pool.Count);
                if (pool[randomCrate] != null)
                {
                    crate = pool[randomCrate];
                }
            }
            if (crate != null)
            {
                crate.transform.position = spawnPos;
                crate.SetActive(true);
                spawnable spawned = crate.GetComponent<spawnable>();
                if (spawned != null)
                {
                    spawned.OnSpawn();
                }
                //GetComponent<PhotonView>().RPC("PUNOnSpawn", PhotonTargets.All, crate, spawnPos);

                if (hazard)
                {
                    hazardActive.Add(crate);
                    hazardPool.Remove(crate);

                }
                else
                {
                    active.Add(crate);
                    pool.Remove(crate);
                }
                ReturnToPool timer = crate.GetComponent<ReturnToPool>();
                if (timer != null)
                {
                    timer.StartTimer();
                }
            }
        }

        //Transform crate = Instantiate(powerUps[Random.Range(0,powerUps.Length)], spawnPos, Quaternion.identity) as Transform;
    }



    [PunRPC]
    public void PUNShowBubbles(Vector3 spawnPos, PhotonMessageInfo info)
    {
        //bubbles.position = spawnPos;
    }

    [PunRPC]
    public void PUNHideBubbles(PhotonMessageInfo info)
    {
        //bubbles.position = new Vector3(100f, 0f, 100f);
    }

    public void Despawn(GameObject crate, bool hazard)
    {
        //crate.SetActive(false);
        //crate.transform.position = poolPos;

        if (hazard)
        {
            hazardPool.Add(crate);
            hazardActive.Remove(crate);
        }
        else
        {
            pool.Add(crate);
            active.Remove(crate);

        }
        spawnable spawned = crate.GetComponent<spawnable>();
        if (spawned != null)
        {
            spawned.OnDespawn();
        }
    }

    public void Despawn(GameObject crate)
    {
        pool.Add(crate);
        active.Remove(crate);
        spawnable spawned = crate.GetComponent<spawnable>();
        if (spawned != null)
        {
            spawned.OnDespawn();
        }
        //crate.SetActive(false);
        //GetComponent<PhotonView>().RPC("PUNOnDespawn", PhotonTargets.All, crate);
    }



    private void Start()
    {
        if (PhotonNetwork.isMasterClient)
        {
            Fillpool();
        }
        //selection = FindObjectOfType<PlayerSelection>();
    }

    private void Fillpool()
    {
        foreach (PowerUpSettings p in powerUps)
        {
            int i = 0;
            while (i < p.amount)
            {
                GameObject crate = (GameObject)PhotonNetwork.Instantiate(p.powerup.name, poolPos, Quaternion.identity, 0);

                pool.Add(crate);
                i++;
            }
        }

        foreach (PowerUpSettings p in hazards)
        {
            int i = 0;
            while (i < p.amount)
            {
                GameObject crate = (GameObject)PhotonNetwork.Instantiate(p.powerup.name, poolPos, Quaternion.identity, 0);

                hazardPool.Add(crate);
                i++;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //if (selection)
        //{
        //    if (selection.isPlaying)
        //{
        if (PhotonNetwork.isMasterClient)
        {
            Transform[] children = GetComponentsInChildren<Transform>();
            if (children.Length < maxCrates)
            {
                timer += Time.deltaTime * (4.2f / GameStatusManager.Instance.players.Count);
                hazardTimer += Time.deltaTime * (GameStatusManager.Instance.players.Count / 3.2f);
            }
            if (pool.Count > 0 && timer >= waitTime)
            {
                timer = 0f;
                StartCoroutine(SpawnRequest(false));
            }

            if (hazardPool.Count > 0 && hazardTimer >= hazardTime)
            {
                hazardTimer = 0f;
                StartCoroutine(SpawnRequest(true));
            }
        }
    }

    private IEnumerator SpawnRequest(bool hazard)
    {
        float x = Mathf.Lerp(frame.x, frame.y, Random.Range(0f, 1f));
        float z = Mathf.Lerp(frame.z, frame.w, Random.Range(0f, 1f));
        Vector3 spawnPos = new Vector3(x, 0.17f, z);

        yield return null;

        bool foundPos = true;

        List<GameObject> allpowerups = new List<GameObject>();
        allpowerups.AddRange(active);
        allpowerups.AddRange(hazardActive);

        foreach (GameObject g in allpowerups)
        {
            if (Vector3.Distance(g.transform.position, spawnPos) < g.GetComponent<spawnable>().safeDistance)
            {
                SpawnRequest(hazard);
                foundPos = false;
                yield break;
            }
        }

        if (foundPos)
        {
            StartCoroutine(Spawn(spawnPos, hazard));
        }
        //crate.parent = transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(new Vector3(frame.x, 0.17f, frame.z), 1f);
        Gizmos.DrawSphere(new Vector3(frame.y, 0.17f, frame.z), 1f);
        Gizmos.DrawSphere(new Vector3(frame.x, 0.17f, frame.w), 1f);
        Gizmos.DrawSphere(new Vector3(frame.y, 0.17f, frame.w), 1f);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new NotImplementedException();
    }
}