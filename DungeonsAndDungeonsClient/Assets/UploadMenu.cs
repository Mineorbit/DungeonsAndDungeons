using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UploadMenu : MenuPage
{
    public static LevelData.LevelMetaData levelToUpload;

    public override void Awake()
    {
        base.Awake();
        index = 5;
    }
    public override void Start()
    {
        base.Start();

    }
}
