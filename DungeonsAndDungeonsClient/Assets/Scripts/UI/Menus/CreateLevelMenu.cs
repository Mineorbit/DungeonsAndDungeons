using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateLevelMenu : MenuPage
{
    Button startCreateButton;
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
    }
    void GoNewLevel()
    {
        LevelData.LevelMetaData levelMetaData = new LevelData.LevelMetaData("Test");

        GameManager.instance.createLevel(levelMetaData);
    }
}
