using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class GameReadyPacket : Packet
{
    //One Field LocalId of PlayerReady if -1 discard, if 4 start PlayMode
    public GameReadyPacket(int localId)
    {
        types = new Type[1];
        types[0] = typeof(int);
        content = new object[1];
        content[0] = localId;
        packetId = 5;

    }
    public GameReadyPacket()
    {
        types = new Type[1];
        types[0] = typeof(int);
        content = new object[1];
        content[0] = -1;
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
