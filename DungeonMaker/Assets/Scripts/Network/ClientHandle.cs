using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    //Für jede PaketArt einen Handler
    public static void ConnectInfo(Packet _packet)
    {

        int _myId = _packet.ReadInt();

        Debug.Log($"Verbunden mit Server");
        Client.instance.globalId = _myId;
        Client.instance.updateGame();
        ClientSend.PlayerConnect("TestUser123");
        
    }
}