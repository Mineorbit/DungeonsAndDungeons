using System.Collections;
using System.Collections.Generic;
using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;
using UnityEngine.UI;

public class NetInterface : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    private Button downloadButton;
    void Start()
    {
        SetupNetInterface();
    }

    void SetupNetInterface()
    {
        downloadButton = transform.Find("Download").GetComponent<Button>();
        downloadButton.onClick.AddListener(DownloadLevel);
    }
    
    private void DownloadLevel()
    {
        var metaData = EditMenu.currentLevelList.GetSelectedLevel();
        LevelDataManager.New(metaData,saveImmediately:true,instantiateImmediately: false);
        HttpManager.DownloadLevel(metaData);
        LevelDataManager.Save(metaData: false, levelData:false, extraSaveMetaData: metaData);
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
}
