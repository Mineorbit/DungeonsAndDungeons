using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public static PlayerController currentPlayer;
    Player player;
    public CharacterController controller;
    public Transform cam;
    float convergenceSpeed = 0.1f;
    float turnSmoothVel;
    public float Speed = 3f;
    public Vector3 targetDirection;
    public Vector3 movingDirection;
    float speedY = 0;
    float gravity = 4f;

    public float currentSpeed;

    public bool activated;
    public bool IsGrounded;
    public bool doInput;
    public bool isMe;
    public static bool doSim;
    public bool allowedToMove = true;
    //Setup References for PlayerController and initial values if necessary
    public void Awake()
    {
        if(Camera.main!=null)
        cam = Camera.main.transform;

        controller = transform.GetComponent<CharacterController>();
    }

    void Start()
    {
        player = transform.GetComponent<Player>();
    }

    public void UpdateGround()
    {
        int mask = 1 << 10;
        RaycastHit hit;
        IsGrounded = controller.isGrounded || Physics.Raycast(transform.position,-Vector3.up,out hit, 1.5f,mask);
    }

    public void Move()
    {

        if (!controller.isGrounded && doSim)
        {
            speedY -= gravity * Time.deltaTime;
        }
        if(controller.isGrounded || !doSim)
        {
            speedY = 0;
        }
        targetDirection = new Vector3(0,0,0);
        
        if (doInput)
        {
            if (cam != null)
                targetDirection = Vector3.Normalize(Vector3.ProjectOnPlane(cam.right, transform.up) * Input.GetAxisRaw("Horizontal") + Vector3.ProjectOnPlane(cam.forward, transform.up) * Input.GetAxisRaw("Vertical"));

            

            
            if (IsGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                speedY = 1.5f;
            }
        }

        targetDirection.y = speedY;
        if (targetDirection.sqrMagnitude >= 0.01f)
        {
            movingDirection = targetDirection;
        }else
        {
            movingDirection = new Vector3(0, 0, 0);
        }
        controller.Move(targetDirection * Speed * Time.deltaTime);
    }

    public void OnDisable()
    {
    //    if (currentPlayer == this) currentPlayer = null;
    }
}
