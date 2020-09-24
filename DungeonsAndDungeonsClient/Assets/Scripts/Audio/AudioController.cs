using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public int audioSources;
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AudioProfile", order = 1)]
    class AudioProfile : ScriptableObject
    {
        public Vector3 relativePosition;
        public float minVolume = 0.5f;
        public float maxVolume = 1;
        public AudioClip audioClip;
    }

    AudioProfile[] audioProfiles;
    void Awake()
    {
        
    }

    void SetupAudioProfiles()
    {

    }
    
}
