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
    List<int> visitedChunks;
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
        if(Level.currentLevel!=null)
        {
            if(Level.currentLevel.isLoaded)
            { 
            Tuple<int, int> chunkLocation = Level.currentLevel.GetChunkLocation(transform.position);
            int saveID = Level.currentLevel.GetSaveID(chunkLocation);
            if(!visitedChunks.Contains(saveID))
            {
            //Send chunk
            Chunk c = Level.currentLevel.GetChunk(transform.position);
                    if(c != null)
                    { 
                        Chunk.ChunkData d = c.GetChunkData(saveID);
                        ChunkDataPacket packet = new ChunkDataPacket(chunkLocation.Item1,chunkLocation.Item2,d);
                        Server.SendPacket(localId,packet);
                        visitedChunks.Add(saveID);
                    }
                }
            }
        }
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
    public void updateLocomotionData(Vector3 loc,Quaternion rot)
    {
        Debug.Log("Changing target "+localId);
        targetPosition = loc;
        targetRotation = rot;
    }
}
