using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class ChunkDataPacket : Packet
{
    public ChunkDataPacket(int x, int y,Chunk.ChunkData chunkData)
    {
        packetId = 7;
        types = new Type[3];
        types[0] = typeof(int);
        types[1] = typeof(int);
        types[2] = typeof(Chunk.ChunkData);
        content = new object[3];
        content[0] = x;
        content[1] = y;
        content[2] = chunkData;
    }
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
}
