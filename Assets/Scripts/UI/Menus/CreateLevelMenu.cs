using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine.UI;

public class CreateLevelMenu : MenuPage
{
    private InputField inputNameField;
    private InputField inputDescriptionField;
    private Button startCreateButton;

    public void Awake()
    {
        base.Awake();
        index = 4;
        setupUI();
    }

    private void setupUI()
    {
        var Interface = transform.Find("Interface");
        startCreateButton = Interface.Find("StartCreateLevel").GetComponent<Button>();
        startCreateButton.onClick.AddListener(GoNewLevel);
        inputNameField = Interface.Find("Name").GetComponent<InputField>();
        inputDescriptionField = Interface.Find("Description").GetComponent<InputField>();
    }

    private void GoNewLevel()
    {
        var name = inputNameField.text;
        var description = inputDescriptionField.text;
        //Do some checks
        var levelMetaData = LevelDataManager.GetNewLevelMetaData();
        levelMetaData.FullName = name;
        levelMetaData.Description = description;
        GameManager.instance.createLevel(levelMetaData);
    }
}