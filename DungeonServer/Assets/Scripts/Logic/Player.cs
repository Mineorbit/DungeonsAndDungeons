using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Player : MonoBehaviour
{
    public int localId;
    public string name;

    public bool ready;

    Vector3 targetPosition;
    Quaternion targetRotation;


    public Client client;
    public List<int> visitedChunks;
    void Start()
    {
        Setup();
    }
    public void Setup()
    {
        ready = false;
        visitedChunks = new List<int>();
        PlayerManager.playerManager.AddPlayer(this,localId);
    }
    void Update()
    {
        transform.position = Vector3.Lerp(targetPosition,transform.position,0.5f);
        transform.rotation = Quaternion.Lerp(targetRotation,transform.rotation,0.5f);
    }
    void UpdatePlay()
    {
        Level.currentLevel.SendChunkAt(transform.position,localId);
    }
    void FixedUpdate()
    {
        if(ServerManager.instance.GetState()==ServerManager.State.Play)
        {
            UpdatePlay();
        }
        if(targetPosition!=transform.position)
        {
            //Update Player Locally
            PlayerLocomotionPacket p = new PlayerLocomotionPacket(transform.position, new Quaternion(0, 0, 0, 0), localId);
            //Send info to others
            Server.SendPacketToAllExcept(localId, p);
        }

    }
    public void setPositionData(Vector3 loc, Quaternion rot)
    {
        transform.position = loc;
        transform.rotation = rot;
        updateLocomotionData(loc,rot);
    }
    public void updateLocomotionData(Vector3 loc,Quaternion rot)
    {
        targetPosition = loc;
        targetRotation = rot;
    }
}
