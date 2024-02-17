using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new AudioClipSetting", menuName = "Audio Clip Setting")]
public class AudioClipSetting : ScriptableObject
{
    public AudioClip Clip;
    [Range(0f, 1f)]
    public float Volume;

    public AudioClipSetting(float volume)
    {
        this.Volume = volume;
    }
}
