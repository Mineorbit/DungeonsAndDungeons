using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using com.mineorbit.dungeonsanddungeonscommon;

public class Lobby : Logic
{
    public static Lobby lobby;

    int localPlayer = 0;

    public override void Init()
    {

        sceneIndex = 1;

        if (lobby == null)
            lobby = this;

    }
    public void OpenImmediate(string name)
    {


        LevelDataManager.networkLevels = new LevelMetaData[0];
        //Open Pop Up with connect
        UnityEvent onConnectEvent = new UnityEvent();
        onConnectEvent.AddListener(OpenLobbyMenuImmediate);
        //NetworkManager.instance.GameConnect(onConnectEvent, name);

    }
    public void Open(string name)
    {
        LevelDataManager.networkLevels = new LevelMetaData[0];

        //Open Pop Up with connect
        UnityEvent onConnectEvent = new UnityEvent();
        onConnectEvent.AddListener(OpenLobbyMenu);
        //NetworkManager.instance.GameConnect(onConnectEvent,name);

    }
    public void AddLocalPlayer(int localId, string name)
    {
        localPlayer = localId;
        AddPlayer(localId,name);
    }

    public void RemoveLocalPlayer()
    {
        RemovePlayer(localPlayer);
    }

    public void AddPlayer(int localId, string name)
    {



        PlayerManager.playerManager.Add(localId,name,false);

        LobbyMenu.UpdateDisplay();
    }


    public void RemovePlayer(int localId)
    {
        PlayerManager.playerManager.Remove(localId);
        LevelDataManager.networkLevels = new LevelMetaData[0];
        LobbyMenu.UpdateDisplay();
    }

    void OpenLobbyMenu()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.Transaction.FromPlayToLobby);
        LobbyMenu.UpdateDisplay();
    }
    void OpenLobbyMenuImmediate()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.Transaction.FromNoneToLobby);
        LobbyMenu.UpdateDisplay();
    }


}
