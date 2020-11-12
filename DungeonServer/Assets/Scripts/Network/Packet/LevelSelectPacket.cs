using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class LevelSelectPacket : Packet
{
    public LevelSelectPacket()
    {
        packetId = 11;
        types = new Type[1];
        types[0] = typeof(long);
        content = new object[1];
    }
    public LevelSelectPacket(LevelData.LevelMetaData levelMetaData)
    {
        packetId = 11;
        types = new Type[1];
        types[0] = typeof(long);
        content = new object[1];

    }
    public override void OnReceive(int localId)
    {
        Debug.Log("Hussa");
        LevelManager.SetSelectedLevel((long)content[0]);
        //Move so that every new player also knows
        Server.SendPacketToAllExcept(localId,this);
    }
}
