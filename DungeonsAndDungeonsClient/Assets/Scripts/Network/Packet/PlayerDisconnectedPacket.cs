using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisconnectedPacket : Packet
{
    public PlayerDisconnectedPacket()
    {

        packetId = 8;
    }

    public override void OnReceive()
    {

    }
}
