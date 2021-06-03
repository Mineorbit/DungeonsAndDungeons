using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine.UI;

public class EditMenu : MenuPage
{
    public Switcher switcher;
    private Button deleteButton;
    private Button editButton;
    private LevelList currentLevelList;
    private Button newLevelButton;
    private Button uploadButton;

    public override void Awake()
    {
        base.Awake();
        index = 3;
        setupUI();
        switcher.valueChanged.AddListener((x) => { currentLevelList = switcher.selected.GetComponent<LevelList>();});
    }


    public override void Start()
    {
        base.Start();
    }

    private void setupUI()
    {
        var Interface = transform.Find("Interface");
        editButton = Interface.Find("Edit").GetComponent<Button>();
        newLevelButton = Interface.Find("New").GetComponent<Button>();
        deleteButton = Interface.Find("Delete").GetComponent<Button>();
        uploadButton = Interface.Find("Upload").GetComponent<Button>();
        uploadButton.onClick.AddListener(OpenUploadMenu);
        newLevelButton.onClick.AddListener(OpenNewLevelMenu);
        editButton.onClick.AddListener(StartEdit);
        deleteButton.onClick.AddListener(RemoveLevel);
    }

    private void RemoveLevel()
    {
        var metaData = currentLevelList.GetSelectedLevel();
        if (metaData == null) return;
        LevelDataManager.Delete(metaData);
    }

    private void OpenUploadMenu()
    {
        var metaData = currentLevelList.GetSelectedLevel();
        if (metaData != null)
        {
            UploadMenu.levelToUpload = metaData;
            MainMenuManager.instance.OpenPage(MainMenuManager.FromEditToUploadMenu);
        }
    }

    private void OpenNewLevelMenu()
    {
        MainMenuManager.instance.OpenPage(MainMenuManager.FromEditToCreateMenu);
    }

    private void StartEdit()
    {
        var metaData = currentLevelList.GetSelectedLevel();
        if (metaData == null) return;
        GameManager.instance.editLevel(metaData);
    }
}