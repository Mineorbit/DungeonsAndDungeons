using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    public static MainMenuManager instance;
    public MenuPage[] pages;
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
        setupMainMenu();
    }
    void sortPages()
    {
        MenuPage[] arr = pages;
        MenuPage temp;
        for (int j = 0; j <= arr.Length - 2; j++)
        {
            for (int i = 0; i <= arr.Length - 2; i++)
            {
                if (arr[i].index > arr[i + 1].index)
                {
                    temp = arr[i + 1];
                    arr[i + 1] = arr[i];
                    arr[i] = temp;
                }
            }
        }
    }
    public void setupMainMenu()
    {
        UnityEngine.Debug.Log("MainMenu Controls Setup");
        pages = GameObject.FindObjectsOfType<MenuPage>();
        sortPages();
        int[][] menuTransition = { {1,2 },{2,1 } };
        mainMenuFSM = new FSM("MainMenuFSM",);
    }

}
