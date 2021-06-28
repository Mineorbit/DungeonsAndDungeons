using System;
using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    public static Page None = new Page("None",-1);
    public static Page Main = new Page("Main",0);
    public static Page Play = new Page("Play",1);
    public static Page Lobby = new Page("Lobby",2);
    public static Page Edit = new Page("Edit",3);
    public static Page Create = new Page("Create",4);
    public static Page Upload = new Page("Upload",5);
    public static Page Win = new Page("Win",6);

    public static Transaction FromNoneToMain = new Transaction("NoneToMain",0);
    public static Transaction FromMainToPlay = new Transaction("MainToPlay",1);
    public static Transaction GoBack = new Transaction("GoBack",2);
    public static Transaction FromMainToEdit = new Transaction("MainToEdit",3);
    public static Transaction FromPlayToLobby = new Transaction("PlayToLobby",4);
    public static Transaction FromEditToCreateMenu = new Transaction("EditToCreate",5);
    public static Transaction FromEditToUploadMenu = new Transaction("EditToUpload",6);
    public static Transaction FromNoneToLobby = new Transaction("NoneToLobby",7);
    public static Transaction FromNoneToWin = new Transaction("NoneToWin",8);
    public static Transaction FromWinToLobby = new Transaction("WinToLobby",9);
    public MenuPage[] pages;
    public int currentPage = -1;

    private FSM<Page, Transaction> mainMenuFSM;

    private void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
        SetupMainMenuFSM();
    }

    public static Transaction startingAction = FromNoneToMain;
    private void Start()
    {
        MouseStateController.UnlockBlocking();
        Debug.Log("Doing "+startingAction.cardinal);
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


        pages = FindObjectsOfType<MenuPage>();
        sortPages();

        LevelDataManager.levelListUpdatedEvent.AddListener(LevelList.UpdateDisplay);
        
        Action<Transaction> act = x =>
        {
            UnityEvent e = new UnityEvent();
            e.AddListener(() =>
            {
                LevelDataManager.instance.networkLevels = HttpManager.instance.levelMetaDatas;
                LevelList.UpdateDisplay();
            });
            HttpManager.FetchLevelList(e);
            
            
            LevelList.UpdateDisplay();
            
            if (currentPage >= 0)
                pages[currentPage].Close();
            Debug.Log("Opening Page: "+mainMenuFSM.state.Cardinal());
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

            NetworkManager.instance.client.onDisconnectEvent.AddListener(() =>
            {
                OpenPage(GoBack);
                ((LobbyLogic) GameManager.instance.currentLogic).OnLobbyOpen();
            });
            LobbyMenu.UpdateDisplay();
        };


        Action<Transaction> actLobbyOpen = x =>
        {
            if (currentPage >= 0)
                pages[currentPage].Close();

            NetworkManager.instance.client.onDisconnectEvent.AddListener(() =>
            {
                MainCaller.Do(() =>
                {
                    OpenPage(GoBack); 
                    
                    ((LobbyLogic) GameManager.instance.currentLogic).OnLobbyOpen();
                });
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
            new Tuple<Action<Transaction>, Page>(actLobbyOpen, Lobby));
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(None, FromNoneToWin),
            new Tuple<Action<Transaction>, Page>(actWin, Win));
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
        mainMenuFSM.transitions.Add(new Tuple<Page, Transaction>(Win, FromWinToLobby),
            new Tuple<Action<Transaction>, Page>(act, Lobby));
        
        Debug.Log("SIZE : "+mainMenuFSM.transitions.Count);
    }

    public class Page : CustomEnum
    {
        public Page(string val, int card) : base(val,card)
        {
            Value = val;
            cardinal = card;
        }

        public override string ToString()
        {
            return cardinal.ToString()+" "+Value;
        }
    }

    public class Transaction : CustomEnum
    {
        public Transaction(string val, int card) : base(val, card)
        {
            Value = val;
            cardinal = card;
        }
        public override string ToString()
        {
            return cardinal.ToString()+" "+Value;
        }
    }
}