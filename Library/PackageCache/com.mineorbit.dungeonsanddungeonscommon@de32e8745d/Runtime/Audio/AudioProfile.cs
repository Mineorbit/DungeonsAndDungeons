using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AudioProfile", order = 1)]
    public class AudioProfile : ScriptableObject
    {
        public enum AudioType
        {
            SFX,
            MUSIC,
            UI
        }


        public static float SFXVolume = 1;

        public static float MusicVolume = 1;

        public static float UiVolume = 1;
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
            if (audioType == AudioType.SFX) return SFXVolume;
            if (audioType == AudioType.MUSIC) return MusicVolume;
            if (audioType == AudioType.UI) return UiVolume;

            return 1;
        }
    }
}