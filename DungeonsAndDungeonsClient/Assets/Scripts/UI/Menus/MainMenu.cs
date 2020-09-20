using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MenuPage
{
    Button playModeButton;
    Button testModeButton;
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
        testModeButton = transform.Find("Test").GetComponent<Button>();
        testModeButton.onClick.AddListener(enterTest);
        playModeButton.onClick.AddListener(enterPlay);
    }
    void enterPlay()
    {
        MainMenuManager.instance.OpenPage(1);
    }
    void enterTest()
    {
        GameManager.instance.performAction(GameManager.GameAction.EnterTestFromMainMenu);
    }
}
