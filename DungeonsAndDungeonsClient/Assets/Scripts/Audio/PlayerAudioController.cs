using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : AudioController
{
    public CharacterController controller;
    void Start()
    {
        controller = transform.GetComponent<CharacterController>();    
    }
    void Update()
    {
        float speed = controller.velocity.magnitude;
        Blend(0,PlayerAnimator.speed);
        if (controller.isGrounded && speed > 0)
        {
            Play(0);
        }
        else
        {
            Stop(0);
        }
    }
}
