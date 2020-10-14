using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class PlayerDisconnectedPacket : Packet
{
    public PlayerDisconnectedPacket()
    {
        types = new Type[2];
        types[0] = typeof(string);
        types[1] = typeof(int);
        content = new object[2];
        content[0] = "default";
        content[1] = 0;
        packetId = 3;
    }
    public PlayerDisconnectedPacket(string reason,int id)
    {
        types = new Type[2];
        types[0] = typeof(string);
        types[1] = typeof(int);
        content = new object[2];
        content[0] = reason;
        content[1] = id;
        packetId = 3;
    }
    public override void OnReceive(int localId)
    {
        Debug.Log($"Player {localId} disconnected: {content[0]}");
        ServerManager.instance.RemoveClient(localId);

        PlayerDisconnectedPacket respP = new PlayerDisconnectedPacket((string) content[0] ,localId);
        Server.SendPacketToAllExcept(localId,respP);
    }
}
