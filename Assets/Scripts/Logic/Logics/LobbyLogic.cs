using com.mineorbit.dungeonsanddungeonscommon;

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
    }

    public void ReadyPlayer(int localId, bool ready)
    {
        
    }
    public void OpenImmediate(string name)
    {
        LevelDataManager.instance.networkLevels = new LevelMetaData[0];
        NetworkManager.instance.Connect("127.0.0.1", name, OpenLobbyMenu);
    }

    public void Open(string ip, string name)
    {
        LevelDataManager.instance.networkLevels = new LevelMetaData[0];

        NetworkManager.instance.Connect(ip, name, OpenLobbyMenu);
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

    private void OpenLobbyMenu()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.FromPlayToLobby);
        LobbyMenu.UpdateDisplay();
    }

    private void OpenLobbyMenuImmediate()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.FromNoneToLobby);
        LobbyMenu.UpdateDisplay();
    }
}