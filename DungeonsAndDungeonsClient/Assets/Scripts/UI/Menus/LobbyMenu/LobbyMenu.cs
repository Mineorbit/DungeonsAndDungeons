using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LobbyMenu : MenuPage
{
    Button readyButton;
    public override void Awake()
    {
        base.Awake();
        index = 2;

        readyButton = transform.Find("Actions").Find("Ready").GetComponent<Button>();
        readyButton.onClick.AddListener(CallReady);
    }
    void CallReady()
    {
        NetworkManager.instance.CallReady();
    }
    public override void Open()
    {
        base.Open();

    }
    public override void Close()
    {
        base.Close();
    }
}
