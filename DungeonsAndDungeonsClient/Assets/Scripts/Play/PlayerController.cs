using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool acceptInput = true;
    CharacterController controller;
    public Transform cam;
    float convergenceSpeed = 0.1f;
    float turnSmoothVel;
    public float Speed = 1f;
    public Vector3 targetDirection;
    public static Vector3 movingDirection;
    float speedY = 0;
    float gravity = 1f;
    void Start()
    {
        controller = transform.GetComponent<CharacterController>();
    }

    void Update()
    {
       if(acceptInput)
        {
            Move();
        }
    }
    void Move()
    {
        targetDirection =
                    Vector3.Normalize(Vector3.ProjectOnPlane(cam.right, transform.up) * Input.GetAxisRaw("Horizontal") + Vector3.ProjectOnPlane(cam.forward, transform.up) * Input.GetAxisRaw("Vertical"));


        if (!controller.isGrounded)
        {
            speedY -= gravity * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            speedY = 1;
        }
        targetDirection.y = speedY;
        if (targetDirection.sqrMagnitude >= 0.01f)
        {
            movingDirection = targetDirection;
            controller.Move(targetDirection * Speed * Time.deltaTime);
        }
    }
}
