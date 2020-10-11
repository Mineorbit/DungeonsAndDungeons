using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    Client mainClient;
    Client gameClient;
    public string username;
    public int localId;

    void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
        mainClient = new Client("127.0.0.1", 13587);
        gameClient = new Client("127.0.0.1", 13587);
    }
    public void LobbyConnect(UnityEvent onConnectEvent)
    {
        LobbyConnect(onConnectEvent,"123456");
    }
    public void LobbyConnect(UnityEvent onConnectEvent, string name)
    {
        username = name;
        onConnectEvent.AddListener(sendUsernameData);

        UnityEvent cancelEvent = new UnityEvent();
        cancelEvent.AddListener(LobbyCancelConnect);
        AlertScreen.alert.Open("Verbinde zu Lobby ...",cancelEvent);
        onConnectEvent.AddListener(AlertScreen.alert.Close);
        gameClient.Connect(onConnectEvent);
     
    }
    public void LobbyCancelConnect()
    {
        AlertScreen.alert.Close();
        gameClient.CancelConnect();
    }

    public void LobbyDisconnect(UnityEvent disconnectEvent)
    {   
        gameClient.Disconnect(disconnectEvent);
    }

    void sendUsernameData()
    {
        PlayerConnectedPacket usernamePacket = new PlayerConnectedPacket(username);
        gameClient.Send(usernamePacket);
    }

}
