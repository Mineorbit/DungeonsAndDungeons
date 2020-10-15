using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuilderHUD : MonoBehaviour
{
    Button enterTest;
    void Start()
    {
        enterTest = transform.Find("Test").GetComponent<Button>();
        enterTest.onClick.AddListener(EnterTest);
    }
    void EnterTest()
    {
        GameManager.instance.performAction(GameManager.GameAction.EnterTestFromEdit);
    }

}
