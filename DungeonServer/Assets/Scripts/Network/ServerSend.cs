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
        for (int i = 1; i <= 4; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }
    public static void PlayerPosition(Player p)
    {

    }
    //Tell Everyone except player disconnected
    public static void PlayerDisconnected(int localId)
    {
        Packet discPack = new Packet((int)ServerPackets.PlayerGameDisconnect);
        discPack.Write(Server.globalIds[localId]);
        SendTCPDataToAll(localId,discPack);
    }
}