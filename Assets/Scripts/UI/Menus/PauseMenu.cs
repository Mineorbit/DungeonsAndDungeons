using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool pauseMenuAvailable;
    public bool freezeGame;
    public bool open;
    public bool freezePlayer = true;

    public Button backToMainMenuButton;
    public Button optionsButton;
    public UIAnimation uiAnimation;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!open && !uiAnimation.isOpen() && !BlurScreen.blurScreen.isOpen())
                Open();
            else if (open && uiAnimation.isOpen() && BlurScreen.blurScreen.isOpen()) Close();
        }
    }

    private void Setup()
    {
        uiAnimation = new FadeAndGrow();
        uiAnimation.target = transform;
        backToMainMenuButton = transform.Find("Main").GetComponent<Button>();
        optionsButton = transform.Find("Opt").GetComponent<Button>();
        optionsButton.onClick.AddListener(GotoOptions);
        backToMainMenuButton.onClick.AddListener(GotoMainMenu);
    }

    private void GotoOptions()
    {
        OptionsMenu.options.Open();
    }

    private void GotoMainMenu()
    {
        Close();
        Debug.Log("Entering Main Menu from Pause Menu");
        GameManager.instance.performAction(GameManager.EnterMainMenu);
    }

    private bool checkAvailability()
    {
        if (GameManager.GetState() == GameManager.Init) pauseMenuAvailable = false;
        if (GameManager.GetState() == GameManager.MainMenu)
            pauseMenuAvailable = false;
        else
            pauseMenuAvailable = true;
        if (LoadingScreen.instance.open) pauseMenuAvailable = false;
        return pauseMenuAvailable;
    }

    private void Open()
    {
        if (!checkAvailability()) return;
        open = true;
        if (freezeGame)
            //Inform GameManager of Attempt to (stop simulation in local play)

            if (freezePlayer)
                PlayerManager.acceptInput = false;
        BlurScreen.blurScreen.Open();
        uiAnimation.Play();
        MouseStateController.UnlockBlocking();
    }

    private void Close()
    {
        if (!checkAvailability()) return;

        if (freezeGame)
            //Inform GameManager of Attempt to (unfreeze Sim)
            PlayerManager.acceptInput = true;
        BlurScreen.blurScreen.Close();
        uiAnimation.Play();
        open = false;
        MouseStateController.LockUnblocking();
    }
}