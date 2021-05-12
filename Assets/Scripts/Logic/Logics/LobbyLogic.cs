using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using com.mineorbit.dungeonsanddungeonscommon;

public class LobbyLogic : Logic
{
    public static LobbyLogic lobbyLogic;

    int localPlayer = 0;

    public override void Init()
    {

        sceneIndex = 1;

        if (lobbyLogic == null)
            lobbyLogic = this;


    }
    public void OpenImmediate(string name)
    {


        LevelDataManager.instance.networkLevels = new LevelMetaData[0];
        NetworkManager.instance.Connect("127.0.0.1",name, OpenLobbyMenu);

    }
    public void Open(string ip, string name)
    {
        LevelDataManager.instance.networkLevels = new LevelMetaData[0];

        NetworkManager.instance.Connect(ip,name, OpenLobbyMenu);

    }
    public void AddLocalPlayer(int localId, string name)
    {
        localPlayer = localId;
        AddPlayer(localId, name);
    }

    public void RemoveLocalPlayer()
    {
        RemovePlayer(localPlayer);
    }

    public void AddPlayer(int localId, string name)
    {



        PlayerManager.playerManager.Add(localId, name, false);

        LobbyMenu.UpdateDisplay();
    }


    public void RemovePlayer(int localId)
    {
        PlayerManager.playerManager.Remove(localId);
        LevelDataManager.instance.networkLevels = new LevelMetaData[0];
        LobbyMenu.UpdateDisplay();
    }

    void OpenLobbyMenu()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.FromPlayToLobby);
        LobbyMenu.UpdateDisplay();
    }
    void OpenLobbyMenuImmediate()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.FromNoneToLobby);
        LobbyMenu.UpdateDisplay();
    }


}
