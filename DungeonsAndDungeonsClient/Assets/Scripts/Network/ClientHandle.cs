using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    
    //Für jede PaketArt einen Handler
    public static void ConnectInfo(Packet _packet)
    {

        Debug.Log($"Verbunden mit Server");
        int _myId = _packet.ReadInt();


    }
    public static void Information(Packet _packet)
    {

        string info = _packet.ReadString();

        Debug.Log($"Message: {info}");

    }
    public static void GameReady(Packet _packet)
    {
       
    }
    public static void PlayerLocomotionData(Packet _packet)
    {
        int locId =_packet.ReadByte();
        float x = _packet.ReadFloat();
        float y = _packet.ReadFloat();
        float z = _packet.ReadFloat();
        float qx = _packet.ReadFloat();
        float qy = _packet.ReadFloat();
        float qz = _packet.ReadFloat();
        float qw = _packet.ReadFloat();
        Vector3 loc = new Vector3(x,y,z);
        Quaternion rot = new  Quaternion(qx,qy,qz,qw);
    }
    public static void PlayerLobbyDisconnect(Packet _packet)
    {

    }
    public static void PlayerGameDisconnect(Packet _packet)
    {
        int localId = (int) _packet.ReadByte();
    }
}