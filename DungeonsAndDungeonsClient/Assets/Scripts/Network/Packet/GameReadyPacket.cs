using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class GameReadyPacket : Packet
{
    //One Field LocalId of PlayerReady if -1 discard else mark players
    public GameReadyPacket(int localId, bool go)
    {
        types = new Type[2];
        types[0] = typeof(int);
        types[1] = typeof(bool);
        content = new object[2];
        content[0] = localId;
        content[1] = go;
        packetId = 5;

    }
    public GameReadyPacket()
    {
        types = new Type[2];
        types[0] = typeof(int);
        types[1] = typeof(bool);
        content = new object[2];
        content[0] = -1;
        content[1] = false;
        packetId = 5;

    }



    public override void OnReceive()
    {
        int localId = (int)content[0];

        if(localId == -1)
        {

        }else 
        if(localId == 4)
        {
            GameManager.instance.performAction(GameManager.GameAction.StartPlay);
        }
    }
}
