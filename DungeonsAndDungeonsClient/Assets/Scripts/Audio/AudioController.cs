using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AudioProfile", order = 1)]
    class AudioProfile : ScriptableObject
    {
        public Vector3 relativePosition;
        public float minVolume = 0.5f;
        public float maxVolume = 1;
        public float spatialFactor;
        public bool loop;
        public AudioClip audioClip;
    }

    AudioProfile[] audioProfiles;
    AudioSource[] audioSources;
    void Awake()
    {
        audioSources = new AudioSource[audioProfiles.Length];
        SetupAudioProfiles();
    }

    void SetupAudioProfiles()
    {
        foreach(AudioProfile ap in audioProfiles)
        {
            AudioSource source = transform.gameObject.AddComponent<AudioSource>();
            source.loop = ap.loop;
            source.clip = ap.audioClip;
        }
    }
    public void Blend(int index, float t)
    {
        audioSources[index].volume = (1 - t) * audioProfiles[index].minVolume + t * audioProfiles[index].maxVolume;
    }
    public void Play(int index)
    {
        audioSources[index].Play();
    }

    public void Stop(int index)
    {
        audioSources[index].Stop();
    }
    public void StopAll()
    {
        foreach(AudioSource source in audioSources)
        {
            source.Stop();
        }
    }

}
