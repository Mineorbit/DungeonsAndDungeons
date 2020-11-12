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
        PlayerManager.playerManager.AddPlayer(this,localId);
        SendLevelList();
        Reset();
    }
    void SendLevelList()
    {
        foreach(LevelData.LevelMetaData levelData in LevelManager.levelManager.availableLocalLevels)
        {
            LevelListPacket p = new LevelListPacket(levelData);
            client.Send(p);
        }
    }

    void Update()
    {
        transform.position = Vector3.Lerp(targetPosition,transform.position,0.5f);
        transform.rotation = Quaternion.Lerp(targetRotation,transform.rotation,0.5f);
    }
    void UpdatePlay()
    {
        SendVicinity();
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
        if(targetPosition!=transform.position)
        {
            //Update Player Locally
            PlayerLocomotionPacket p = new PlayerLocomotionPacket(transform.position, new Quaternion(0, 0, 0, 0), localId);
            //Send info to others
            Server.SendPacketToAll(p);
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
