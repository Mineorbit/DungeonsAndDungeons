using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.mineorbit.dungeonsanddungeonscommon;

public class EditMenu : MenuPage
{
    Button newLevelButton;
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
        uploadButton = Interface.Find("Upload").GetComponent<Button>();
        uploadButton.onClick.AddListener(OpenUploadMenu);
        newLevelButton.onClick.AddListener(OpenNewLevelMenu);
        editButton.onClick.AddListener(StartEdit);
        deleteButton.onClick.AddListener(RemoveLevel);
    }
    void RemoveLevel()
    {
        LevelMetaData metaData = levelList.GetSelectedLevel();
        if (metaData == null) return;
        LevelDataManager.Delete(metaData);
    }
    void OpenUploadMenu()
    {
        LevelMetaData metaData = levelList.GetSelectedLevel();
        if(metaData!=null)
        { 
        UploadMenu.levelToUpload = metaData;
        MainMenuManager.instance.OpenPage(MainMenuManager.FromEditToUploadMenu);
        }
    }
    void OpenNewLevelMenu()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.FromEditToCreateMenu);
    }
    void StartEdit()
    {

        LevelMetaData metaData = levelList.GetSelectedLevel();
        if (metaData == null) return;
        GameManager.instance.editLevel(metaData);
    }


    public override void Start()
    {
        base.Start();

    }
}
