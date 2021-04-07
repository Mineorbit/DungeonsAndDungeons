using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Must be in state Play (online play)
public class NetPlayer : Player
{
    public long globalId;

    public bool isMe;
    public bool lockNetUpdate;

    public Vector3 targetPosition;
    public Quaternion targetRotation;

    Vector3 lastPosition;

    float moveDelta = 0.005f;

    public void Start()
    {
        SetupPlay();
        targetPosition = transform.position;
        lastPosition = transform.position;
    }

    void SetupPlay()
    {
        gameObject.name = "Player" + localId;
    }

    public void setTargetLocomotionData(Vector3 targetPos, Quaternion targetRot)
    {
        if (!lockNetUpdate)
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

    void Update()
    {
        isMe = localId == NetworkManager.instance.localId;
        if ((targetPosition - transform.position).magnitude < moveDelta)
        {
            lockNetUpdate = false;
            transform.position = targetPosition;
            transform.rotation = targetRotation;
        }

        if (!isMe)
        {
            transform.position = (transform.position + targetPosition) / 2;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.5f);
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

    public override void Spawn(Vector3 location, Quaternion rotation, bool allowedToMove)
    {
        base.Spawn(location,rotation,allowedToMove);
        setPositionData(location, rotation);
        setMovementStatus(allowedToMove);
        if (localId == NetworkManager.instance.localId)
        {
            PlayLogic.SpawnPositionSet = true;
        }
    }

    public override void Kill()
    {
        base.Kill();
        if (NetworkManager.instance.localId == localId)
        { DeathScreen.instance.Open(); }
    }
}
