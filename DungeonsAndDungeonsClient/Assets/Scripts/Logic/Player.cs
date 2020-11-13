using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public long globalId;
    public int localId;
    public string name;
    public enum Color {Blue, Red, Green, Yellow};
    public Color playerColor;

    public bool isMe;

    bool netInput;
    public Vector3 targetPosition;
    public Quaternion targetRotation;

    Vector3 lastPosition;

    public void Start()
    {
        gameObject.name = "Player" + localId;

        targetPosition = transform.position;

        
        lastPosition = transform.position;

    }

    public void setTargetLocomotionData(Vector3 targetPos, Quaternion targetRot)
    {
        targetPosition = targetPos;
        targetRotation = targetRot;

        netInput = true;
    }

    
    public void setPositionData(Vector3 loc, Quaternion rot)
    {
        Debug.Log($"Player {localId} Setting Position: {loc}");

        transform.position = loc;
        transform.rotation = rot;
        lastPosition = loc;
        setTargetLocomotionData(loc, rot);
    }

    public void Update()
    {
        isMe = localId == NetworkManager.instance.localId;


        if(!isMe&&netInput&&(transform.position - targetPosition).magnitude>1)
        { 
        transform.position = (transform.position + targetPosition) / 2;
        }

       
        if(isMe)
        {
            if ((lastPosition-transform.position).magnitude > 0.0005f)
            {
                PlayerLocomotionPacket p = new PlayerLocomotionPacket(transform.position,transform.rotation, localId);
                NetworkManager.instance.SendLocomotionData(p);
            }
        }
        lastPosition = transform.position;
    }
}
