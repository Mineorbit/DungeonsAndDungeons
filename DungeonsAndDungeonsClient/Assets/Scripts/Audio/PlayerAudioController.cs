using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : AudioController
{
    public CharacterController controller;
    Vector2 moveVel;
    void Start()
    {
        controller = transform.GetComponent<CharacterController>();    
    }
    void Update()
    {
        moveVel = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        float speed = moveVel.magnitude;
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
