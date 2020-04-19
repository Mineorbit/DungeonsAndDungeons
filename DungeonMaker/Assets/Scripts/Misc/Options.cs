using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    Button backButton;
    public void Setup()
    {
        backButton=transform.Find("Close").GetComponent<Button>();
        backButton.onClick.AddListener(close);
    }
    void  close()
    {
        UIManager.current.closeOption();
    }
    
}
