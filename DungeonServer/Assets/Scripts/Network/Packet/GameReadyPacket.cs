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

    //Also Send packet if between 0 and 3 so they update ui
    public override void OnReceive(int localId)
    {


        PlayerManager.playerManager.players[localId].ready = (bool) content[1];



        GameReadyPacket fanswerPacket = new GameReadyPacket((int) content[0],(bool)content[1]);
        Server.SendPacketToAllExcept(localId, fanswerPacket);
        bool startGame = true;
        bool anyone = false;
        Debug.LogError("Checking if to Start");
        for (int i = 0;i<4;i++)
        {
            
            if(PlayerManager.playerManager.players[i] != null)
            {
                anyone = true;
                startGame = startGame && PlayerManager.playerManager.players[i].ready;
            
            }
        }
        if(startGame && anyone)
        ServerManager.instance.performAction(ServerManager.GameAction.StartGame);
    }
}
