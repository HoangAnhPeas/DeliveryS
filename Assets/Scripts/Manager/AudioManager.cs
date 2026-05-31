using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip pickupClip;
    public AudioClip deliveryClip;
    public AudioClip buttonClip;
    public AudioClip gameOverClip;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadVolumes();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    // ===== MUSIC =====

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;

        PlayerPrefs.SetFloat(
            "MusicVolume",
            volume
        );
    }

    public void FadeOutMusic(float duration)
    {
        StartCoroutine(FadeMusicCoroutine(duration));
    }

    IEnumerator FadeMusicCoroutine(float duration)
    {
        float startVolume = musicSource.volume;

        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;

            musicSource.volume =
                Mathf.Lerp(startVolume, 0f, t / duration);

            yield return null;
        }

        musicSource.volume = 0f;
        musicSource.Stop();
    }

    // ===== SFX =====

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;

        PlayerPrefs.SetFloat(
            "SFXVolume",
            volume
        );
    }

    void LoadVolumes()
    {
        float musicVolume =
            PlayerPrefs.GetFloat(
                "MusicVolume",
                0.5f
            );

        float sfxVolume =
            PlayerPrefs.GetFloat(
                "SFXVolume",
                1f
            );

        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }
}