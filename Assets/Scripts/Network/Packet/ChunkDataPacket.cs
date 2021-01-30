using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class ChunkDataPacket : Packet
{
    public ChunkDataPacket()
    {
        packetId = 7;
        types = new Type[3];
        types[0] = typeof(int);
        types[1] = typeof(int);
        types[2] = typeof(Chunk.ChunkData);
        content = new object[3];
        content[0] = 0;
        content[1] = 0;
        content[2] = null;
    }
    public override void OnReceive()
    {
        Tuple<int, int> chunkLocation = new Tuple<int, int>((int) content[0], (int) content[1]);
        Chunk.ChunkData chunkData = (Chunk.ChunkData)content[2];
        Level.currentLevel.FromChunkData(chunkData,chunkLocation);
        if(Level.currentLevel.spawn[NetworkManager.instance.localId] != null)
        {
            if(GameManager.instance.currentLogic.GetType() == typeof(PlayLogic))
            { 
            PlayLogic.SpawnChunkReceived = true;
            }
        }
    }
}
