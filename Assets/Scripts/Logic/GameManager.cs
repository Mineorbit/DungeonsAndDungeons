using System;
using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public static State Init = new State("Init", 0);
    public static State MainMenu = new State("Main Menu", 1);
    public static State Play = new State("Play", 2);
    public static State Edit = new State("Edit", 3);
    public static State Test = new State("Test", 4);
    public static State PrepareGame = new State("PrepareGame", 5);

    public static GameAction LoadGameFromBoot = new GameAction("LoadGameFromBoot", 0);
    public static GameAction Reset = new GameAction("Reset", 1);
    public static GameAction EnterMainMenu = new GameAction("EnterMainMenu", 2);
    public static GameAction BackToLobbyAfterWin = new GameAction("BackToLobbyAfterWin", 3);
    public static GameAction EnterTestFromMainMenu = new GameAction("EnterTestFromMainMenu", 4);
    public static GameAction EnterEditFromMainMenuNewLevel = new GameAction("EnterEditFromMainMenuNewLevel", 5);
    public static GameAction EnterEditFromMainMenu = new GameAction("EnterEditFromMainMenu", 6);
    public static GameAction EnterTestFromEdit = new GameAction("EnterTestFromEdit", 7);
    public static GameAction EnterEditFromTest = new GameAction("EnterEditFromTest", 8);
    public static GameAction StartPlay = new GameAction("StartPlay", 9);
    public static GameAction PrepareGameFromMainMenu = new GameAction("PrepareGameFromMainMenu", 10);

    public bool wonLastGame;


    public NetLevel.LevelMetaData levelMetaDataForNewLevel;

    public NetLevel.LevelMetaData levelMetaDataForEditLevel;

    private UnityEvent[] asyncEvent;
    public Logic currentLogic;


    private FSM<State, GameAction> gameStateFSM;
    private bool instanceSet;

    public void Start()
    {
        Setup();

        NetworkManager.prepareRoundEvent.AddListener(() => { performAction(PrepareGameFromMainMenu); });
        NetworkManager.startRoundEvent.AddListener(() => { performAction(StartPlay); });
        NetworkManager.disconnectEvent.AddListener(() => { performAction(EnterMainMenu); });
        NetworkManager.winEvent.AddListener(() => { performAction(BackToLobbyAfterWin); });
        performAction(LoadGameFromBoot);
    }

    //this is ugly need better way
    private void Update()
    {
        if (currentLogic != null)
            if (currentLogic.GetType() == typeof(TestLogic))
                (currentLogic as TestLogic).Update();
    }

    public static State GetState()
    {
        return instance.gameStateFSM.state;
    }

    private void Setup()
    {
        if (instance != null && !instanceSet) Destroy(this);
        instance = this;
        if (!instanceSet) instanceSet = true;


        SetupGameStateFSM();
    }


    private void SetLogic()
    {
        UpdateLogic();
        if (currentLogic != null)
            if (currentLogic.running == false)
                currentLogic.Start();
    }


    public void SetupGameStateFSM()
    {
        //GameConsole.Log(Init.ToString());
        gameStateFSM = new FSM<State, GameAction>();
        gameStateFSM.state = Init;
        gameStateFSM.name = "GameState";

        Action<GameAction> fromInitToMainMenu = x =>
        {
            wonLastGame = false;
            var initEvent = new UnityEvent();


            Level.instantiateType = Level.InstantiateType.Default;

            var menuLoadFinishedEvent = new UnityEvent();
           // MainMenuManager.startingAction = MainMenuManager.FromNoneToMain;
            menuLoadFinishedEvent.AddListener(() => { MainCaller.Do(() => { LoadingScreen.instance.Close(); }); });
            initEvent.AddListener(() =>
            {
                SceneLoadManager.instance.load(SceneLoadManager.SceneIndex.menu, menuLoadFinishedEvent);
            });

            SetLogic();


            LoadingScreen.instance.openEvent = initEvent;
            LoadingScreen.instance.Open();
        };

        Action<GameAction> fromMainMenuToEdit = x =>
        {
            wonLastGame = false;
            var initEvent = new UnityEvent();


            var editLoadFinishedEvent = new UnityEvent();
            editLoadFinishedEvent.AddListener(LoadingScreen.instance.Close);
            initEvent.AddListener(() =>
            {
                SceneLoadManager.instance.unloadCurrentScenes();
                Debug.Log("Loading " + levelMetaDataForEditLevel.FullName);
                LevelDataManager.Load(levelMetaDataForEditLevel, Level.InstantiateType.Edit);
                SceneLoadManager.instance.load(
                    new[] {SceneLoadManager.SceneIndex.edit, SceneLoadManager.SceneIndex.test}, editLoadFinishedEvent);
            });

            SetLogic();

            LoadingScreen.instance.openEvent = initEvent;
            LoadingScreen.instance.Open();
        };

        Action<GameAction> fromMainMenuToEditNewLevel = x =>
        {
            wonLastGame = false;
            var initEvent = new UnityEvent();


            Level.instantiateType = Level.InstantiateType.Edit;

            var editLoadFinishedEvent = new UnityEvent();
            editLoadFinishedEvent.AddListener(LoadingScreen.instance.Close);
            initEvent.AddListener(() =>
            {
                SceneLoadManager.instance.unloadCurrentScenes();
                LevelDataManager.New(levelMetaDataForNewLevel);


                SceneLoadManager.instance.load(
                    new[] {SceneLoadManager.SceneIndex.edit, SceneLoadManager.SceneIndex.test}, editLoadFinishedEvent);
            });

            SetLogic();

            LoadingScreen.instance.openEvent = initEvent;
            LoadingScreen.instance.Open();
        };

        Action<GameAction> fromEditToMainMenu = x =>
        {
            // Ask for safe?

            wonLastGame = false;
            var initEvent = new UnityEvent();

            Level.instantiateType = Level.InstantiateType.Default;
            MainMenuManager.startingAction = MainMenuManager.FromNoneToMain;

            var menuLoadFinishedEvent = new UnityEvent();
            menuLoadFinishedEvent.AddListener(LoadingScreen.instance.Close);
            MainMenuManager.startingAction = MainMenuManager.FromNoneToMain;
            initEvent.AddListener(() =>
            {
                SceneLoadManager.instance.unloadCurrentScenes();
                LevelManager.Clear();
                SceneLoadManager.instance.load(SceneLoadManager.SceneIndex.menu, menuLoadFinishedEvent);
            });

            SetLogic();

            LoadingScreen.instance.openEvent = initEvent;
            LoadingScreen.instance.Open();
        };

        Action<GameAction> fromTestToMainMenu = x =>
        {
            // Ask for save?

            Level.instantiateType = Level.InstantiateType.Default;

            wonLastGame = false;
            var initEvent = new UnityEvent();
            MainMenuManager.startingAction = MainMenuManager.FromNoneToMain;

            var menuLoadFinishedEvent = new UnityEvent();
            menuLoadFinishedEvent.AddListener(LoadingScreen.instance.Close);
            MainMenuManager.startingAction = MainMenuManager.FromNoneToMain;
            initEvent.AddListener(() =>
            {
                SceneLoadManager.instance.unloadCurrentScenes();
                LevelManager.Clear();
                SceneLoadManager.instance.load(SceneLoadManager.SceneIndex.menu, menuLoadFinishedEvent);
            });

            SetLogic();

            LoadingScreen.instance.openEvent = initEvent;
            LoadingScreen.instance.Open();
        };


        Action<GameAction> fromEditToTest = x =>
        {
            Level.instantiateType = Level.InstantiateType.Test;

            SceneLoadManager.SetSceneState(SceneLoadManager.SceneIndex.edit, false);
            SceneLoadManager.SetSceneState(SceneLoadManager.SceneIndex.test, true);
            SetLogic();
        };

        Action<GameAction> fromTestToEdit = x =>
        {
            LevelManager.ResetDynamicState();

            Level.instantiateType = Level.InstantiateType.Edit;
            SceneLoadManager.SetSceneState(SceneLoadManager.SceneIndex.edit, true);
            SceneLoadManager.SetSceneState(SceneLoadManager.SceneIndex.test, false);
            SetLogic();
        };

        Action<GameAction> actResetDisconnect = x =>
        {
            wonLastGame = false;
            var connectionResetEvent = new UnityEvent();

            //NetworkManager.instance.Reset();


            LoadingScreen.instance.openEvent = connectionResetEvent;
            LoadingScreen.instance.Open();
        };


        Action<GameAction> actPrepareGame = x =>
        {
            Level.instantiateType = Level.InstantiateType.Online;

            var initEvent = new UnityEvent();
            initEvent.AddListener(() =>
            {
                SceneLoadManager.instance.unloadCurrentScenes();
                SceneLoadManager.instance.load(SceneLoadManager.SceneIndex.play);
            });

            LoadingScreen.instance.openEvent = initEvent;

            LoadingScreen.instance.Open();
        };

        Action<GameAction> actStartGame = x =>
        {
            wonLastGame = false;
            Debug.LogError("Starte Runde in State " + GetState());
            SetLogic();

            LoadingScreen.instance.Close();
        };
        
        
        Action<GameAction> actDropGame = x =>
        {
            wonLastGame = false;
            var initEvent = new UnityEvent();


            var menuLoadFinishedEvent = new UnityEvent();
            menuLoadFinishedEvent.AddListener(LoadingScreen.instance.Close);
            MainMenuManager.startingAction = MainMenuManager.FromNoneToMain;
            initEvent.AddListener(() =>
            {
                SceneLoadManager.instance.unloadCurrentScenes();
                LevelManager.Clear();
                PlayerManager.playerManager.Reset();
                NetworkManager.instance.Disconnect();

                SetLogic();
                SceneLoadManager.instance.load(SceneLoadManager.SceneIndex.menu, menuLoadFinishedEvent);
            });

            LoadingScreen.instance.openEvent = initEvent;


            MainCaller.Do(() =>
            {
                LoadingScreen.instance.Open();
            });
        };

        Action<GameAction> actCloseGame = x =>
        {
            wonLastGame = false;
            var initEvent = new UnityEvent();


            var menuLoadFinishedEvent = new UnityEvent();
            menuLoadFinishedEvent.AddListener(LoadingScreen.instance.Close);
            MainMenuManager.startingAction = MainMenuManager.FromNoneToLobby;
            initEvent.AddListener(() =>
            {
                SceneLoadManager.instance.unloadCurrentScenes();
                LevelManager.Clear();
                PlayerManager.playerManager.Reset();
                NetworkManager.instance.Disconnect();

                SetLogic();
                SceneLoadManager.instance.load(SceneLoadManager.SceneIndex.menu, menuLoadFinishedEvent);
            });

            LoadingScreen.instance.openEvent = initEvent;


            MainCaller.Do(() =>
            {
                LoadingScreen.instance.Open();
            });
        };

        Action<GameAction> actWinGame = x =>
        {
            wonLastGame = true;
            var initEvent = new UnityEvent();


            var menuLoadFinishedEvent = new UnityEvent();
            menuLoadFinishedEvent.AddListener(LoadingScreen.instance.Close);
            MainMenuManager.startingAction = MainMenuManager.FromNoneToWin;
            
            initEvent.AddListener(() =>
            {
                SceneLoadManager.instance.unloadCurrentScenes();
                LevelManager.Clear();

                SetLogic();
                SceneLoadManager.instance.load(SceneLoadManager.SceneIndex.menu, menuLoadFinishedEvent);
            });

            LoadingScreen.instance.openEvent = initEvent;


            MainCaller.Do(() =>
            {
                LoadingScreen.instance.Open();
            });
        };

        Action<GameAction> nop = x => { };

        

        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(Init, LoadGameFromBoot),
            new Tuple<Action<GameAction>, State>(fromInitToMainMenu, MainMenu));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(MainMenu, Reset),
            new Tuple<Action<GameAction>, State>(actResetDisconnect, MainMenu));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(MainMenu, EnterMainMenu),
            new Tuple<Action<GameAction>, State>(nop, MainMenu));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(MainMenu, EnterTestFromMainMenu),
            new Tuple<Action<GameAction>, State>(fromMainMenuToEdit, Test));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(MainMenu, EnterEditFromMainMenu),
            new Tuple<Action<GameAction>, State>(fromMainMenuToEdit, Edit));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(MainMenu, EnterEditFromMainMenuNewLevel),
            new Tuple<Action<GameAction>, State>(fromMainMenuToEditNewLevel, Edit));


        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(MainMenu, PrepareGameFromMainMenu),
            new Tuple<Action<GameAction>, State>(actPrepareGame, PrepareGame));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(PrepareGame, StartPlay),
            new Tuple<Action<GameAction>, State>(actStartGame, Play));

        //Hier evtl mod
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(Play, EnterMainMenu),
            new Tuple<Action<GameAction>, State>(actDropGame, MainMenu));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(Play, BackToLobbyAfterWin),
            new Tuple<Action<GameAction>, State>(actWinGame, MainMenu));


        //Reset Level
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(Edit, EnterMainMenu),
            new Tuple<Action<GameAction>, State>(fromEditToMainMenu, MainMenu));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(Test, EnterMainMenu),
            new Tuple<Action<GameAction>, State>(fromTestToMainMenu, MainMenu));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(Edit, EnterTestFromEdit),
            new Tuple<Action<GameAction>, State>(fromEditToTest, Test));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(Test, EnterEditFromTest),
            new Tuple<Action<GameAction>, State>(fromTestToEdit, Edit));
    }


    public void performAction(GameAction action)
    {
        Debug.Log("GameManager: " + action);
        gameStateFSM.Move(action);
    }

    public void createLevel(NetLevel.LevelMetaData newLevelData)
    {
        levelMetaDataForNewLevel = newLevelData;
        performAction(EnterEditFromMainMenuNewLevel);
    }

    public void editLevel(NetLevel.LevelMetaData data)
    {
        levelMetaDataForEditLevel = data;
        performAction(EnterEditFromMainMenu);
    }

    //Disgusting but no better option really with custom enums

    public void UpdateLogic()
    {
        if (currentLogic != null)
        {
            if (currentLogic.running) currentLogic.Stop();
            currentLogic.DeInit();
        }


        if (GetState() == MainMenu)
            currentLogic = new LobbyLogic();
        else if (GetState() == Edit)
            currentLogic = new EditLogic();
        else if (GetState() == Test)
            currentLogic = new TestLogic();
        else if (GetState() == Play)
            currentLogic = new PlayLogic();
        else if (GetState() == Init)
            currentLogic = null;

        if (currentLogic != null) currentLogic.Init();
    }

    public class State : CustomEnum
    {
        public State(string val, int card) : base(val, card)
        {
            Value = val;
            cardinal = card;
        }
    }

    public class GameAction : CustomEnum
    {
        public GameAction(int card) : base(card)
        {
            cardinal = card;
        }

        public GameAction(string val, int card) : base(val, card)
        {
            Value = val;
            cardinal = card;
        }
    }
}