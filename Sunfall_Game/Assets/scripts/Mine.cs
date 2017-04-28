using UnityEngine;
using System.Collections;

public class Mine : spawnable {

    public Ship owner;
    public GameObject Explosion;
    public GameObject Splash;
    public bool destroyOnImpact = true;
    private bool exploded = false;

    private int childsCollided = 0;
    private bool collided;
    private Collision collision;

    private PowerUpSpawner spawner;

    void Start()
    {
        spawner = FindObjectOfType<PowerUpSpawner>();
    }

    public void ChildCollided(Collision col)
    {
        childsCollided++;
        collision = col;
    }

    void Update()
    {
        if (childsCollided > 0 && !collided)
        {

            OnCollisionEnter(collision);
        }
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision col)
    {

        foreach (ContactPoint contact in col.contacts)
        {
            Ship ship = contact.otherCollider.transform.GetComponent<Ship>();
            collided = true;

            if (ship != null && owner != ship)
            {
                if (Explosion != null && !exploded)
                {
                    exploded = true;
                    GameObject ex;
                    Debug.Log(gameObject.name + " caused explosion " + Explosion.name + ", at time: " + Time.time);
                    ex = Instantiate(Explosion, contact.otherCollider.transform.position, Quaternion.identity) as GameObject;


                    cannonBall c = ex.GetComponentInChildren<cannonBall>();
                    if (c != null)
                    {

                        if (owner == null)
                        {
                            c.owner = ship;
                        }
                        else
                        {
                            c.owner = owner;
                        }

                    }
                }

                if (!ship.invulnerable)
                {
                    ship.StartCoroutine(ship.Die());

                    if (owner != null)
                    {
                        GameObject coin;
                        coin = Instantiate(owner.currentStats.rewardCoin, contact.otherCollider.transform.position, Quaternion.identity) as GameObject;
                        coin.GetComponent<rewardCoin>().target = (owner.startPos * 1.2f) - (Vector3.forward * 2.5f);

                        owner.StartCoroutine(owner.GetPoint());

                    }
                    else if (ship.kills > 0)
                    {
                        ship.StartCoroutine(ship.RemovePoint());
                    }
                }


                if (destroyOnImpact)
                {
                    spawner.Despawn(gameObject);
                }

            }
            else if (ship == null)
            {
                //Spawn Splash
                if (Splash != null)
                {
                    GameObject ex;
                    ex = Instantiate(Splash, transform.position, Quaternion.identity) as GameObject;
                    cannonBall c = ex.GetComponentInChildren<cannonBall>();
                    if (c != null)
                    {
                        c.owner = owner;
                    }
                }

                if (destroyOnImpact)
                {
                    spawner.Despawn(gameObject);
                }
            }
        }
        exploded = false;

    }
}
