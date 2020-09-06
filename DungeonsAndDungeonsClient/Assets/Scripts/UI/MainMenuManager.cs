using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        mainMenuFSM.Move(0);
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
    //States: 0 Init, 1 MainMenu
    public void setupMainMenu()
    {
        UnityEngine.Debug.Log("MainMenu Controls Setup");
        pages = GameObject.FindObjectsOfType<MenuPage>();
        sortPages();

        int[] initTable = { 0, 1 };
        int[] mainTable = { 2, 3 };

        int[][] menuTransition = {initTable,mainTable};
        System.Action<int>[] stateTable =
        {
            x => {
            //Open MainMenu
            pages[0].Open();
            },
            x => { }
        };
        mainMenuFSM = new FSM("MainMenuFSM",menuTransition,stateTable);
    }

}
