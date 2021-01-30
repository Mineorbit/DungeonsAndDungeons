using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;


public class PlayerLocomotionPacket : Packet
{
    public PlayerLocomotionPacket()
    {
        types = new Type[8];
        
        content = new object[8];

        for (int i = 0; i < 7; i++)
        {
            types[i] = typeof(float);
            content[i] = 0f;
        }

        types[7] = typeof(int);

        content[7] = -1;

        packetId = 6;
    }
    public PlayerLocomotionPacket(Vector3 position, Quaternion rotation, int localId)
    {
        types = new Type[8];

        for (int i = 0; i < 7; i++)
        {
            types[i] = typeof(float);
        }
        types[7] = typeof(int);

        content = new object[8];
        content[0] = position.x;
        content[1] = position.y;
        content[2] = position.z;

        content[3] = rotation.x;
        content[4] = rotation.y;
        content[5] = rotation.z;
        content[6] = rotation.w;

        content[7] = localId;
        packetId = 6;
    }
    //State Must be  Play
    public override void OnReceive()
    {
        int localId = (int)content[7];

        Vector3 position = new Vector3((float)content[0], (float)content[1], (float)content[2]);
        Quaternion rotation = new Quaternion((float)content[3], (float)content[4], (float)content[5], (float)content[6]);
        if(localId>=0)
            if(PlayerManager.playerManager.players[localId]!=null)
            {
                ( (NetPlayer) PlayerManager.playerManager.players[localId]).setTargetLocomotionData(position,rotation);
            }


    }
}
