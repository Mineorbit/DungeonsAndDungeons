using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;
    MenuPage[] pages;
    FSM mainMenuFSM;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }
    void Start()
    {
    }
    public void setupMainMenu()
    {
        UnityEngine.Debug.Log("MainMenu Controls Setup");
        pages = GameObject.FindObjectsOfType<MenuPage>();

    }
    
}
