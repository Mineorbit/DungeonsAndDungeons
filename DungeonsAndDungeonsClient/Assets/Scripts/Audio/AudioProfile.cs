using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AudioProfile", order = 1)]
public class AudioProfile : ScriptableObject
{
    public enum AudioType { SFX, MUSIC, UI};
    public AudioType audioType;
    public bool overloading;
    public bool onAwake;
    public Vector3 relativePosition;
    
    
    public float minVolume = 0.5f;

    public float maxVolume = 1f;

    public float spatialFactor;
    public bool loop;
    public AudioClip[] audioClip;

    public float VolumeCoefficient()
    {
        if(audioType == AudioType.SFX)
        {
            return SFXVolumeHandler.volume;
        }
        if (audioType == AudioType.MUSIC)
        {
            return MusicVolumeHandler.volume;
        }
        if (audioType == AudioType.UI)
        {
            return UIVolumeHandler.volume;
        }

        return 1;
    }
}
