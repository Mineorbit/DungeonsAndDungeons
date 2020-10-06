using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuManager : MonoBehaviour
{

    public static MainMenuManager instance;
    public MenuPage[] pages;
    FSM mainMenuFSM;
    public int currentPage = 0;
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
        MouseStateController.UnlockBlocking();
        setupMainMenu();
        OpenPage(0);
    }
    void OnDestroy()
    {
        MouseStateController.LockUnblocking();
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
    public void OpenPage(int n)
    {
        mainMenuFSM.Move(n);
    }
    //States: 0 Init, 1 MainMenu
    public void setupMainMenu()
    {
        UnityEngine.Debug.Log("MainMenu Controls Setup");
        pages = GameObject.FindObjectsOfType<MenuPage>();
        sortPages();

        int[] mainTable = { 0, 1 ,3,3 , 4};
        int[] playTable = { 0, 2, 2, 3 , 4 };
        int[] lobbyTable = { 0, 2, 2, 3 ,4 };
        int[] editTable = { 0, 0, 2, 3, 4 };
        int[] createTable = { 0,0,0,0,0 };
        int[][] menuTransition = {mainTable,playTable,lobbyTable,editTable, createTable };
        System.Action<int>[] stateTable =
        {
            x => {
            pages[currentPage].Close();
            //Open MainMenu
            pages[x].Open();
            currentPage = x;
            },
            x => {
            pages[currentPage].Close();
            //Open 

            pages[x].Open();
            currentPage = x;
            },
            x => {
            pages[currentPage].Close();
                //Open 
                pages[x].Open();
            currentPage = x;
            },
            x => {
            pages[currentPage].Close();
                pages[x].Open();
            currentPage = x;
            },
            x => {
            pages[currentPage].Close();
                pages[x].Open();
            currentPage = x;
            }

        };
        mainMenuFSM = new FSM("MainMenuFSM",menuTransition,stateTable);
    }

}
