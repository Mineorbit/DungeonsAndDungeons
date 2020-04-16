using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public bool Host;
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
    public void updateLocomotionData(Vector3 loc,Quaternion rot)
    {
        transform.position = loc;
        transform.rotation = rot;
    }
}
