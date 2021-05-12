using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenu : MenuPage
{
    public void Awake()
    {
        base.Awake();
        index = 1;
    }

    public override void Start()
    {
        base.Start();
        Setup();
    }
    public void Setup()
    {
        Button lobbyButton = transform.Find("Connect").Find("Go").GetComponent<Button>();
        lobbyButton.onClick.AddListener(goLobby);
    }
    void goLobby()
    {
        string name = transform.Find("Connect").Find("Name").GetComponentInChildren<InputField>().text;
        string ip = transform.Find("Connect").Find("IP").GetComponentInChildren<InputField>().text;
        LobbyLogic.lobbyLogic.Open(ip,name);
    }
}
