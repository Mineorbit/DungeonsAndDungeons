using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class PlayerConnectedPacket : Packet
{
    public PlayerConnectedPacket()
    {
        Type t = typeof(string);
        types = new Type[2];
        types[0] = t;
        types[1] = typeof(int);
        content = new object[2];
        content[0] = "Test";
        content[1] = 0;
        packetId = 1;
    }
    public PlayerConnectedPacket(string name)
    {
        Type t = typeof(string);
        types = new Type[2];
        types[0] = t;
        types[1] = typeof(int);
        content = new object[2];
        content[0] = name;
        content[1] = -1;
        packetId = 1;
    }
    public override void OnReceive()
    {
        int localId = (int)content[1];

        //Spawn PlayerChar according to mode
        if (GameManager.GetState() == GameManager.State.MainMenu)
        {
            Lobby.lobby.AddPlayer(localId,(string) content[0]);
        }
    }
}
