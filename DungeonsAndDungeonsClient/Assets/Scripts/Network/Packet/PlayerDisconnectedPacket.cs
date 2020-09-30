using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
public class PlayerDisconnectedPacket : Packet
{
    public PlayerDisconnectedPacket()
    {
        types = new Type[0];
        content = new object[0];

        packetId = 2;
    }

    public override void OnReceive()
    {

    }
}
