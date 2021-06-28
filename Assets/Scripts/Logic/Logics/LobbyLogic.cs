using System.Collections;
using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;

public class LobbyLogic : Logic
{
    public static LobbyLogic lobbyLogic;

    private int localPlayer;

    public override void Init()
    {
        sceneIndex = 1;

        if (lobbyLogic == null)
            lobbyLogic = this;
        
        
        NetworkManager.readyEvent.AddListener( (x) => ReadyPlayer(x.Item1,x.Item2));
        PlayerManager.DeactivateAllPlayers();
    }

    public void ReadyPlayer(int localId, bool ready)
    {
        
    }

    
    
    public void Open(string ip, string name)
    {
        LevelDataManager.instance.networkLevels = new NetLevel.LevelMetaData[0];

        NetworkManager.instance.Connect(ip, name, OpenLobbyMenu);
    }



    public void OnLobbyOpen()
    {
        NetworkManager.instance.CallLobbyReady();
    }


 

    private void OpenLobbyMenu()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.FromPlayToLobby);
        LobbyMenu.UpdateDisplay();
    }
}