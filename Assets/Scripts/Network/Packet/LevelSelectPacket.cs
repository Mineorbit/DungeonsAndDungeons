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
        content[0] = levelMetaData.ulid;

    }
    public override void OnReceive()
    {
        LobbyMenu.SetSelectedLevel((long)content[0]);
    }
}
