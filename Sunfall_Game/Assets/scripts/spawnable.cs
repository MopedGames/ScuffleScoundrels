using UnityEngine;
using System.Collections;

public class spawnable : MonoBehaviour
{

    public float safeDistance;

    public GameObject thingsToHide;

    public bool isEnabled;

    public bool hazard;

    public virtual void OnSpawn()
    {
        GetComponent<PhotonView>().RPC("PUNOnSpawn", PhotonTargets.All);
    }

    public virtual void OnDespawn()
    {
        GetComponent<PhotonView>().RPC("PUNOnDespawn", PhotonTargets.All);
    }

    [PunRPC]
    public void PUNOnSpawn(PhotonMessageInfo info)
    {
        //this.gameObject.SetActive(true);
        this.gameObject.transform.position = info.photonView.gameObject.transform.position;
        EnableThings();
    }

    [PunRPC]
    public void PUNOnDespawn(PhotonMessageInfo info)
    {
        //this.gameObject.SetActive(false);
        DisableThings();
    }

    public virtual void EnableThings()
    {
        thingsToHide.SetActive(true);
        GetComponent<Collider>().enabled = true;
        isEnabled = true;
    }

    public virtual void DisableThings()
    {
        thingsToHide.SetActive(false);
        GetComponent<Collider>().enabled = false;
        isEnabled = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if (stream.isWriting)
        //{
        //    stream.SendNext(gameObject.activeSelf);
        //}
        //else if (stream.isReading)
        //{
        //    gameObject.SetActive((bool)stream.ReceiveNext());
        //}
    }
}
