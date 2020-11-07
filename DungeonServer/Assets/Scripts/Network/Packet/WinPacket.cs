using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class WinPacket : Packet
{
    public WinPacket()
    {
        packetId = 9;
        types = new Type[0];
        content = new object[0];
    }
}
