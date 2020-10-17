using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static PlayerController player;

    public static bool acceptInput = true;
    CharacterController controller;
    public Transform cam;
    float convergenceSpeed = 0.1f;
    float turnSmoothVel;
    public float Speed = 1f;
    public Vector3 targetDirection;
    public static Vector3 movingDirection;
    float speedY = 0;
    float gravity = 4f;
    public void Awake()
    {
        if (player != null) Destroy(this);
        player = this;
    }
    //Setup References for PlayerController and initial values if necessary
    public static void Setup()
    {
        if(Camera.main!=null)
        player.cam = Camera.main.transform;

        player.controller = player.transform.GetComponent<CharacterController>();

    }
    public static void Spawn(Vector3 location)
    {
        if (player == null) return;
        player.transform.position = location;
        player.gameObject.SetActive(true);
    }
    public static void Despawn()
    {
        if (player == null) return;
        player.gameObject.SetActive(false);
    }
    void Update()
    {
    Move();
    }
    void Move()
    {
        if (acceptInput)
        {
            if(cam!=null)
        targetDirection = Vector3.Normalize(Vector3.ProjectOnPlane(cam.right, transform.up) * Input.GetAxisRaw("Horizontal") + Vector3.ProjectOnPlane(cam.forward, transform.up) * Input.GetAxisRaw("Vertical"));
        }

        if (!controller.isGrounded)
        {
            speedY -= gravity * Time.deltaTime;
        }
        if (controller.isGrounded && acceptInput && Input.GetKeyDown(KeyCode.Space))
        {
                speedY = 1.5f;
        }
        targetDirection.y = speedY;
        if (targetDirection.sqrMagnitude >= 0.01f)
        {
            movingDirection = targetDirection;
            controller.Move(targetDirection * Speed * Time.deltaTime);
        }
    }
    public void OnDisable()
    {
        player = null;
    }
}
