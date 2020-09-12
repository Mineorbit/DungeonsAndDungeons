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
        Vector3 targetDirection = new Vector3(controller.velocity.x,0, controller.velocity.z).normalized;
        Vector2 inputVelocity = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        characterAnimator.SetFloat("Speed",Mathf.Min(inputVelocity.magnitude,1));
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), 0.125f);
        
    }
}
