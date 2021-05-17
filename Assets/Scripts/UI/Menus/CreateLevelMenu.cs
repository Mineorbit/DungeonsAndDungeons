using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine.UI;

public class CreateLevelMenu : MenuPage
{
    private InputField inputField;
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
        inputField = Interface.Find("Name").GetComponent<InputField>();
    }

    private void GoNewLevel()
    {
        var name = inputField.text;

        //Do some checks
        var levelMetaData = LevelDataManager.GetNewLevelMetaData();
        levelMetaData.FullName = name;
        GameManager.instance.createLevel(levelMetaData);
    }
}