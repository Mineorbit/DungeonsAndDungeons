using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : AudioController
{
    public PlayerController controller;
    float walkSoundStrength;
    void Start()
    {
        controller = transform.GetComponent<PlayerController>();    
    }
    void Update()
    {
        walkSoundStrength = (walkSoundStrength + controller.currentSpeed) / 2;
        Blend(0, walkSoundStrength);
        if (controller.IsGrounded && controller.currentSpeed > 0)
        {
            Play(0);
        }
        else
        {
            Stop(0);
        }
    }
}
