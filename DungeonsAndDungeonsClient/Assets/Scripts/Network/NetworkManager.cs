using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    Client lobbyClient;
    Client gameClient;
    public string username;
    public int localId;

    void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
        lobbyClient = new Client();
        gameClient = new Client();
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
        lobbyClient.Connect(onConnectEvent);
     
    }
    public void LobbyCancelConnect()
    {
        AlertScreen.alert.Close();
        lobbyClient.CancelConnect();
    }

    public void LobbyDisconnect(UnityEvent disconnectEvent)
    {   
        lobbyClient.Disconnect(disconnectEvent);
    }

    void sendUsernameData()
    {
        PlayerConnectedPacket usernamePacket = new PlayerConnectedPacket(username);
        lobbyClient.Send(usernamePacket);
    }

}
