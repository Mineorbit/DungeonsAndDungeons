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
        }
    }
    void SetupList()
    {
        Transform hook = transform.Find("LevelTypeBar").Find("Viewport").Find("Content");
        dataObjects = Resources.LoadAll<LevelObjectData>("pref/level/data"); 
        foreach(LevelObjectData d in dataObjects)
        {
            GameObject selObject = Instantiate(selectorPrefab,hook) as GameObject;
            selObject.GetComponent<LevelObjectDataSelector>().SetData(d);
        }
    }
}
