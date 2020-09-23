using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenu : MenuPage
{
    public void Awake()
    {
        base.Awake();
        index = 1;
    }

    public override void Start()
    {
        base.Start();
        Setup();
    }
    public void Setup()
    {
        Button backButton = transform.Find("Back").GetComponent<Button>();
        backButton.onClick.AddListener(goBack);
    }
    void goBack()
    {
        MainMenuManager.instance.OpenPage(0);
    }
}
