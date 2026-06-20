using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager Instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

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
        if(Instance != null)
        {
            return;
        }

        Instance = this;
    }

    void Start()
    {
        StartCoroutine(StartBackgroundOst());
    }

    private IEnumerator StartBackgroundOst()
    {
        AudioClip[] backgroundTracks = {backgroundTheme1, backgroundTheme2};
        int index = UnityEngine.Random.Range(0, backgroundTracks.Length);

        while (true)
        {
            audioSource.clip = backgroundTracks[index];
            audioSource.Play();

            yield return new WaitForSeconds(backgroundTracks[index].length);

            index = (index + 1) % backgroundTracks.Length;
        }
    }
}


