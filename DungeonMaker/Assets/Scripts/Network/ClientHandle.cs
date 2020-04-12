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

        Client.instance.globalId = _myId;
        Client.updateNetworkMessage("Vebunden zum Server");
        ClientSend.PlayerConnect(Client.instance.name);
        ConnectBar.current.close();
        LobbyBar.current.Start();
        LobbyBar.current.open();

    }
    public static void Information(Packet _packet)
    {

        string info = _packet.ReadString();

        Debug.Log($"Message: {info}");
        Client.updateNetworkMessage(info);

    }
}