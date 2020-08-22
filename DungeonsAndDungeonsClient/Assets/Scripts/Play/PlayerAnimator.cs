using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    CharacterController controller;
    void Start()
    {
        controller = transform.GetComponent<CharacterController>();    
    }

    void Update()
    {
        Vector3 targetDirection = new Vector3(controller.velocity.x,0, controller.velocity.z).normalized;
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(targetDirection),0.125f);
    }
}
