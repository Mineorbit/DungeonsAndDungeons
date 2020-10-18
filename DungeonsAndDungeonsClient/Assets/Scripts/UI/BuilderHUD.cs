using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuilderHUD : MonoBehaviour
{
    Button enterTest;
    public UnityEngine.Object selectorPrefab;
    public LevelObjectData[] dataObjects;
    void Start()
    {
        enterTest = transform.Find("Test").GetComponent<Button>();
        enterTest.onClick.AddListener(EnterTest);

        SetupList();
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
        Debug.Log("TestFest");
        dataObjects = Resources.LoadAll<LevelObjectData>("pref/level/data"); 
        foreach(LevelObjectData d in dataObjects)
        {
            GameObject selObject = Instantiate(selectorPrefab,transform.Find("LevelTypeBar")) as GameObject;
            selObject.GetComponent<LevelObjectDataSelector>().SetData(d);
        }
    }
}
