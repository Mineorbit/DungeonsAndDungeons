using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EditMenu : MenuPage
{
    Button newLevelButton;
    Button backButton;
    public override void Awake()
    {
        base.Awake();
        index = 3;
        setupUI();
    }
    void setupUI()
    {
        Transform Interface = transform.Find("Interface");
        newLevelButton = Interface.Find("New").GetComponent<Button>();
        backButton = transform.Find("Back").GetComponent<Button>();
        newLevelButton.onClick.AddListener(OpenNewLevelMenu);
        backButton.onClick.AddListener(GoBack);
    }
    void OpenNewLevelMenu()
    {
        MainMenuManager.instance.OpenPage(4);
    }
    void GoBack()
    {
        MainMenuManager.instance.OpenPage(0);
    }
    public override void Start()
    {
        base.Start();

    }
}
