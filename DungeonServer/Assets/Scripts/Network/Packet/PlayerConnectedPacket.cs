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
    public PlayerConnectedPacket(string name, int l)
    {
        Type t = typeof(string);
        types = new Type[2];
        types[0] = t;
        types[1] = typeof(int);
        content = new object[2];
        content[0] = name;
        content[1] = l;
        packetId = 1;
    }
    public override void OnReceive(int localId)
    {
        if( (int) content[1] == -1)
        {
        Server.GetClient(localId).name = (string) content[0];

        ServerManager.instance.AddClient(localId,Server.GetClient(localId));

        ConnectionInfoPacket cIp = new ConnectionInfoPacket(localId);
        Server.SendPacket(localId,cIp);

            //Informiere neuen Spieler über alle bisherigen
            foreach(Player pl in PlayerManager.playerManager.players)
            {
                if(pl!=null)
                {
                PlayerConnectedPacket newPlayerPacket = new PlayerConnectedPacket(pl.name, pl.localId);
                Server.SendPacket(localId, newPlayerPacket);
                }
            }

            //Sende LevelListe später wo anders

            PlayerManager.playerManager.players[localId].SendLevelList();
            PlayerManager.SpawnPlayerInLobby(localId);

            //Informiere alle spieler über neuen Spieler

            PlayerConnectedPacket playerConnectedPacket = new PlayerConnectedPacket((string)content[0],localId);
        Server.SendPacketToAllExcept(localId,playerConnectedPacket);
        }

    }
}
