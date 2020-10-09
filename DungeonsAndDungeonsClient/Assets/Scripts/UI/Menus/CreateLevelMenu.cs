using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateLevelMenu : MenuPage
{
    Button startCreateButton;
    Button backButton;
    InputField inputField;
    public void Awake()
    {
        base.Awake();
        index = 4;
        setupUI();
    }
    void setupUI()
    {
        Transform Interface = transform.Find("Interface");
        startCreateButton = Interface.Find("StartCreateLevel").GetComponent<Button>();
        backButton = transform.Find("Back").GetComponent<Button>();
        startCreateButton.onClick.AddListener(GoNewLevel);
        backButton.onClick.AddListener(GoBack);
        inputField = Interface.Find("Name").GetComponent<InputField>();
    }
    void GoBack()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.Transaction.GoBack);
    }
    void GoNewLevel()
    {
        string name = inputField.text;
        LevelData.LevelMetaData levelMetaData = new LevelData.LevelMetaData(name);
        GameManager.instance.createLevel(levelMetaData);
    }
}
