using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    /// <summary>Sends a packet to a client via TCP.</summary>
    /// <param name="_toClient">The client to send the packet the packet to.</param>
    /// <param name="_packet">The packet to send to the client.</param>
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }
    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 0; i < 4; i++)
        {
            if (i != _exceptClient)
            {
                if(Server.clients[i]!=null)
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }
    
    public static void GameReady()
    {
        Packet readyPack =  new Packet((byte)ServerPackets.GameReady);
        for(int i = 0;i<4;i++)
        {
            if(GameLogic.current.players[i]!=null)
            {
                readyPack.Write((byte)1);
                readyPack.Write((byte)1);
            }else
            {
                readyPack.Write((byte)0);
                readyPack.Write((byte)0);
            }
        }

        SendTCPDataToAll(-1,readyPack);
    }
    //Tell Everyone except player disconnected
    public static void PlayerDisconnected(int localId)
    {
        Packet discPack = new Packet((byte)ServerPackets.PlayerGameDisconnect);
        discPack.Write((byte)localId);
        SendTCPDataToAll(localId,discPack);
    }
    public static void PlayerLocomotionData(int localId, Vector3 position, Quaternion rotation)
    {
        Packet locoData =  new  Packet((byte)ServerPackets.PlayerLocomotionData);
        locoData.Write((byte)localId);
        locoData.Write(position.x);
        locoData.Write(position.y);
        locoData.Write(position.z);
        locoData.Write(rotation.x);
        locoData.Write(rotation.y);
        locoData.Write(rotation.z);
        locoData.Write(rotation.w);
        SendTCPDataToAll(localId,locoData);
        
    }
}