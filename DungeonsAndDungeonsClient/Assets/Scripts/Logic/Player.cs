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


    bool alive = true;
    int health = 100;


    public void Start()
    {
        if(GameManager.GetState() == GameManager.State.Play)
        {
            SetupPlay();
        }

        targetPosition = transform.position;

        
        lastPosition = transform.position;

    }

    void SetupPlay()
    {
    gameObject.name = "Player" + localId;
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

    public void  Spawn(Vector3 location,Quaternion rotation, bool allowedToMove)
    {
        health = 100;
        alive = true;


        PlayerManager.playerManager.SpawnPlayer(localId, location);

        setPositionData(location, rotation);

        setMovementStatus(allowedToMove);

        if (localId == NetworkManager.instance.localId)
        {
            PlayLogic.SpawnPositionSet = true;
        }
    }

    public void setMovementStatus(bool allowedToMove)
    {
        PlayerManager.playerManager.playerControllers[localId].allowedToMove = allowedToMove;
    }

    public void Kill()
    {
        health = 0;
        alive = false;
        PlayerManager.playerManager.DespawnPlayer(localId);
        //HIER STATTDESSEN GAME LOGIC CALLEN für übergreifenden  effekt
        if (NetworkManager.instance.localId == localId)
        { DeathScreen.instance.Open(); }
    }


    public void Update()
    {
        if(GameManager.GetState() == GameManager.State.Play)
        {
            PlayUpdate();
        }
    }
}
