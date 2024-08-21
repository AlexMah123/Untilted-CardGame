using System;
using System.Linq;
using UnityEngine;

[Serializable]
public struct SoundEffect
{
    public string groupID;
    public AudioClip[] clips;
}

public class SoundLibrary : MonoBehaviour
{
    public SoundEffect[] soundEffects;

    public AudioClip GetAudioClip(string name)
    {
        var audioClip = soundEffects.FirstOrDefault(sfx => sfx.groupID == name);

        if (audioClip.Equals(default(SoundEffect)))
        {
            throw new NullReferenceException($"{name} clip was not found");
        }

        return audioClip.clips[UnityEngine.Random.Range(0, audioClip.clips.Length)];
    }
}
