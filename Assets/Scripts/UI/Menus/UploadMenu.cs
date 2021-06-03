using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Text;
using com.mineorbit.dungeonsanddungeonscommon;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UploadMenu : MenuPage
{
    public static NetLevel.LevelMetaData levelToUpload;


    private TextMeshProUGUI nameText;
    private Button uploadButton;
    
    

    public FileStructureProfile levels;
    public FileStructureProfile compressedLevels;
    public override void Awake()
    {
        base.Awake();
        index = 5;
        var Interface = transform.Find("Interface");
        nameText = Interface.Find("LevelName").GetComponent<TextMeshProUGUI>();
        uploadButton = Interface.Find("Upload").GetComponent<Button>();
        uploadButton.onClick.AddListener(
            ()=>HttpManager.StartUpload(levelToUpload));
    }


    

}