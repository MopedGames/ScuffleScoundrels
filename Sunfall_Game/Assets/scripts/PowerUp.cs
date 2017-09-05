using UnityEngine;
using System.Collections;

[System.Serializable]
public class powerStats
{

    public GameObject rewardCoin;

    public float speed;
    public float steering;
    public float reloadTime = 1.0f;
    public cannonBall projectile;
    public float shootForce = 10.0f;

}

public class PowerUp : spawnable
{

    public Texture2D texture;
    public Renderer render;
    public float duration = 3.0f;
    public powerStats powers;

    public GameObject Explosion;
    public AudioClip audio;

    public bool destroyOnActivate = true;

    private AudioSource audiosource;

    public Color powerUpColor;

    public bool active = false;

    private PowerUpSpawner spawner;

    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {

        Ship ship = col.gameObject.GetComponent<Ship>();
        if (ship != null)
        {
            //Give ship PowerUp
            ship.GetComponent<PhotonView>().RPC("PUNGivePowerUps", PhotonTargets.All, GetComponent<PhotonView>().viewID); //Give ship powerup
            GetComponent<PhotonView>().RPC("PUNPlayPowerUpAudio", PhotonTargets.All); //Play audio

        }
        else
        {
            //Spawn Explosion
            PhotonNetwork.Instantiate("Explosion", transform.position, Quaternion.identity, 0);
        }
        if (destroyOnActivate)
        {
            Remove();
        }

    }

    [PunRPC]
    public void PUNPlayPowerUpAudio(PhotonMessageInfo info)
    {
        if (info.photonView.gameObject == this.gameObject)
        {
            audiosource.clip = audio;
            audiosource.Play();
        }
    }

    void Remove()
    {
        spawner.Despawn(gameObject);
    }

    // Update is called once per frame
    void Awake()
    {
        if (texture != null)
        {
            if (render == null)
            {
                render = GetComponentInChildren<MeshRenderer>();
            }

            render.material.SetTexture("_MainTex", texture);
        }

        audiosource = Camera.main.GetComponent<AudioSource>();
        spawner = FindObjectOfType<PowerUpSpawner>();

    }
}
