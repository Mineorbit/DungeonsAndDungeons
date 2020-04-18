using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public bool Online = true;
    public bool Host;
    public Vector3 targetPosition;
    public  Quaternion targetRotation;
    Transform model;
    void  Start()
    {
        model = transform.Find("char");
    }
    void FixedUpdate()
    {
        if(Host)
        {
            //Send Data
            ClientSend.PlayerLocomotionData(id,transform.position,model.rotation);
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
        if(Online) processOnline(); else processOffline();
    }
    void processOnline()
    {
    if(!Host)
        {
            UpdatePosition();
        }
    }
    void  processOffline()
    {

    }
    void UpdatePosition()
    {
        transform.position = Vector3.Lerp(targetPosition,transform.position,0.5f);
        transform.localRotation = Quaternion.Lerp(targetRotation,model.rotation,0.5f);
    }

}
