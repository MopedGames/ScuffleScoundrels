using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vortex : spawnable
{

    public float range = 1.6f;
    public float force = 0.1f;
    public AnimationCurve effectCurve;

    public float deathZone = 0.5f;

    public GameObject Splash;

    private List<Ship> ships = new List<Ship>();

    public float warmupTime = 5f;
    public float age = 0f;

    // Use this for initialization
    void Update()
    {
        if (isEnabled)
        {
            age = Mathf.Clamp(age + Time.deltaTime, 0f, warmupTime);
        }
    }

    void Awake()
    {
        GetComponent<SphereCollider>().radius = range;
    }

    void Swallow(Ship ship)
    {
        ship.GetComponent<PhotonView>().RPC("PUNDie", PhotonTargets.All);

        GameObject ex;
        ex = Instantiate(Splash, transform.position, Quaternion.identity) as GameObject;

        if (ship.kills > 0)
        {
            --ship.kills;
        }
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        foreach (Ship s in ships)
        {
            s.ExitSargasso();
        }
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        GetComponentInChildren<scaleAnimation>().Restart();
    }

    // Update is called once per frame
    void OnTriggerStay(Collider col)
    {
        Ship ship = col.gameObject.GetComponent<Ship>();
        if (ship != null)
        {
            if (!ship.invulnerable)
            {
                float dist = Vector3.Distance(transform.position, ship.transform.position);
                float f = force * effectCurve.Evaluate(1 - dist / range) * Time.deltaTime;
                Vector3 distVector = 2 * transform.position - ship.transform.position;
                Vector3 perpendicular = Vector3.Cross(Vector3.up, transform.position - ship.transform.position);
                Vector3 target = transform.position + (Vector3.Lerp(ship.transform.position - transform.position, perpendicular, 0.1f).normalized * (dist * 0.99f));//Vector3.Lerp ( perpendicular,transform.position, effectCurve.Evaluate ((1 - dist / range)/10));
                                                                                                                                                                    //Vector3 force = target;

                Debug.DrawLine(ship.transform.position, target);

                ship.transform.position = Vector3.Lerp(ship.transform.position, target, f * (age / warmupTime));
                ship.currentStats.speed = Mathf.Lerp(ship.standardStats.speed, 0, 1 - dist / range);
                //ship.moveForce = target;
                if (dist < deathZone)
                {
                    Swallow(ship);
                }
            }
        }
    }
    void OnTriggerEnter(Collider col)
    {
        Ship ship = col.gameObject.GetComponent<Ship>();
        if (ship != null)
        {

            ships.Add(ship);

        }

    }

    void OnTriggerExit(Collider col)
    {
        Ship ship = col.gameObject.GetComponent<Ship>();
        if (ship != null)
        {

            ship.currentStats.speed = ship.standardStats.speed;
            ships.Remove(ship);

        }
    }
}
