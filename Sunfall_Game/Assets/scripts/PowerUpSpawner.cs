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

    public Vector3 poolPos;

    //public GameObject[] powerUps;
    public int maxCrates = 5;

    public Vector4 frame;

    public float waitTime = 6.0f;
    private float timer = 0f;

    public Transform bubbles;

    public PlayerSelection selection;

    private IEnumerator Spawn(Vector3 spawnPos)
    {
        if (pool.Count > 0)
        {
            bubbles.position = spawnPos;
            yield return new WaitForSeconds(2f);
            bubbles.position = new Vector3(100f, 0f, 100f);

            int randomCrate = Random.Range(0, pool.Count);
            GameObject crate = pool[randomCrate];

            crate.transform.position = spawnPos;
            crate.SetActive(true);
            spawnable spawned = crate.GetComponent<spawnable>();
            if (spawned != null)
            {
                spawned.OnSpawn();
            }

            active.Add(crate);
            pool.Remove(crate);
            ReturnToPool timer = crate.GetComponent<ReturnToPool>();
            if (timer != null)
            {
                timer.StartTimer();
            }
        }

        //Transform crate = Instantiate(powerUps[Random.Range(0,powerUps.Length)], spawnPos, Quaternion.identity) as Transform;
    }

    public void Despawn(GameObject crate)
    {
        pool.Add(crate);
        active.Remove(crate);
        crate.SetActive(false);
    }

    private void Start()
    {
        Fillpool();
        selection = FindObjectOfType<PlayerSelection>();
    }

    private void Fillpool()
    {
        foreach (PowerUpSettings p in powerUps)
        {
            int i = 0;
            while (i < p.amount)
            {
                GameObject crate = (GameObject)Instantiate(p.powerup, poolPos, Quaternion.identity);

                pool.Add(crate);
                i++;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (selection)
        {
            if (selection.isPlaying)
            {
                Transform[] children = GetComponentsInChildren<Transform>();
                if (children.Length < maxCrates)
                {
                    timer += Time.deltaTime;
                }
                if (timer >= waitTime)
                {
                    timer = 0f;
                    StartCoroutine(SpawnRequest());
                }
            }
        }
    }

    private IEnumerator SpawnRequest()
    {
        float x = Mathf.Lerp(frame.x, frame.y, Random.Range(0f, 1f));
        float z = Mathf.Lerp(frame.z, frame.w, Random.Range(0f, 1f));
        Vector3 spawnPos = new Vector3(x, 0.17f, z);

        yield return null;

        bool foundPos = true;

        foreach (GameObject g in active)
        {
            if (Vector3.Distance(g.transform.position, spawnPos) < g.GetComponent<spawnable>().safeDistance)
            {
                SpawnRequest();
                foundPos = false;
                yield break;
            }
        }

        if (foundPos)
        {
            StartCoroutine(Spawn(spawnPos));
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
}