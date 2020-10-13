using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
public class ConnectionInfoPacket : Packet
{
    public ConnectionInfoPacket()
    {
        Type t = typeof(int);
        types = new Type[1];
        types[0] = t;
        content = new object[1];
        packetId = 2;
        content[0] = 0;
    }
    public ConnectionInfoPacket(int id)
    {
        Type t = typeof(int);
        types = new Type[1];
        types[0] = t;
        content = new object[1];
        packetId = 2;
        content[0] = id;
    }
    public override void OnReceive()
    {
    }
}
