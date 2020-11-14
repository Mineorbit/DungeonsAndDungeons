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
    public bool lockNetUpdate;

    public Vector3 targetPosition;
    public Quaternion targetRotation;

    Vector3 lastPosition;

    float moveDelta = 0.005f;



    public void Start()
    {
        gameObject.name = "Player" + localId;


        targetPosition = transform.position;

        
        lastPosition = transform.position;

    }

    public void setTargetLocomotionData(Vector3 targetPos, Quaternion targetRot)
    {
        if(!lockNetUpdate)
        {
        targetPosition = targetPos;
        targetRotation = targetRot;
        }
    }

    
    public void setPositionData(Vector3 loc, Quaternion rot)
    {
       
        Debug.Log($"Player {localId} Setting Position: {loc}");

        lockNetUpdate = true;
        transform.position = loc;
        transform.rotation = rot;
        targetPosition = loc;
        targetRotation = rot;

    }

    void PlayUpdate()
    {
        isMe = localId == NetworkManager.instance.localId;


        if ((targetPosition - transform.position).magnitude < moveDelta)
        {
            lockNetUpdate = false;
            transform.position = targetPosition;
        }

        if (!isMe)
        {
            transform.position = (transform.position + targetPosition) / 2;
        }


        if (isMe)
        {
            if ((lastPosition - transform.position).magnitude > moveDelta)
            {
                if (!lockNetUpdate)
                {
                    PlayerLocomotionPacket p = new PlayerLocomotionPacket(transform.position, transform.rotation, localId);
                    NetworkManager.instance.SendLocomotionData(p);
                }
            }
        }
        lastPosition = transform.position;
    }
    
    public void Update()
    {
        if(GameManager.GetState() == GameManager.State.Play)
        {
            PlayUpdate();
        }
    }
}
