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
        MainCaller.startCoroutine(SetLobbyPosition());
    }

    public void ReadyPlayer(int localId, bool ready)
    {
        
    }

    IEnumerator SetLobbyPosition()
    {
        while (!NetworkManager.isConnected)
        {
            yield return new WaitForEndOfFrame();
        }
        GameObject player = PlayerManager.playerManager.GetPlayer(NetworkManager.instance.localId);
        while (player == null)
        {
            player = PlayerManager.playerManager.GetPlayer(NetworkManager.instance.localId);
            yield return new WaitForEndOfFrame();
        }
        
        Vector3 target = new Vector3(8*NetworkManager.instance.localId,6,0);
        
        player.transform.position = target;
        player.transform.rotation = Quaternion.identity;
        while ((player.transform.position - target).magnitude > 0.05f)
        {
            player.transform.position = target;
            player.transform.rotation = Quaternion.identity;
            yield return new WaitForEndOfFrame();
        }
    }
    
    public void Open(string ip, string name)
    {
        LevelDataManager.instance.networkLevels = new NetLevel.LevelMetaData[0];

        NetworkManager.instance.Connect(ip, name, OpenLobbyMenu);
    }


    



 

    private void OpenLobbyMenu()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.FromPlayToLobby);
        LobbyMenu.UpdateDisplay();
    }
}