using UnityEngine;
using System.Collections;

[System.Serializable]
public class Banner
{
    public GameObject banner;
    public animateUv[] scripts;
    public bool wasActive;
}

public class bannerManagement : MonoBehaviour
{
    public Banner[] banners;

    // Use this for initialization
    private void Start()
    {
    }

    private void WakeUp(animateUv[] scripts)
    {
        foreach (animateUv s in scripts)
        {
            s.Awake();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        foreach (Banner b in banners)
        {
            if (b.banner.activeInHierarchy && !b.wasActive)
            {
                WakeUp(b.banner.GetComponentsInChildren<animateUv>());
            }
            b.wasActive = b.banner.activeInHierarchy;
        }
    }
}