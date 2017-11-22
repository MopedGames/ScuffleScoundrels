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

    public int powerUpId;

    public Animation animator;
    public AnimationClip spawnAnimation;
    public AnimationClip pickupAnimation;
    public AnimationClip idleAnimation;
    public float removeTime = 0f;
    public bool collected = false;

    

    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {

        Ship ship = col.gameObject.GetComponent<Ship>();
        if (ship != null)
        {
            //Give ship PowerUp
            ship.GetComponent<PhotonView>().RPC("PUNGivePowerUps", PhotonTargets.All, GetComponent<PhotonView>().viewID); //Give ship powerup
            GetComponent<PhotonView>().RPC("PUNPlayPowerUpAudio", PhotonTargets.All); //Play audio
            if (destroyOnActivate)
            {
                StartCoroutine(Remove(removeTime));
            }
        }
        else
        {
            //Spawn Explosion
            //Debug.Log("Spawn powerup explosion?");
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
            if (animator != null)
            {
                animator.Play(pickupAnimation.name, PlayMode.StopAll);

            }
        }
    }

    override public void OnSpawn()
    {
        base.OnSpawn();
        Debug.Log("spawn");
        if (animator != null && spawnAnimation != null && idleAnimation != null)
        {
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        collected = true;
        animator.Play(spawnAnimation.name, PlayMode.StopAll);
        yield return new WaitForSeconds(spawnAnimation.length - 1f);
        collected = false;
        yield return new WaitForSeconds(1f);
        animator.Play(idleAnimation.name, PlayMode.StopAll);


    }

    void Remove()
    {
        if (animator != null)
        {
            animator.Play(idleAnimation.name, PlayMode.StopAll);
        }
        collected = false;
        spawner.Despawn(gameObject, hazard);
    }

    IEnumerator Remove(float time)
    {

        yield return new WaitForSeconds(time);
        if (animator != null)
        {
            animator.Play(idleAnimation.name, PlayMode.StopAll);
        }
        collected = false;
        spawner.Despawn(gameObject, hazard);

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
