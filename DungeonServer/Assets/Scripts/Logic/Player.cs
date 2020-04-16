using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    void Start()
    {
        
    }
    public void updateData(Vector3 pos,Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }
    void FixedUpdate()
    {
        ServerSend.PlayerLocomotionData(id,transform.position,transform.rotation);
    }
}
