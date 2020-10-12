using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
public class ConnectionInfoPacket : Packet
{
    public ConnectionInfoPacket(int id)
    {
        Type t = typeof(int);
        types = new Type[1];
        types[0] = t;
        content = new object[1];
        packetId = 1;
        content[0] = id;
    }
    public override void OnReceive()
    {
    }
}
