using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static void PlayerGameReady(Packet _packet)
    {
        int localid = _packet.ReadByte();
        GameLogic.current.setupPlayer(localid);
        GameLogic.current.checkAllPlayersReady();

    }
    public static void PlayerLocomotionData(Packet _packet)
    {
        int localid = _packet.ReadByte();
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
        Quaternion q = new Quaternion(qx,qy,qz,qw);
        GameLogic.current.players[localid].
        
    }

}