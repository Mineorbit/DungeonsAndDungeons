using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Courtesy Tom Weiland
//Rework später
public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

   //Funktionen die Fertig Pakete senden

   //Hier HookUp zum Client

    public static void PlayerConnect(string username)
    {
        using (Packet _packet = new Packet((byte)ClientPackets.PlayerConnect))
        {
            Debug.Log($"Schicke Login Paket mit Namen: {username}");
            _packet.Write(username);
            SendTCPData(_packet);
        }
    }
    public static void PlayerGameConnect(int globalId,int localId)
    {
        using (Packet _packet = new Packet((byte)ClientPackets.PlayerGameConnect))
        {
            Debug.Log("Verbinde zu Spiel");
            _packet.Write(globalId);
            _packet.Write((byte)localId);
            SendTCPData(_packet);
        }
    }
    public static void PlayerReady(int id)
    {
        Packet _p = new Packet((byte)ClientPackets.PlayerGameReady);
        Debug.Log("Verschicke ReadyPaket");   
        SendTCPData(_p);
    }
    public static void PlayerLocomotionData(int localId,Vector3 pos, Quaternion rot)
    {

        using (Packet _packet = new Packet((byte)ClientPackets.PlayerLocomotionData))
        {
            _packet.Write(pos.x);
            _packet.Write(pos.y);
            _packet.Write(pos.z);

            _packet.Write(rot.x);
            _packet.Write(rot.y);
            _packet.Write(rot.z);
            _packet.Write(rot.w);
            SendTCPData(_packet);
        }
    }

    
}