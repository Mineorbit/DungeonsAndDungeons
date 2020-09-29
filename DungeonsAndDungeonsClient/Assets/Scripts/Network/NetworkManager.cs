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
    public int globalId;
    void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
        lobbyClient = new Client();
        gameClient = new Client();
    }
    public void LobbyConnect(UnityEvent onConnectEvent)
    {
        lobbyClient.Connect(onConnectEvent);
    }
    public void LobbyConnect(UnityEvent onConnectEvent, string name)
    {
        username = name;
        onConnectEvent.AddListener(sendUsernameData);
        AlertScreen.alert.Open();
        onConnectEvent.AddListener(AlertScreen.alert.Close);
        lobbyClient.Connect(onConnectEvent);

     
    }

    void sendUsernameData()
    {
        //Sende Message with name
        PlayerConnectedPacket usernamePacket = new PlayerConnectedPacket();
        lobbyClient.Send(usernamePacket);
    }

}
