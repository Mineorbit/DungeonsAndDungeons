using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using com.mineorbit.dungeonsanddungeonscommon;

public class PlayerAnimator : MonoBehaviour
{
    PlayerController playerController;
    CharacterController controller;
    public Animator characterAnimator;
    public ParticleSystem[] runDust;
    bool playingDust;

    public PlayerAnimationEventHandler playerAnimationEventHandler;


    void Start()
    {

        playerAnimationEventHandler = transform.GetComponentInChildren<PlayerAnimationEventHandler>();

    
        controller = transform.GetComponent<CharacterController>();
        playerController = transform.GetComponent<PlayerController>();
        characterAnimator = transform.Find("character").GetComponent<Animator>();
        runDust = transform.Find("Particles").Find("Running").GetComponentsInChildren<ParticleSystem>();
        playingDust = true;
        StopDust();
    }

    void Update()
    {
        Vector3 targetDirection = playerController.movingDirection;
        targetDirection.y = 0;
        Vector2 inputVelocity = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));

        float movementSpeed = playerController.currentSpeed;
        characterAnimator.SetFloat("Speed",movementSpeed);
        bool movementKeyPressed = Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.D);

       
        if (movementKeyPressed && playerController.doInput)
        { 
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), 0.2f);
        }



        if (movementSpeed > 0 && playerController.IsGrounded)
        {
            StartDust();
        }
        else
        {
            StopDust();
        }

    }


    public  void Attack()
    {
        playerAnimationEventHandler.Attack();
        characterAnimator.SetTrigger("Attack");
    }
   

    void StartDust()
    {
        if(!playingDust)
        { 
            playingDust = true;
            runDust[0].Play();
            runDust[1].Play();
        }
    }
    void StopDust()
    {
        if(playingDust)
        { 
        playingDust = false;
        runDust[0].Stop();
        runDust[1].Stop();
        }
    }
}
