using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
   


    public AudioProfile[] audioProfiles;
    AudioSource[][] audioSources;
    int[] currentPlay;
    void Awake()
    {
        audioSources = new AudioSource[audioProfiles.Length][];
        SetupAudioProfiles();
    }

    void SetupAudioProfiles()
    {
        int j = 0;
        currentPlay = new int[audioProfiles.Length];
        foreach(AudioProfile ap in audioProfiles)
        {
            audioSources[j] = new AudioSource[ap.audioClip.Length];
            int i = 0;
            foreach (AudioClip c in ap.audioClip)
            {
                audioSources[j][i] = transform.gameObject.AddComponent<AudioSource>();
                audioSources[j][i].loop = ap.loop;
                audioSources[j][i].mute = true;
                audioSources[j][i].volume = ap.VolumeCoefficient();
                audioSources[j][i].spatialBlend = ap.spatialFactor;
                audioSources[j][i].playOnAwake = ap.onAwake;
                audioSources[j][i].clip = ap.audioClip[i];
                i++;
            }
            j++;
        }
    }
    public void Blend(int index, float t)
    {
        audioSources[index][currentPlay[index]].volume = audioProfiles[index].VolumeCoefficient()*((1 - t) * audioProfiles[index].minVolume + t * audioProfiles[index].maxVolume);
    }

    IEnumerator CrossFader(int indexA, int indexB, float time)
    {
        float t = 0;

        Blend(indexA, 1);
        Play(indexB);
        Blend(indexB, 0);
        while (t<time)
        {
            t += Time.deltaTime;
            float fraction = t / time;

            Blend(indexA, 1 - fraction);
            Blend(indexB, fraction);

            yield return 0;

        }

        Blend(indexA, 0);
        Blend(indexB, 1);
        if(indexA!=indexB)
        Stop(indexA);
    }

    public void CrossFade(int indexA, int indexB, float timeForFade)
    {
        IEnumerator fader = CrossFader(indexA,indexB,timeForFade);
        StartCoroutine(fader);
    }


    void prePlay(int index)
    {
        //Prüfen ob schon läuft
        audioSources[index][currentPlay[index]].mute = false;
    }
    public void Play(int index)
    {
        prePlay(index);
        if (!audioSources[index][currentPlay[index]].isPlaying)
        {
            audioSources[index][currentPlay[index]].Play();
        }
    }
    public void PlayNext(int index)
    {
        prePlay(index);
        int nextPlay = (currentPlay[index] + 1) % (audioSources[index].Length);
        bool clipFinished = audioSources[index][currentPlay[index]].time >= audioSources[index][currentPlay[index]].clip.length;
        if (!audioSources[index][currentPlay[index]].isPlaying && (clipFinished) )
        {

            audioSources[index][currentPlay[index]].Stop();
            audioSources[index][nextPlay].Play();
        }

        if (audioSources[index][currentPlay[index]].isPlaying && (clipFinished) )
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
        /*
        if (audioProfiles[index].loop)
        {
            audioProfiles[index].loop = false;
        }
        else
        {
        }
        */
    }
    public void StopAll(int index)
    {
        foreach (AudioSource source in audioSources[index])
            source.Stop(); 
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
