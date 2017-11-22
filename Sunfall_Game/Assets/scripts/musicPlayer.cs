using UnityEngine;
using System.Collections;
using UnityEngine.Audio;


public class musicPlayer : MonoBehaviour
{

    public AudioMixer mixer;

    //public AudioSource[] sources;

    public float musicVolume;

    public AudioSource currentSource = null;

    //Convert.ToInt32();

    // Use this for initialization
    void Start()
    {
        //Play(currentSource);
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void GotoTime(AudioSource source, float time)
    {
        source.time = time;
    }

    public void Play(AudioSource source)
    {
        if (currentSource != null)
        {
            if (currentSource != source)
            {
                currentSource = source;
                source.Play();
            }
        }
        else
        {
            currentSource = source;
            source.Play();
        }
    }

    public void PlayAfterDelay(AudioSource source, float waitTime)
    {

        WaitAndPlay(source, waitTime);

    }

    public void PlayWithFadeIn(AudioSource source, float fadeTime)
    {
        if (currentSource != null)
        {
            if (currentSource != source)
            {
                StartCoroutine(FadeIn(source, fadeTime));

                source.Play();
            }
        }
        else
        {
            StartCoroutine(FadeIn(source, fadeTime));

            source.Play();
        }
    }

    private IEnumerator WaitAndPlay(AudioSource source, float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        if (currentSource != null)
        {
            if (currentSource != source)
            {

                currentSource.Stop();
                source.volume = musicVolume;
                source.Play();
                source = currentSource;

            }
        }
        else
        {

            currentSource.Stop();
            source.volume = musicVolume;
            source.Play();
            source = currentSource;

        }
    }

    private IEnumerator FadeIn(AudioSource source, float fadeTime)
    {

        AudioSource prev = currentSource;
        currentSource = source;
        float progress = 0f;
        //source.volume = 0.0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime / fadeTime;

            currentSource.volume = Mathf.Lerp(0f, musicVolume, progress);
            prev.volume = Mathf.Lerp(musicVolume, 0f, progress);
            yield return null;


        }
        prev.Stop();


    }

    // Update is called once per frame
    void Update()
    {
        if (currentSource != null)
        {
            if (currentSource.pitch != Time.timeScale)
            {
                currentSource.pitch = Time.timeScale;
            }
        }
    }
}
