using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AudioProfile", order = 1)]
    class AudioProfile : ScriptableObject
    {
        public bool         overloading;
        public Vector3      relativePosition;
        public float        minVolume = 0.5f;
        public float        maxVolume = 1;
        public float        spatialFactor;
        public bool         loop;
        public AudioClip[]    audioClip;
    }
    

    AudioProfile[] audioProfiles;
    AudioSource[][] audioSources;
    int[] currentPlay;
    void Awake()
    {
        audioSources = new AudioSource[audioProfiles.Length];
        SetupAudioProfiles();
    }

    void SetupAudioProfiles()
    {
        int j = 0;
        currentPlay = new int[audioProfiles.Length];
        foreach(AudioProfile ap in audioProfiles)
        {
            AudioSource[] source = new AudioSource[ap.audioClip.Length];
            int i = 0;
            foreach (AudioClip c in ap.audioClip)
            {
                source[i] = transform.gameObject.AddComponent<AudioSource>();
                source[i].loop = ap.loop;
                source[i].clip = ap.audioClip;
                i++;
            }
            audioSources[j] = source;
            j++;
        }
    }
    public void Blend(int index, float t)
    {
        audioSources[index].volume = (1 - t) * audioProfiles[index].minVolume + t * audioProfiles[index].maxVolume;
    }
    public void Play(int index)
    {
        if(!audioSources[index].isPlaying)
        {
            audioSources[index].Play();
        }
    }
    public void PlayNext(int index)
    {
        int nextPlay = (currentPlay[index] + 1) % (audioSources[index].Length);
        if (!audioSources[index][currentPlay[index]].isPlaying)
        {

            audioSources[index][currentPlay[index]].Stop();
            audioSources[index][nextPlay].Play();
        }

        if (audioSources[index][currentPlay].isPlaying)
        {

            if (audioProfiles[index].overloading)
            {
                audioSources[index][nextPlay].Play();
            }

        }
        currentPlay[index] = nextPlay;
    }

    public void Stop(int index)
    {
        audioSources[index][currentPlay[index]].Stop();
    }
    public void StopAll(int index)
    {
    }
    public void StopAll()
    {
        foreach(AudioSource[] sources in audioSources)
        {
            foreach(AudioSource source in sources)
            source.Stop();
        }
    }

}
