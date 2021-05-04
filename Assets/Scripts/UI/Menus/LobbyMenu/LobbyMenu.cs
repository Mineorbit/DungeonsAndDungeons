using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.mineorbit.dungeonsanddungeonscommon;

public class LobbyMenu : MenuPage
{
    static LevelList netList;
    Toggle readyButton;
    public override void Awake()
    {
        base.Awake();
        index = 2;

        netList = transform.Find("LevelList").GetComponent<LevelList>();

        readyButton = transform.Find("Actions").Find("Ready").GetComponent<Toggle>();

        readyButton.onValueChanged.AddListener(delegate { CallReady(); });
    }
    public static void UpdateDisplay()
    {

        PlayerView.playerView.UpdatePlayerView();
    }
    public static void SetSelectedLevel(long ulid)
    {
        netList.SetSelected(ulid);
    }
    void CallReady()
    {
        //NetworkManager.instance.CallReady(readyButton.isOn);
    }
}
