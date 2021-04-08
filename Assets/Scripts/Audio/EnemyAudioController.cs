using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.mineorbit.dungeonsanddungeonscommon;

public class EnemyAudioController : AudioController
{
    public EnemyController controller;
    float walkSoundStrength;
    void Start()
    {
    }
    void Update()
    {
        if(controller  != null)
        {
            walkSoundStrength = (controller.currentSpeed + walkSoundStrength) / 2;
        Blend(0, walkSoundStrength);
        if (walkSoundStrength > 0)
        {
            Play(0);
        }
        else
        {
            Stop(0);
        }
        }
        else
        {
            controller = transform.GetComponent<EnemyController>();
        }
    }
}
