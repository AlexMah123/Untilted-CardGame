using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Header("Sound FX Prefab")]
    [SerializeField] private AudioSource soundFXObject;

    [Header("SFX Library")]
    public SoundLibrary sfxLibrary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform)
    {
        if (audioClip == null)
        {
            throw new NullReferenceException($"{audioClip.name} is null");
        }

        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlaySoundFXClip(string soundName, Transform spawnTransform)
    {
        var audioClip = sfxLibrary.GetAudioClip(soundName);

        PlaySoundFXClip(audioClip, spawnTransform);
    }
}
