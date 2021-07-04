using System.Collections;
using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class AudioController : MonoBehaviour
    {
        public AudioProfile[] audioProfiles;
        private AudioSource[][] audioSources;
        private int[] currentPlay;

        private void Awake()
        {
            audioSources = new AudioSource[audioProfiles.Length][];
            SetupAudioProfiles();
        }

        private void SetupAudioProfiles()
        {
            var j = 0;
            currentPlay = new int[audioProfiles.Length];
            foreach (var ap in audioProfiles)
            {
                audioSources[j] = new AudioSource[ap.audioClip.Length];
                var i = 0;
                foreach (var c in ap.audioClip)
                {
                    audioSources[j][i] = transform.gameObject.AddComponent<AudioSource>();
                    audioSources[j][i].loop = ap.loop;
                    audioSources[j][i].mute = true;
                    audioSources[j][i].volume = ap.VolumeCoefficient() * ap.minVolume;
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
            audioSources[index][currentPlay[index]].volume = audioProfiles[index].VolumeCoefficient() *
                                                             ((1 - t) * audioProfiles[index].minVolume +
                                                              t * audioProfiles[index].maxVolume);
        }

        private IEnumerator CrossFader(int indexA, int indexB, float time)
        {
            float t = 0;

            Blend(indexA, 1);
            Play(indexB);
            Blend(indexB, 0);
            while (t < time)
            {
                t += Time.deltaTime;
                var fraction = t / time;

                Blend(indexA, 1 - fraction);
                Blend(indexB, fraction);

                yield return 0;
            }

            Blend(indexA, 0);
            Blend(indexB, 1);
            if (indexA != indexB)
                Stop(indexA);
        }

        public void CrossFade(int indexA, int indexB, float timeForFade)
        {
            var fader = CrossFader(indexA, indexB, timeForFade);
            StartCoroutine(fader);
        }


        private void prePlay(int index)
        {
            //Prüfen ob schon läuft
            audioSources[index][currentPlay[index]].mute = false;
        }

        public void Play(int index)
        {
            prePlay(index);
            if (!audioSources[index][currentPlay[index]].isPlaying) audioSources[index][currentPlay[index]].Play();
        }

        public void PlayNext(int index)
        {
            prePlay(index);
            var nextPlay = (currentPlay[index] + 1) % audioSources[index].Length;
            var clipFinished = audioSources[index][currentPlay[index]].time >=
                               audioSources[index][currentPlay[index]].clip.length;
            if (!audioSources[index][currentPlay[index]].isPlaying && clipFinished)
            {
                audioSources[index][currentPlay[index]].Stop();
                audioSources[index][nextPlay].Play();
            }

            if (audioSources[index][currentPlay[index]].isPlaying && clipFinished)
                if (audioProfiles[index].overloading)
                    audioSources[index][nextPlay].Play();
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
            foreach (var source in audioSources[index])
                source.Stop();
        }

        public void StopAll()
        {
            foreach (var sources in audioSources)
            foreach (var source in sources)
                source.Stop();
        }
    }
}