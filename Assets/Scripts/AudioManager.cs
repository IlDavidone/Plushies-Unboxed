using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Background Music")]
    [SerializeField] private AudioClip backgroundTheme1;
    [SerializeField] private AudioClip backgroundTheme2;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip cashRegisterSFX;
    [SerializeField] private AudioClip tapePeelSFX;
    [SerializeField] private AudioClip uiOpenSFX, uiCloseSFX;
    [SerializeField] private AudioClip errorSFX;
    [SerializeField] private AudioClip boxRipSFX;
    [SerializeField] private AudioClip[] squeakySFX;

    void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }

    void Start()
    {
        LoadVolumes();
        StartCoroutine(StartBackgroundOst());
    }


    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        audioSource.volume = value;
        if (sfxSource != null) sfxSource.volume = value;
    }

    public void SetMasterVolume(float value)
    {
        AudioListener.volume = value; // affects all audio globally
    }

    private void LoadVolumes()
    {
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.6f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 0.8f));
        SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 1f));
    }


    public void PlayCashRegister() => audioSource.PlayOneShot(cashRegisterSFX);
    public void PlayTapePeel() => audioSource.PlayOneShot(tapePeelSFX);
    public void PlayUIOpen() => audioSource.PlayOneShot(uiOpenSFX);
    public void PlayUIClose() => audioSource.PlayOneShot(uiCloseSFX);
    public void PlayError() => audioSource.PlayOneShot(errorSFX);
    public void PlayBoxRip() => audioSource.PlayOneShot(boxRipSFX);

    public void PlayRandomSqueaky()
    {
        if (squeakySFX.Length == 0) return;
        int randomIndex = UnityEngine.Random.Range(0, squeakySFX.Length);
        audioSource.PlayOneShot(squeakySFX[randomIndex]);
    }


    private IEnumerator StartBackgroundOst()
    {
        AudioClip[] backgroundTracks = { backgroundTheme1, backgroundTheme2 };
        int index = UnityEngine.Random.Range(0, backgroundTracks.Length);

        while (true)
        {
            if (backgroundTracks[index] == null)
            {
                index = (index + 1) % backgroundTracks.Length;
                continue;
            }

            musicSource.clip = backgroundTracks[index];
            musicSource.Play();
            yield return new WaitForSeconds(backgroundTracks[index].length);
            index = (index + 1) % backgroundTracks.Length;
        }
    }
}