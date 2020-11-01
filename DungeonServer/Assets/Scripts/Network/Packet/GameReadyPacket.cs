using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class GameReadyPacket : Packet
{
    //One Field LocalId of PlayerReady if -1 discard else mark players
    public GameReadyPacket(bool go)
    {
        types = new Type[1];
        types[0] = typeof(int);
        content = new object[1];
        content[0] = go? 4:-1;
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

    //Also Send packet if between 0 and 3 so they update ui
    public override void OnReceive(int localId)
    {


        GameLogic.current.players[localId].ready = !GameLogic.current.players[localId].ready;



        GameReadyPacket fanswerPacket = new GameReadyPacket(true);
        fanswerPacket.content[0] = localId;
        Server.SendPacketToAllExcept(localId, fanswerPacket);

        Debug.Log("Check");
        for (int i = 0;i<4;i++)
        {
            
            if(GameLogic.current.players[i] != null)
            { 
            if(!GameLogic.current.players[i].ready)
            {
                return;
            }
            }
        }

        ServerManager.instance.performAction(ServerManager.GameAction.StartGame);
    }
}
