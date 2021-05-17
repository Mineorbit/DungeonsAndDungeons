using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    public static Button goBackButton;
    private Button leaveButton;
    private Button settingsButton;

    private void Start()
    {
        goBackButton = transform.Find("Back").GetComponent<Button>();
        settingsButton = transform.Find("Settings").GetComponent<Button>();
        leaveButton = transform.Find("Leave").GetComponent<Button>();
        goBackButton.onClick.AddListener(GoBack);
        settingsButton.onClick.AddListener(GoOptions);
        leaveButton.onClick.AddListener(ExitGame);
    }

    private void GoBack()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.GoBack);
    }

    private void GoOptions()
    {
        Debug.Log("Opening");
        OptionsMenu.options.Open();
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}