using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MenuPage
{
    Button playModeButton;
    Button editModeButton;
    public override void Awake()
    {
        base.Awake();
        index = 0;
    }
    public override void Start()
    {
        base.Start();
        setupButtons(); 
    }
    void setupButtons()
    {
        playModeButton = transform.Find("Play").GetComponent<Button>();
        editModeButton = transform.Find("Edit").GetComponent<Button>();
        playModeButton.onClick.AddListener(enterPlay);
        editModeButton.onClick.AddListener(enterEdit);
    }
    
    void enterPlay()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.Transaction.FromMainToPlay);
    }
    void enterEdit()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.Transaction.FromMainToEdit);
    }
}
