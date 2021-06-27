using com.mineorbit.dungeonsanddungeonscommon;
using NetLevel;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenu : MenuPage
{
    private static LevelList netList;
    private Toggle readyButton;

    public override void Awake()
    {
        base.Awake();
        index = 2;

        netList = transform.Find("LevelList").GetComponent<LevelList>();
        netList.levelSelectedEvent.AddListener((x) =>
        {
            CallSelected(x);
        });
        readyButton = transform.Find("Actions").Find("Ready").GetComponent<Toggle>();

        readyButton.onValueChanged.AddListener(delegate { CallReady(); });
        NetworkManager.readyEvent.AddListener((x)=>ChangeReadyDisplay(x.Item2,x.Item1));
        NetworkManager.lobbyRequestEvent.AddListener((x) =>
        {
            netList.SetSelectedLevel(x.SelectedLevel, false);
            GameConsole.Log("Level "+x.SelectedLevel+" was selected");
        });
        
    }

    public void CallSelected(LevelMetaData metaData)
    {
        Debug.Log("You selected: "+metaData);
        NetworkManager.instance.CallSelected(metaData);
    }
    
    public void ChangeReadyDisplay(bool r, int localId)
    {
        PlayerView.playerView.playerElements[localId].SetReady(r);
    }
        
    public static void UpdateDisplay()
    {
        PlayerView.playerView.UpdatePlayerView();
    }

  

    private void CallReady()
    {
        NetworkManager.instance.CallReady(readyButton.isOn);
        Debug.Log("Sent Call for Ready");
    }
}