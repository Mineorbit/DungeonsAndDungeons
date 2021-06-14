using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class WinMenu : MenuPage
{
    // Start is called before the first frame update

    public Button toLobby;
    public override  void Awake()
    {
        base.Awake();
        base.
        index = 6;
    }

    public override void Start()
    {
        base.Start();
        Setup();
    }

    void Setup()
    {
        toLobby.onClick.AddListener(GoToLobby);
    }

    void GoToLobby()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.FromWinToLobby);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
