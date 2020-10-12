using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class PlayerConnectedPacket : Packet
{
    public PlayerConnectedPacket(string name)
    {
        Type t = typeof(string);
        types = new Type[1];
        types[0] = t;
        content = new object[1];
        content[0] = name;
        packetId = 1;
    }
    public override void OnReceive(int localId)
    {
        ConnectionInfoPacket cIp = new ConnectionInfoPacket(localId);
        Server.SendPacket(localId,cIp);
    }
}
