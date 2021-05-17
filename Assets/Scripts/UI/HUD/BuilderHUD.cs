using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;
using UnityEngine.UI;

public class BuilderHUD : MonoBehaviour
{
    private Button enterTest;
    private Button saveButton;

    private void Start()
    {
        enterTest = transform.Find("Test").GetComponent<Button>();
        saveButton = transform.Find("Save").GetComponent<Button>();

        enterTest.onClick.AddListener(EnterTest);
        saveButton.onClick.AddListener(SaveLevel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) enterTest.onClick.Invoke();
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)) saveButton.onClick.Invoke();
    }


    private void SaveLevel()
    {
        LevelDataManager.Save();
    }

    private void EnterTest()
    {
        GameManager.instance.performAction(GameManager.EnterTestFromEdit);
    }
}