using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EditMenu : MenuPage
{
    Button newLevelButton;
    Button backButton;
    Button editButton;
    Button deleteButton;
    Button uploadButton;
    LevelList levelList;
    public override void Awake()
    {
        base.Awake();
        index = 3;
        setupUI();
    }
    void setupUI()
    {
        Transform Interface = transform.Find("Interface");
        levelList = transform.Find("LevelList").GetComponent<LevelList>();
        editButton = Interface.Find("Edit").GetComponent<Button>();
        newLevelButton = Interface.Find("New").GetComponent<Button>();
        deleteButton = Interface.Find("Delete").GetComponent<Button>();
        backButton = transform.Find("Back").GetComponent<Button>();
        uploadButton = Interface.Find("Upload").GetComponent<Button>();
        uploadButton.onClick.AddListener(OpenUploadMenu);
        newLevelButton.onClick.AddListener(OpenNewLevelMenu);
        backButton.onClick.AddListener(GoBack);
        editButton.onClick.AddListener(StartEdit);
        deleteButton.onClick.AddListener(RemoveLevel);
    }
    void RemoveLevel()
    {
        LevelData.LevelMetaData metaData = levelList.GetSelectedLevel();
        if (metaData == null) return;
        LevelManager.Delete(metaData);
    }
    void OpenUploadMenu()
    {
        LevelData.LevelMetaData metaData = levelList.GetSelectedLevel();
        UploadMenu.levelToUpload = metaData;
        MainMenuManager.instance.OpenPage(MainMenuManager.Transaction.FromEditToUploadMenu);
    }
    void OpenNewLevelMenu()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.Transaction.FromEditToCreateMenu);
    }
    void StartEdit()
    {

        LevelData.LevelMetaData metaData = levelList.GetSelectedLevel();
        if (metaData == null) return;
        GameManager.instance.editLevel(metaData);
    }
    void GoBack()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.Transaction.GoBack);
    }


    public override void Start()
    {
        base.Start();

    }
}
