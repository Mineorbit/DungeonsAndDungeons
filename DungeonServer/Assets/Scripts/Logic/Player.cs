using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Player : MonoBehaviour
{
    public int localId;
    public string name;

    //Reset after win
    public bool ready;

    public Vector3 targetPosition;
    public Quaternion targetRotation;


    public Client client;
    public List<int> visitedChunks;

    bool netUpdate = true;

    bool Interpolate = true;
    void Start()
    {
        Setup();
    }
    public void Reset()
    {
        ready = false;
        visitedChunks = new List<int>();
    }
    public void Setup()
    {
        Reset();
    }
    

    void Update()
    {
        if(Interpolate = true)
        { 
        transform.position = Vector3.Lerp(targetPosition,transform.position,0.5f);
        transform.rotation = Quaternion.Lerp(targetRotation,transform.rotation,0.5f);
        }
        else
        {
            transform.position = targetPosition;
            transform.rotation = targetRotation;
        }
    }
    void UpdatePlay()
    {
        SendVicinity();
    }
    public void SendLevelList()
    {
        foreach (LevelData.LevelMetaData levelData in LevelManager.levelManager.availableLocalLevels)
        {
            Debug.Log("Sending " + levelData.name);
            LevelListPacket p = new LevelListPacket(levelData);
            Server.SendPacket(localId, p);
        }
    }
    void SendVicinity()
    {
        for(int i = -1; i<=1;i++)
            for(int j = -1; j<=1;j++)
                Level.currentLevel.SendChunkAt(transform.position+ new Vector3(32*i,0,32*j), localId);
    }
    void FixedUpdate()
    {
        if(ServerManager.instance.GetState()==ServerManager.State.Play)
        {
            UpdatePlay();
        }


        if((targetPosition-transform.position).magnitude>0.05)
        {
            Interpolate = false;
            if(netUpdate)
            {

            PlayerLocomotionPacket p = new PlayerLocomotionPacket(transform.position, new Quaternion(0, 0, 0, 0), localId);
            //Send info to others
            Server.SendPacketToAll(p);
            }
        }else
        {
            Interpolate = true;
        }

    }
    public void setPositionData(Vector3 loc, Quaternion rot)
    {
        Interpolate = false;
        transform.position = loc;
        transform.rotation = rot;
        targetPosition = loc;
        targetRotation = rot;
    }
    public void updateLocomotionData(Vector3 loc,Quaternion rot)
    {
        Interpolate = true;
        targetPosition = loc;
        targetRotation = rot;
    }
}
