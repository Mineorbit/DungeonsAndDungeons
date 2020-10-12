using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class PlayerDisconnectedPacket : Packet
{
    public PlayerDisconnectedPacket( string reason, int localId)
    {
        Type t = typeof(string);
        types = new Type[2];
        types[0] = t;
        types[1] = typeof(int);
        content = new object[2];
        content[0] = reason;
        content[1] = localId;
        packetId = 1;
    }
    public override void OnReceive()
    {
        int localId = (int) content[1];
        if(localId == NetworkManager.instance.localId)
        {

            //wenn eigene, dann aus spiel rausgehen bzw schauen was zu tun ist.
        }
        else
        {

            //wenn nicht
            //wenn in lobby dann lobby updaten
        }
    }
}
