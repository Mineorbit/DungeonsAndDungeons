using System;
using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    public static Page None = new Page(-1);
    public static Page Main = new Page(0);
    public static Page Play = new Page(1);
    public static Page Lobby = new Page(2);
    public static Page Edit = new Page(3);
    public static Page Create = new Page(4);
    public static Page Upload = new Page(4);

    public static Transaction FromNoneToMain = new Transaction(0);
    public static Transaction FromMainToPlay = new Transaction(1);
    public static Transaction GoBack = new Transaction(2);
    public static Transaction FromMainToEdit = new Transaction(3);
    public static Transaction FromPlayToLobby = new Transaction(4);
    public static Transaction FromEditToCreateMenu = new Transaction(5);
    public static Transaction FromEditToUploadMenu = new Transaction(6);
    public static Transaction FromNoneToLobby = new Transaction(7);
    public MenuPage[] pages;
    public int currentPage = -1;

    private FSM<Page, Transaction> mainMenuFSM;

    private void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
    }

    public static Transaction startingAction = FromNoneToMain;
    private void Start()
    {
        MouseStateController.UnlockBlocking();
        SetupMainMenuFSM();

        instance.OpenPage(startingAction);

        // May only add once !
    }

    private void OnDestroy()
    {
        MouseStateController.LockUnblocking();
    }

    private void sortPages()
    {
        var arr = pages;
        MenuPage temp;
        for (var j = 0; j <= arr.Length - 2; j++)
        for (var i = 0; i <= arr.Length - 2; i++)
            if (arr[i].index > arr[i + 1].index)
            {
                temp = arr[i + 1];
                arr[i + 1] = arr[i];
                arr[i] = temp;
            }
    }

    public void OpenPage(Transaction t)
    {
        mainMenuFSM.Move(t);
    }

    //States: 0 Init, 1 MainMenu
    public void SetupMainMenuFSM()
    {
        Debug.Log("MainMenu FSM Setup");

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


        pages = FindObjectsOfType<MenuPage>();
        sortPages();

        Action<Transaction> act = x =>
        {
            if (currentPage >= 0)
                pages[currentPage].Close();
            pages[mainMenuFSM.state.Cardinal()].Open();
            currentPage = mainMenuFSM.state.Cardinal();
        };

        Action<Transaction> actLobbyClose = x =>
        {
            if (currentPage >= 0)
                pages[currentPage].Close();

            var onDisconnectEvent = new UnityEvent();


            NetworkManager.instance.Disconnect();

            pages[mainMenuFSM.state.Cardinal()].Open();
            currentPage = mainMenuFSM.state.Cardinal();
        };
        Action<Transaction> actWin = x =>
        {
            if (currentPage >= 0)
                pages[currentPage].Close();
            pages[mainMenuFSM.state.Cardinal()].Open();
            currentPage = mainMenuFSM.state.Cardinal();

            NetworkManager.instance.client.onDisconnectEvent.AddListener(() => { OpenPage(GoBack); });
            LobbyMenu.UpdateDisplay();
        };


        Action<Transaction> actLobbyOpen = x =>
        {
            if (currentPage >= 0)
                pages[currentPage].Close();

            NetworkManager.instance.client.onDisconnectEvent.AddListener(() =>
            {
                MainCaller.Do(() => { OpenPage(GoBack); });
            });

            pages[mainMenuFSM.state.Cardinal()].Open();
            currentPage = mainMenuFSM.state.Cardinal();
        };


        currentPage = -1;
        mainMenuFSM = new FSM<Page, Transaction>();
        mainMenuFSM.name = "MainMenu";
        mainMenuFSM.state = None;
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(None, FromNoneToMain),
            new Tuple<Action<Transaction>, Page>(act, Main));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(None, FromNoneToLobby),
            new Tuple<Action<Transaction>, Page>(actWin, Lobby));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Main, FromMainToPlay),
            new Tuple<Action<Transaction>, Page>(act, Play));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Play, GoBack),
            new Tuple<Action<Transaction>, Page>(act, Main));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Play, FromPlayToLobby),
            new Tuple<Action<Transaction>, Page>(actLobbyOpen, Lobby));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Lobby, GoBack),
            new Tuple<Action<Transaction>, Page>(actLobbyClose, Play));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Main, FromMainToEdit),
            new Tuple<Action<Transaction>, Page>(act, Edit));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Edit, FromEditToCreateMenu),
            new Tuple<Action<Transaction>, Page>(act, Create));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Create, GoBack),
            new Tuple<Action<Transaction>, Page>(act, Edit));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Edit, GoBack),
            new Tuple<Action<Transaction>, Page>(act, Main));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Edit, FromEditToUploadMenu),
            new Tuple<Action<Transaction>, Page>(act, Upload));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Upload, GoBack),
            new Tuple<Action<Transaction>, Page>(act, Edit));
    }

    public class Page : CustomEnum
    {
        public Page(int card) : base(card)
        {
            cardinal = card;
        }
    }

    public class Transaction : CustomEnum
    {
        public Transaction(int card) : base(card)
        {
            cardinal = card;
        }
    }
}