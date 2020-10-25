using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UploadMenu : MenuPage
{
    public static LevelData.LevelMetaData levelToUpload;

    Button goBack;
    public override void Awake()
    {
        base.Awake();
        goBack = transform.Find("Back").GetComponent<Button>();
        goBack.onClick.AddListener(GoBack);
        index = 5;
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
