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
        content[0] = null;
        packetId = 1;
    }
    public override void OnReceive()
    {
        Debug.Log("Hurra ein InfoPacket: "+content[0]);
        NetworkManager.instance.globalId = (int)content[0];
    }
}
