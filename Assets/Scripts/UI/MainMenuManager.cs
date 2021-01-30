using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuManager : MonoBehaviour
{

    public static MainMenuManager instance;
    public MenuPage[] pages;

    public enum Page {None = -1, Main = 0, Play, Lobby, Edit, Create, Upload };
    public enum Transaction {FromNoneToMain, FromMainToPlay,GoBack, FromMainToEdit,FromPlayToLobby, FromEditToCreateMenu, FromEditToUploadMenu, FromNoneToLobby };


    FSM<Page,Transaction> mainMenuFSM;
    public int currentPage = -1;
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

        if(!GameManager.instance.wonLastGame)
        {
            instance.OpenPage(Transaction.FromNoneToMain);
        }else
        {
            instance.OpenPage(Transaction.FromNoneToLobby);
        }

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
    public void OpenPage(Transaction t)
    {
        mainMenuFSM.Move(t);
    }
    //States: 0 Init, 1 MainMenu
    public void setupMainMenu()
    {
        UnityEngine.Debug.Log("MainMenu Controls Setup");
        pages = GameObject.FindObjectsOfType<MenuPage>();
        sortPages();

        System.Action<Transaction> act = x =>
        {
            if( currentPage >= 0)
            pages[currentPage].Close();
            pages[(int) mainMenuFSM.state].Open();
            currentPage = (int) mainMenuFSM.state;
        };

        System.Action<Transaction> actLobbyClose = x =>
        {
            if (currentPage >= 0)
                pages[currentPage].Close();

            UnityEvent onDisconnectEvent = new UnityEvent();


            NetworkManager.instance.GameDisconnect(onDisconnectEvent);

            pages[(int)mainMenuFSM.state].Open();
            currentPage = (int)mainMenuFSM.state;

        };
        System.Action<Transaction> actWin = x =>
        {
            if (currentPage >= 0)
                pages[currentPage].Close();
            pages[(int)mainMenuFSM.state].Open();
            currentPage = (int)mainMenuFSM.state;
            LobbyMenu.UpdateDisplay();
        };



        currentPage = -1;
        mainMenuFSM = new FSM<Page,Transaction>();
        mainMenuFSM.name = "MainMenu";
        mainMenuFSM.state = Page.None;
        mainMenuFSM.transitions.Add(new Tuple<Page,Transaction>(Page.None,Transaction.FromNoneToMain), new Tuple<Action<Transaction>,Page>(act,Page.Main));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Page.None, Transaction.FromNoneToLobby), new Tuple<Action<Transaction>, Page>(actWin, Page.Lobby));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Page.Main, Transaction.FromMainToPlay), new Tuple<Action<Transaction>, Page>(act, Page.Play));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Page.Play, Transaction.GoBack), new Tuple<Action<Transaction>, Page>(act, Page.Main));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Page.Play, Transaction.FromPlayToLobby), new Tuple<Action<Transaction>, Page>(act, Page.Lobby));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Page.Lobby, Transaction.GoBack), new Tuple<Action<Transaction>, Page>(actLobbyClose, Page.Play));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Page.Main, Transaction.FromMainToEdit), new Tuple<Action<Transaction>, Page>(act, Page.Edit));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Page.Edit, Transaction.FromEditToCreateMenu), new Tuple<Action<Transaction>, Page>(act, Page.Create));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Page.Create, Transaction.GoBack), new Tuple<Action<Transaction>, Page>(act, Page.Edit));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Page.Edit, Transaction.GoBack), new Tuple<Action<Transaction>, Page>(act, Page.Main));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Page.Edit, Transaction.FromEditToUploadMenu), new Tuple<Action<Transaction>, Page>(act, Page.Upload));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Page.Upload, Transaction.GoBack), new Tuple<Action<Transaction>, Page>(act, Page.Edit));

    }

}
