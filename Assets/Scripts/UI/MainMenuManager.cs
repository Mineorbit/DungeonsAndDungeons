using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using com.mineorbit.dungeonsanddungeonscommon;

public class MainMenuManager : MonoBehaviour
{

    public static MainMenuManager instance;
    public MenuPage[] pages;

    public class Page : CustomEnum
    {
        public Page(int card) : base(card)
        {
            cardinal = card;
        }
    }

    public static Page None = new Page(-1);
    public static Page Main = new Page(0);
    public static Page Play = new Page(1);
    public static Page Lobby = new Page(2);
    public static Page Edit = new Page(3);
    public static Page Create = new Page(4);
    public static Page Upload = new Page(4);

    public class Transaction : CustomEnum
    {
        public Transaction(int card) : base(card)
        {
            cardinal = card;
        }
    }

    public static Transaction FromNoneToMain = new Transaction(0);
    public static Transaction FromMainToPlay = new Transaction(1);
    public static Transaction GoBack = new Transaction(2);
    public static Transaction FromMainToEdit = new Transaction(3);
    public static Transaction FromPlayToLobby = new Transaction(4);
    public static Transaction FromEditToCreateMenu = new Transaction(5);
    public static Transaction FromEditToUploadMenu = new Transaction(6);
    public static Transaction FromNoneToLobby = new Transaction(7);

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
        SetupMainMenuFSM();

        if(!GameManager.instance.wonLastGame)
        {
            instance.OpenPage(FromNoneToMain);
        }else
        {
            instance.OpenPage(FromNoneToLobby);
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
    public void SetupMainMenuFSM()
    {




    UnityEngine.Debug.Log("MainMenu FSM Setup");
            
    None = new Page(-1);
    Main = new Page(0);
    Play = new Page(1);
    Lobby = new Page(2);
    Edit = new Page(3);
    Create = new Page(4);
    Upload = new Page(4);

    FromNoneToMain = new Transaction(0);
    FromMainToPlay = new Transaction(1);
    GoBack = new Transaction(2);
    FromMainToEdit = new Transaction(3);
    FromPlayToLobby = new Transaction(4);
    FromEditToCreateMenu = new Transaction(5);
    FromEditToUploadMenu = new Transaction(6);
    FromNoneToLobby = new Transaction(7);



    pages = GameObject.FindObjectsOfType<MenuPage>();
        sortPages();

        System.Action<Transaction> act = x =>
        {
            if( currentPage >= 0)
            pages[currentPage].Close();
            pages[mainMenuFSM.state.Cardinal()].Open();
            currentPage = mainMenuFSM.state.Cardinal();
        };

        System.Action<Transaction> actLobbyClose = x =>
        {
            if (currentPage >= 0)
                pages[currentPage].Close();

            UnityEvent onDisconnectEvent = new UnityEvent();


            //NetworkManager.instance.GameDisconnect(onDisconnectEvent);

            pages[mainMenuFSM.state.Cardinal()].Open();
            currentPage = mainMenuFSM.state.Cardinal();

        };
        System.Action<Transaction> actWin = x =>
        {
            if (currentPage >= 0)
                pages[currentPage].Close();
            pages[mainMenuFSM.state.Cardinal()].Open();
            currentPage = mainMenuFSM.state.Cardinal();
            LobbyMenu.UpdateDisplay();
        };



        currentPage = -1;
        mainMenuFSM = new FSM<Page,Transaction>();
        mainMenuFSM.name = "MainMenu";
        mainMenuFSM.state = None;
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(None, FromNoneToMain), new Tuple<Action<Transaction>,Page>(act, Main));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(None, FromNoneToLobby), new Tuple<Action<Transaction>, Page>(actWin, Lobby));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Main, FromMainToPlay), new Tuple<Action<Transaction>, Page>(act, Play));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Play, GoBack), new Tuple<Action<Transaction>, Page>(act, Main));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Play, FromPlayToLobby), new Tuple<Action<Transaction>, Page>(act, Lobby));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Lobby, GoBack), new Tuple<Action<Transaction>, Page>(actLobbyClose, Play));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Main, FromMainToEdit), new Tuple<Action<Transaction>, Page>(act, Edit));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Edit, FromEditToCreateMenu), new Tuple<Action<Transaction>, Page>(act, Create));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Create, GoBack), new Tuple<Action<Transaction>, Page>(act, Edit));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Edit, GoBack), new Tuple<Action<Transaction>, Page>(act, Main));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Edit, FromEditToUploadMenu), new Tuple<Action<Transaction>, Page>(act, Upload));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Upload, GoBack), new Tuple<Action<Transaction>, Page>(act, Edit));

    }

}
