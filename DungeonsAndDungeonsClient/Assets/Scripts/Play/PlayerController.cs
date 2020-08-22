using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    public Transform cam;
    float convergenceSpeed = 0.1f;
    float turnSmoothVel;
    public float Speed = 1f;
    public Vector3 targetDirection;
    void Start()
    {
        controller = transform.GetComponent<CharacterController>();
    }

    void Update()
    {
        targetDirection = 
            Vector3.Normalize(Vector3.ProjectOnPlane( cam.right,transform.up) * Input.GetAxisRaw("Horizontal")+ Vector3.ProjectOnPlane(cam.forward, transform.up) * Input.GetAxisRaw("Vertical"));

        if(targetDirection.sqrMagnitude >= 0.1f)
        {
            controller.Move(targetDirection*Speed*Time.deltaTime);    
        }
    }
}
