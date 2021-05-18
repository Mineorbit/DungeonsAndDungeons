using com.mineorbit.dungeonsanddungeonscommon;
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

        readyButton = transform.Find("Actions").Find("Ready").GetComponent<Toggle>();

        readyButton.onValueChanged.AddListener(delegate { CallReady(); });
        NetworkManager.readyEvent.AddListener((x)=>ChangeReadyDisplay(x.Item2,x.Item1));
        
    }

    public void ChangeReadyDisplay(bool r, int localId)
    {
        PlayerView.playerView.playerElements[localId].SetReady(r);
    }
        
    public static void UpdateDisplay()
    {
        PlayerView.playerView.UpdatePlayerView();
    }

    public static void SetSelectedLevel(long ulid)
    {
        netList.SetSelected(ulid);
    }

    private void CallReady()
    {
        NetworkManager.instance.CallReady(readyButton.isOn);
    }
}