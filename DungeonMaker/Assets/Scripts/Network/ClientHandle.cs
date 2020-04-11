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
        Client.updateNetworkMessage("Vebunden zum Server");
        ClientSend.PlayerConnect("TestUser123");

    }
    public static void Information(Packet _packet)
    {

        string info = _packet.ReadString();

        Debug.Log($"Message: {info}");
        Client.updateNetworkMessage(info);

    }
}