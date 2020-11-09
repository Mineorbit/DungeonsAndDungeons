using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LobbyMenu : MenuPage
{
    Toggle readyButton;
    public override void Awake()
    {
        base.Awake();
        index = 2;

        readyButton = transform.Find("Actions").Find("Ready").GetComponent<Toggle>();

        readyButton.onValueChanged.AddListener(delegate { CallReady(); });
    }
    public static void UpdateDisplay()
    {

        PlayerView.playerView.UpdatePlayerView(PlayerManager.playerManager.players);
    }
    void CallReady()
    {
        NetworkManager.instance.CallReady(readyButton.isOn);
    }
}
