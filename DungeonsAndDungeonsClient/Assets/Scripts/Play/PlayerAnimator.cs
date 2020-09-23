using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    CharacterController controller;
    Animator characterAnimator;
    public ParticleSystem[] runDust;
    bool playingDust;

    void Start()
    {
        controller = transform.GetComponent<CharacterController>();
        characterAnimator = transform.Find("character").GetComponent<Animator>();
        runDust = transform.Find("Particles").Find("Running").GetComponentsInChildren<ParticleSystem>();
        StopDust();
    }

    void Update()
    {
        Vector3 targetDirection = PlayerController.movingDirection;
        targetDirection.y = 0;
        Vector2 inputVelocity = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        characterAnimator.SetFloat("Speed",Mathf.Min(inputVelocity.magnitude,1));
        bool movementKeyPressed = Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.D);

        if (controller.velocity.magnitude >= 0.5f && controller.isGrounded && !playingDust)
        {
        StartDust();
        }
        if ((controller.velocity.magnitude <= 0.5f || !controller.isGrounded) && playingDust)
        {
        StopDust();
        }
        if (movementKeyPressed)
        { 
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), 0.05f);
        }

    }

    void StartDust()
    {
        
            playingDust = true;
            runDust[0].Play();
            runDust[1].Play();
        
    }
    void StopDust()
    {

        playingDust = false;
        runDust[0].Stop();
        runDust[1].Stop();
    }
}
