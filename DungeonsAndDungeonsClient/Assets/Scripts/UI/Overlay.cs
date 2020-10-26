using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    Button goBackButton;
    Button settingsButton;
    void Start()
    {
        goBackButton = transform.Find("Back").GetComponent<Button>();
        settingsButton = transform.Find("Settings").GetComponent<Button>();
        goBackButton.onClick.AddListener(GoBack);
    }
    void GoBack()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.Transaction.GoBack);
    }
    
}
