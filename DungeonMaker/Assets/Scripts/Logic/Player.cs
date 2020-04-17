using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public bool Host;
    public Vector3 targetPosition;
    public  Quaternion targetRotation;
    void  Start()
    {

    }
    void FixedUpdate()
    {
        if(Host)
        {
            //Send Data
            ClientSend.PlayerLocomotionData(id,transform.position,transform.rotation);
        }
    }
    public void set(Vector3 loc,Quaternion rot)
    {
        targetPosition = loc;
        targetRotation = rot;
    }
    public void ForceSet(Vector3 loc,Quaternion rot)
    {
        transform.position = loc;
        transform.rotation = rot;
        targetPosition = loc;
        targetRotation = rot;
    }
    void Update()
    {
        if(!Host)
        {
            UpdatePosition();
        }
    }

    void UpdatePosition()
    {
        transform.position = Vector3.Lerp(targetPosition,transform.position,0.5f);
        transform.rotation = Quaternion.Lerp(targetRotation,transform.rotation,0.5f);
    }

}
