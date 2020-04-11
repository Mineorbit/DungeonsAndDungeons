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
        using (Packet _packet = new Packet((int)ClientPackets.PlayerConnect))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(username);
            SendTCPData(_packet);
        }
    }
    
}