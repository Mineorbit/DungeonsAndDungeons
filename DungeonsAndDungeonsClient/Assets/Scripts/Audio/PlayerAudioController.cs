using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : AudioController
{
    public PlayerController controller;
    void Start()
    {
        controller = transform.GetComponent<PlayerController>();    
    }
    void Update()
    {
        Blend(0, controller.currentSpeed);
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
