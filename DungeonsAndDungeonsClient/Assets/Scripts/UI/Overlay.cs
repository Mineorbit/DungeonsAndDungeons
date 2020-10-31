using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    public static Button goBackButton;
    Button settingsButton;
    void Start()
    {
        goBackButton = transform.Find("Back").GetComponent<Button>();
        settingsButton = transform.Find("Settings").GetComponent<Button>();
        goBackButton.onClick.AddListener(GoBack);
        settingsButton.onClick.AddListener(GoOptions);
    }
    void GoBack()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.Transaction.GoBack);
    }
    void GoOptions()
    {
        Debug.Log("Opening");
        Options.options.Open();
    }
    
}
