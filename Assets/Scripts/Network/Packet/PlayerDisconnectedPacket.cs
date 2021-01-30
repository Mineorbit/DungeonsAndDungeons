using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class PlayerDisconnectedPacket : Packet
{
    public PlayerDisconnectedPacket()
    {
        Type t = typeof(string);
        types = new Type[2];
        types[0] = t;
        types[1] = typeof(int);
        content = new object[2];
        content[0] = "default";
        content[1] = 0;
        packetId = 3;

    }
    public PlayerDisconnectedPacket( string reason, int localId)
    {
        Type t = typeof(string);
        types = new Type[2];
        types[0] = t;
        types[1] = typeof(int);
        content = new object[2];
        content[0] = reason;
        content[1] = localId;
        packetId = 3;


    }
    public override void OnReceive()
    {
        int localId = (int) content[1];
        if (GameManager.GetState() == GameManager.State.MainMenu)
        {
            Lobby.lobby.RemovePlayer(localId);
        }else if(GameManager.GetState() == GameManager.State.Play)
        {
            PlayerManager.playerManager.Remove(localId);
        }

        if (localId == NetworkManager.instance.localId)
        {
            //Hier noch alertScreen einbauen
            GameManager.instance.performAction(GameManager.GameAction.EnterMainMenu);
            NetworkManager.instance.gameClient.OnDisconnect();
        }
        else
        {
            
        }
    }
}
