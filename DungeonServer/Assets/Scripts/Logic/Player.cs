using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Player : MonoBehaviour
{
    public int localId;

    public int health;
    public string name;

    //Reset after win
    public bool ready;

    Vector3 lastPosition;
    public Vector3 targetPosition;
    public Quaternion targetRotation;


    public Client client;
    public List<int> visitedChunks;


    public bool lockNetUpdate = false;

    float moveDelta = 0.005f;
    void Start()
    {
        
        Setup();
    }
    public void Reset()
    {
        ready = false;
        visitedChunks = new List<int>();
        lastPosition = transform.position;
    }
    public void Setup()
    {
        Reset();
    }
    

    void Update()
    {
        if ((targetPosition - transform.position).magnitude < moveDelta)
        {
            lockNetUpdate = false;
            transform.position = targetPosition;
            transform.rotation = targetRotation;
            var t = GetTarget();
            targetPosition = t.pos;
            targetRotation = t.rot;
        }

        if (!lockNetUpdate)
        { 
        transform.position = Vector3.Lerp(targetPosition,transform.position,0.5f);
        transform.rotation = Quaternion.Lerp(targetRotation,transform.rotation,0.5f);
        }
        else
        {
            transform.position = targetPosition;
            transform.rotation = targetRotation;
        }
        lastPosition = transform.position;
    }
    void UpdatePlay()
    {

    SendVicinity();
	
    if(transform.position.y <= -8)
	{
	Kill();
	}

    }

    public void Kill()
    {
	health = 0;

	PlayerDeathPacket p = new PlayerDeathPacket(localId);
	Server.SendPacketToAll(p);
    PlayerManager.DespawnPlayer(localId);
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


       if((transform.position-lastPosition).magnitude>moveDelta)
       {
       PlayerLocomotionPacket p = new PlayerLocomotionPacket(transform.position, new Quaternion(0, 0, 0, 0), localId);
       //Send info to others
       Server.SendPacketToAllExcept(localId, p);
       }
    }
    (Vector3 pos, Quaternion rot) GetTarget()
    {
        return (targetPosition,targetRotation);
    }

    public void setPositionData(Vector3 loc, Quaternion rot)
    {

        lockNetUpdate = true;
        transform.position = loc;
        transform.rotation = rot;
        targetPosition = loc;
        targetRotation = rot;
    }
    public void updateLocomotionData(Vector3 loc,Quaternion rot)
    {
        if(!lockNetUpdate)
        {
            targetPosition = loc;
            targetRotation = rot;
        }
    }

  
}
