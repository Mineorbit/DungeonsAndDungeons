using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MenuPage
{
    Button testModeButton;
    public override void Start()
    {
        base.Start();
        setupButtons(); 
    }
    void setupButtons()
    {
        testModeButton = transform.Find("Test").GetComponent<Button>();
        testModeButton.onClick.AddListener(enterTest);
    }
    void enterTest()
    {
        GameManager.instance.performAction(GameManager.GameAction.EnterTestFromMainMenu);
    }
}
