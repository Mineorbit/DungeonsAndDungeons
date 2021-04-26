using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    public static Button goBackButton;
    Button settingsButton;
    Button leaveButton;
    void Start()
    {
        goBackButton = transform.Find("Back").GetComponent<Button>();
        settingsButton = transform.Find("Settings").GetComponent<Button>();
        leaveButton = transform.Find("Leave").GetComponent<Button>();
        goBackButton.onClick.AddListener(GoBack);
        settingsButton.onClick.AddListener(GoOptions);
        leaveButton.onClick.AddListener(ExitGame);
    }
    void GoBack()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.GoBack);
    }
    void GoOptions()
    {
        Debug.Log("Opening");
        OptionsMenu.options.Open();
    }
    void ExitGame()
    {
        Application.Quit();
    }
    
}
