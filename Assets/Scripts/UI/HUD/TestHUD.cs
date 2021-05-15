using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestHUD : MonoBehaviour
{
    public Button enterEdit;
    void Start()
    {
        enterEdit.onClick.AddListener(EnterEdit);
    }
    void EnterEdit()
    {
        GameManager.instance.performAction(GameManager.EnterEditFromTest);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            enterEdit.onClick.Invoke();
        }
    }
}