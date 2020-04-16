using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    void Start()
    {
        
    }
   
    void FixedUpdate()
    {
        ServerSend.PlayerLocomotionData(id,transform.position,transform.rotation);
    }
    public void updateLocomotionData(Vector3 loc,Quaternion rot)
    {
        transform.position = loc;
        transform.rotation = rot;
    }
}
