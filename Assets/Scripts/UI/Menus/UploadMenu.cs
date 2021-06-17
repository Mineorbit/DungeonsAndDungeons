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
   


    private static TextMeshProUGUI nameText;
    private static Button uploadButton;
    
    public static NetLevel.LevelMetaData _levelToUpload;
    public static NetLevel.LevelMetaData levelToUpload{
        get
        {
            return _levelToUpload;
        }
    
        set
        {
            _levelToUpload = value;
            if (nameText != null)
            {
                nameText.text = levelToUpload.FullName;
            }
        }
    }

    public FileStructureProfile levels;
    public FileStructureProfile compressedLevels;
    public override void Awake()
    {
        base.Awake();
        index = 5;
        var Interface = transform.Find("Interface");
        nameText = Interface.Find("LevelName").GetComponent<TextMeshProUGUI>();
        Interface.Find("Info").GetComponent<TextMeshProUGUI>().text = "";
        uploadButton = Interface.Find("Upload").GetComponent<Button>();
        uploadButton.onClick.AddListener(
            ()=>HttpManager.StartUpload(levelToUpload));
    }


    

}