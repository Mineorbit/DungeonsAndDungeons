using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.mineorbit.dungeonsanddungeonscommon;

public class BuilderHUD : MonoBehaviour
{
    Button enterTest;
    Button saveButton;
    void Start()
    {
        enterTest = transform.Find("Test").GetComponent<Button>();
        saveButton = transform.Find("Save").GetComponent<Button>();

        enterTest.onClick.AddListener(EnterTest);
        saveButton.onClick.AddListener(SaveLevel);
    }
    void SaveLevel()
    {
        LevelDataManager.Save();
    }
    void EnterTest()
    {
        GameManager.instance.performAction(GameManager.GameAction.EnterTestFromEdit);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            enterTest.onClick.Invoke();
        }
    }
}
