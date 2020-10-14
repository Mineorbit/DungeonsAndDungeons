using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LobbyMenu : MenuPage
{
    Button backButton;
    public override void Awake()
    {
        base.Awake();
        index = 2;

        backButton = transform.Find("Back").GetComponent<Button>();
        backButton.onClick.AddListener(GoBack);
    }
    void GoBack()
    {

    MainMenuManager.instance.OpenPage(MainMenuManager.Transaction.GoBack);

    }
    public override void Open()
    {
        base.Open();

    }
    public override void Close()
    {
        base.Close();
    }
}
