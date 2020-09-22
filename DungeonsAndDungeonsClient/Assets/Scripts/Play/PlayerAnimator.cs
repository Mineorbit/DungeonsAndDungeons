using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    CharacterController controller;
    Animator characterAnimator;
    void Start()
    {
        controller = transform.GetComponent<CharacterController>();
        characterAnimator = transform.Find("character").GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 targetDirection = PlayerController.movingDirection;
        targetDirection.y = 0;
        Vector2 inputVelocity = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        characterAnimator.SetFloat("Speed",Mathf.Min(inputVelocity.magnitude,1));
        bool movementKeyPressed = Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.D); 
        
        if(movementKeyPressed)
        { 
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), 0.05f);
        }

    }
}
