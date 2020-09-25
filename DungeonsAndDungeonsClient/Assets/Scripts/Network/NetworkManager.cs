using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    Client lobbyClient;
    Client gameClient;
    void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
        lobbyClient = new Client();
        gameClient = new Client();
    }

    public void LobbyConnect()
    {
        lobbyClient.Connect();

        //Sende Message with name
    }

}
