using UnityEngine.UI;

public class MainMenu : MenuPage
{
    private Button editModeButton;
    private Button playModeButton;

    public override void Awake()
    {
        base.Awake();
        index = 0;
    }

    public override void Start()
    {
        base.Start();
        setupButtons();
    }

    private void setupButtons()
    {
        playModeButton = transform.Find("Play").GetComponent<Button>();
        editModeButton = transform.Find("Edit").GetComponent<Button>();
        playModeButton.onClick.AddListener(enterPlay);
        editModeButton.onClick.AddListener(enterEdit);
    }

    private void enterPlay()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.FromMainToPlay);
    }

    private void enterEdit()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.FromMainToEdit);
    }
}