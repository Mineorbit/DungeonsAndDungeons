using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionInfoPacket : Packet
{
    public ConnectionInfoPacket()
    {
        packetId = 1;
    }
    public override void OnReceive()
    {
        Debug.Log("Hurra ein InfoPacket");
    }
}
