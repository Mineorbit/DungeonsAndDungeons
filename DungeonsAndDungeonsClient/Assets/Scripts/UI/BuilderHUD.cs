using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuilderHUD : MonoBehaviour
{
    Button enterTest;
    Button saveButton;
    public UnityEngine.Object selectorPrefab;
    public LevelObjectData[] dataObjects;
    void Start()
    {
        enterTest = transform.Find("Test").GetComponent<Button>();
        saveButton = transform.Find("Save").GetComponent<Button>();

        enterTest.onClick.AddListener(EnterTest);
        saveButton.onClick.AddListener(SaveLevel);
        SetupList();
    }
    void SaveLevel()
    {
        Level.Save();
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
        }else
        if(Input.GetKeyDown(KeyCode.S))
        {
            saveButton.onClick.Invoke();
        }
    }
    void SetupList()
    {
        dataObjects = Resources.LoadAll<LevelObjectData>("pref/level/data"); 
        foreach(LevelObjectData d in dataObjects)
        {
            GameObject selObject = Instantiate(selectorPrefab,transform.Find("LevelTypeBar")) as GameObject;
            selObject.GetComponent<LevelObjectDataSelector>().SetData(d);
        }
    }
}
