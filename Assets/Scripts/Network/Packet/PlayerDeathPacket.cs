using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class PlayerDeathPacket : Packet
{
    public PlayerDeathPacket()
    {
        packetId = 12;
        types = new Type[1];
        types[0] = typeof(int);
        content = new object[9];

    }
    public PlayerDeathPacket(int localId)
    {
        packetId = 12;
        types = new Type[1];
        types[0] = typeof(int);
        content = new object[1];
        content[0] = localId;
    }
    public override void OnReceive()
    {
        PlayerManager.playerManager.players[(int) content[0]].Kill();
    }
}
