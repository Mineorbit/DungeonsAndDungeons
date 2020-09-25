using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AudioProfile", order = 1)]
public class AudioProfile : ScriptableObject
{
    public bool overloading;
    public bool onAwake;
    public Vector3 relativePosition;
    public float minVolume = 0.5f;
    public float maxVolume = 1;
    public float spatialFactor;
    public bool loop;
    public AudioClip[] audioClip;
}
