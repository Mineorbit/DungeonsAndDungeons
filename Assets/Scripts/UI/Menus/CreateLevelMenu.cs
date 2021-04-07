using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateLevelMenu : MenuPage
{
    Button startCreateButton;
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
        startCreateButton.onClick.AddListener(GoNewLevel);
        inputField = Interface.Find("Name").GetComponent<InputField>();
    }
    
    void GoNewLevel()
    {
        string name = inputField.text;

        //Do some checks
        LevelData.LevelMetaData levelMetaData = new LevelData.LevelMetaData(name);
        GameManager.instance.createLevel(levelMetaData);
    }
}
