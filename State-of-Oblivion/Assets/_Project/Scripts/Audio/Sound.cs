using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound
{

    public string name;
    [SerializeField] internal AudioClip audioClip;
    [Range(0f, 1f)]
    [SerializeField] internal float volume;
    [Range(0.1f, 3f)]
    [SerializeField] internal float pitch;
    [SerializeField] internal bool loop;

    internal AudioSource source;
}
