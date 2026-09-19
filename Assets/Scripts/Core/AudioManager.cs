using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Источники звука")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    [Header("Клипы SFX")]
    public AudioClip eatClip;
    public AudioClip deathClip;
    public AudioClip clickClip;

    [Header("Музыка (опционально)")]
    public AudioClip backgroundMusic;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.volume = 0.3f;
            musicSource.Play();
        }
    }

    public void PlayEat() { PlayOneShot(eatClip); }
    public void PlayDeath() { PlayOneShot(deathClip); }
    public void PlayClick() { PlayOneShot(clickClip); }

    private void PlayOneShot(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
            sfxSource.PlayOneShot(clip);
    }
}