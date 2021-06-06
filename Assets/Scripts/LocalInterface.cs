using System.Collections;
using System.Collections.Generic;
using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;
using UnityEngine.UI;

public class LocalInterface : MonoBehaviour
{
    
    private Button deleteButton;
    private Button editButton;
    private Button newLevelButton;
    private Button uploadButton;

    public CanvasGroup canvasGroup;
    
    // Start is called before the first frame update
    void Start()
    {
        SetupLocalInterface();
    }
    void SetupLocalInterface()
    {
        editButton = transform.Find("Edit").GetComponent<Button>();
        newLevelButton = transform.Find("New").GetComponent<Button>();
        deleteButton = transform.Find("Delete").GetComponent<Button>();
        uploadButton = transform.Find("Upload").GetComponent<Button>();
        uploadButton.onClick.AddListener(OpenUploadMenu);
        newLevelButton.onClick.AddListener(OpenNewLevelMenu);
        editButton.onClick.AddListener(StartEdit);
        deleteButton.onClick.AddListener(RemoveLevel);
    }
    
    public void OnEnable()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnDisable()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    
    private void RemoveLevel()
    {
        var metaData = EditMenu.currentLevelList.GetSelectedLevel();
        if (metaData == null) return;
        Debug.Log("Deleting "+metaData);
        LevelDataManager.Delete(metaData);
    }

    private void OpenUploadMenu()
    {
        var metaData = EditMenu.currentLevelList.GetSelectedLevel();
        if (metaData != null)
        {
            UploadMenu.levelToUpload = metaData;
            MainMenuManager.instance.OpenPage(MainMenuManager.FromEditToUploadMenu);
        }
    }

    private void OpenNewLevelMenu()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.FromEditToCreateMenu);
    }

    private void StartEdit()
    {
        var metaData = EditMenu.currentLevelList.GetSelectedLevel();
        Debug.Log("Starting Editing on "+metaData);
        if (metaData == null) return;
        GameManager.instance.editLevel(metaData);
    }
}
