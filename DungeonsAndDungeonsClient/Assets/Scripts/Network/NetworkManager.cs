using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    public Client gameClient;
    public string username;
    public int localId;

    void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
        Setup();
    }
    void Setup()
    {
        if(gameClient == null)
        gameClient = gameObject.AddComponent<Client>();
        gameClient.Setup("127.0.0.1", 13587, "GameClient");
    }
    public void Reset()
    {
        GameDisconnect();
        localId = -1;
        username = "";
    }
    public void GameConnect(UnityEvent onConnectEvent)
    {
        GameConnect(onConnectEvent,"123456");
    }
    public void GameConnect(UnityEvent onConnectEvent, string name)
    {
        Setup();

        username = name;

        UnityEvent cancelEvent = new UnityEvent();
        cancelEvent.AddListener(GameCancelConnect);
        AlertScreen.alert.Open("Verbinde zu Lobby ...",cancelEvent);
        onConnectEvent.AddListener(sendUsernameData);
        onConnectEvent.AddListener(AlertScreen.alert.Close);
        gameClient.Connect(onConnectEvent);
     
    }
    public void GameCancelConnect()
    {
        AlertScreen.alert.Close();
        gameClient.CancelConnect();
    }
    public void GameDisconnect()
    {
        PlayerManager.playerManager.Remove(localId);
        gameClient.Disconnect();
    }
    public void GameDisconnect(UnityEvent disconnectEvent)
    {
        PlayerManager.playerManager.Remove(localId);
        gameClient.Disconnect(disconnectEvent);
    }

    void sendUsernameData()
    {
        PlayerConnectedPacket usernamePacket = new PlayerConnectedPacket(username);
        gameClient.Send(usernamePacket);
    }
    public void CallReady(bool rdyState)
    {
        GameReadyPacket gameReadyPacket = new GameReadyPacket(localId, rdyState);
        gameClient.Send(gameReadyPacket);
    }

    public void SendLevelSelection(LevelData.LevelMetaData d)
    {
        LevelSelectPacket levelSelectPacket = new LevelSelectPacket(d);
        gameClient.Send(levelSelectPacket);
    }

    public void SendLocomotionData(PlayerLocomotionPacket p)
    {
        gameClient.Send(p);
    }
}
