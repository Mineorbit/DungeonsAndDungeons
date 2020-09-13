using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle : MonoBehaviour
{
    public static void PlayerGameReady(int from, Packet _packet)
    {
        int localid = from;
        PlayerManager.setupPlayer(localid);

    }
    public static void PlayerDisconnect(int from, Packet _packet)
    {
        PlayerManager.destroyPlayer(from);
        Server.clients[from].Disconnect();
        ServerSend.PlayerDisconnected(from);
    }
    public static void PlayerLocomotionData(int from, Packet _packet)
    {
        int localid = from;
        float x = 0;
        float y = 0;
        float z = 0;
        float qx = 0;
        float qy = 0;
        float qz = 0;
        float qw = 0;

        x = _packet.ReadFloat();
        y = _packet.ReadFloat();
        z = _packet.ReadFloat();
        qx = _packet.ReadFloat();
        qy = _packet.ReadFloat();
        qz = _packet.ReadFloat();
        qw = _packet.ReadFloat();
        Vector3 loc = new Vector3(x,y,z);
        Quaternion rot = new Quaternion(qx,qy,qz,qw);
        PlayerManager.players[localid].updateLocomotionData(loc,rot);
        
    }

}