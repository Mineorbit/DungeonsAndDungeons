using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class PlayerSpawnPacket : Packet
{
    public PlayerSpawnPacket()
    {
        packetId = 8;
        types = new Type[5];
        types[0] = typeof(int);
        types[1] = typeof(float);
        types[2] = typeof(float);
        types[3] = typeof(float);
        types[4] = typeof(bool);
        content = new object[5];

    }
    public PlayerSpawnPacket(int localId, Vector3 position,bool doInput)
    {
        packetId = 8;
        types = new Type[5];
        types[0] = typeof(int);
        types[1] = typeof(float);
        types[2] = typeof(float);
        types[3] = typeof(float);
        types[4] = typeof(bool);
        content = new object[5];
        content[0] = localId;
        content[1] = position.x;
        content[2] = position.y;
        content[3] = position.z;
        content[4] = doInput;
    }
    public void OnReceive()
    {

    }
}
