using UnityEngine;
using System.Collections;

public class cannonBall : MonoBehaviour
{

    public Ship owner;
    public GameObject Explosion;
    public GameObject Splash;
    public bool destroyOnImpact = true;
    private bool exploded = false;

    private int childsCollided = 0;
    private bool collided;
    private Collision collision;

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
   public void OnCollisionEnter(Collision col)
    {

        foreach (ContactPoint contact in col.contacts)
        {
            Ship ship = contact.otherCollider.transform.GetComponent<Ship>();
            collided = true;

            if (ship != null && owner != ship)
            {
                if (owner) //Test her om det hjælper - Ellers prøv at find på en måde hvor owner kan sættes igen
                {
                    if (owner.GetComponent<PhotonView>())
                    {
                        if (owner.GetComponent<PhotonView>().isMine)
                        {
                            if (Explosion != null && !exploded)
                            {
                                exploded = true;
                                GameObject ex;
                                Debug.Log("Cannonball explosion!");
                                Debug.Log(contact.otherCollider.transform.position);
                                ex = PhotonNetwork.Instantiate(Explosion.name, contact.otherCollider.transform.position, Quaternion.identity, 0) as GameObject;


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
                            if (!ship.invulnerable && ship.alive)
                            {
                                if (owner != null && owner != ship)
                                {
                                    GameObject coin;
                                    Debug.Log(contact.otherCollider.transform.position);
                                    coin = PhotonNetwork.Instantiate(owner.currentStats.rewardCoin.name, contact.otherCollider.transform.position, Quaternion.identity, 0) as GameObject;
                                    coin.GetComponent<rewardCoin>().target = (owner.startPos * 1.2f) - (Vector3.forward * 2.5f);

                                    owner.GetComponent<PhotonView>().RPC("PUNGetPoint", PhotonTargets.All); /*owner.StartCoroutine(owner.GetPoint());*/ //CMT

                                }
                                else if (ship.kills > 0)
                                {
                                    ship.GetComponent<PhotonView>().RPC("PUNRemovePoint", PhotonTargets.All); /* ship.StartCoroutine(ship.RemovePoint());*/ //CMT
                                }

                                ship.GetComponent<PhotonView>().RPC("PUNDie", PhotonTargets.All);
                                ship.alive = false;
                                ship.transform.position = new Vector3(100, 100, 100);
                            }


                            if (destroyOnImpact)
                            {
                                //if (GetComponent<PhotonView>())
                                //{
                                //    if (GetComponent<PhotonView>().viewID != 0)
                                //    {
                                        this.GetComponent<PhotonView>().RPC("Destroy", PhotonTargets.All);
                                //    }
                                //}
                            }

                        }
                    }
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
                    Destroy(gameObject);
                }
            }
        }
        exploded = false;
    }

    [PunRPC]
    public void Destroy(PhotonMessageInfo info)
    {
        if (info.photonView.gameObject == this.gameObject)
        {
            Destroy(this.gameObject); //TODO: This calls an "Illigal view ID 0"
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(owner);
        }
        else if (stream.isReading)
        {
            owner = (Ship)stream.ReceiveNext();
        }
    }
}

