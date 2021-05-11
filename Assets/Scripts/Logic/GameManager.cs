using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using com.mineorbit.dungeonsanddungeonscommon;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    bool instanceSet = false;
    public Logic currentLogic;

    UnityEvent afterMenuLoad;
    UnityEvent afterTestLoad;
    UnityEvent afterEditLoad;
    UnityEvent afterPlayLoad;

    public class State : CustomEnum
    {

        public State(string val, int card) : base(val,card)
        {
            Value = val;
            cardinal = card;
        }
    }


    public static State Init = new State("Init",0);
    public static State MainMenu = new State("Main Menu",1);
    public static State Play = new State("Play",2);
    public static State Edit = new State("Edit",3);
    public static State Test = new State("Test",4);
    public static State PrepareGame = new State("PrepareGame", 5);

    public class GameAction: CustomEnum
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

    public static GameAction LoadGameFromBoot = new GameAction("LoadGameFromBoot", 0);
    public static GameAction Reset = new GameAction("Reset", 1);
    public static GameAction EnterMainMenu = new GameAction("EnterMainMenu",2);
    public static GameAction BackToLobbyAfterWin = new GameAction("BackToLobbyAfterWin", 3);
    public static GameAction EnterTestFromMainMenu = new GameAction("EnterTestFromMainMenu", 4);
    public static GameAction EnterEditFromMainMenuNewLevel = new GameAction("EnterEditFromMainMenuNewLevel",5);
    public static GameAction EnterEditFromMainMenu = new GameAction("EnterEditFromMainMenu", 6);
    public static GameAction EnterTestFromEdit = new GameAction("EnterTestFromEdit", 7);
    public static GameAction EnterEditFromTest = new GameAction("EnterEditFromTest", 8);
    public static GameAction StartPlay = new GameAction("StartPlay", 9);
    public static GameAction PrepareGameFromMainMenu = new GameAction("PrepareGameFromMainMenu", 10);


    FSM<State,GameAction> gameStateFSM;

    UnityEvent[] asyncEvent;

    public bool wonLastGame = false;
    public static State GetState()
    {
        return instance.gameStateFSM.state;
    }
    public void Start()
    {
        Setup();

        NetworkManager.prepareRoundEvent.AddListener(()=> { performAction(PrepareGameFromMainMenu); });
        NetworkManager.startRoundEvent.AddListener(() => { performAction(StartPlay); });

        performAction(LoadGameFromBoot);
    }

    public void ResetGame()
    {
        UnityEngine.Debug.Log("Reseting");
        setupAfterLoadEvents();
    }
    void Setup()
    {
        if (instance != null && !instanceSet) Destroy(this);
        instance = this;
        if (!instanceSet) instanceSet = true;



        setupAfterLoadEvents();
        SetupGameStateFSM();
    }

    public void setupAfterLoadEvents()
    {
        afterMenuLoad = new UnityEvent();
        
        afterMenuLoad.AddListener(SetLogic);
        
        afterMenuLoad.AddListener(LevelDataManager.UpdateLocalLevelList);

        //afterMenuLoad.AddListener(OptionsMenu.HandleSimpleLobbyChange);

        afterMenuLoad.AddListener(LoadingScreen.instance.Close);

        afterTestLoad = new UnityEvent();
        afterTestLoad.AddListener(SetLogic);
        afterTestLoad.AddListener(LevelDataManager.UpdateLocalLevelList);
        afterTestLoad.AddListener(LoadingScreen.instance.Close);

        afterPlayLoad = new UnityEvent();

        afterEditLoad = new UnityEvent();
        afterEditLoad.AddListener(SetLogic);
        afterEditLoad.AddListener(LevelDataManager.UpdateLocalLevelList);
        afterEditLoad.AddListener(LoadingScreen.instance.Close);

    }


    void SetLogic()
    {
        UpdateLogic();
        if(currentLogic!=null)
        {
            if(currentLogic.running==false)
            currentLogic.Start();
        }
    }


    public LevelMetaData levelMetaDataForNewLevel;

    public LevelMetaData levelMetaDataForEditLevel;




    public void SetupGameStateFSM()
    {

    UnityEngine.Debug.Log(Init);
    gameStateFSM = new FSM<State,GameAction>();
    gameStateFSM.state = Init;
    gameStateFSM.name = "GameState";

        Action<GameAction> fromInitToMainMenu = x =>
        {
            wonLastGame = false;
            UnityEvent initEvent = new UnityEvent();


            LevelDataManager.instance.loadType = LevelDataManager.LoadType.Near;

            UnityEvent menuLoadFinishedEvent = new UnityEvent();
            menuLoadFinishedEvent.AddListener(LoadingScreen.instance.Close);
            initEvent.AddListener(()=> {
            SceneLoadManager.instance.load(SceneLoadManager.SceneIndex.menu,menuLoadFinishedEvent);
            });

            SetLogic();


            LoadingScreen.instance.openEvent = initEvent;
            LoadingScreen.instance.Open();
        };

        Action<GameAction> fromMainMenuToEdit = x =>
        {
            wonLastGame = false;
            UnityEvent initEvent = new UnityEvent();

            LevelDataManager.instance.loadType = LevelDataManager.LoadType.All;

            UnityEvent editLoadFinishedEvent = new UnityEvent();
            editLoadFinishedEvent.AddListener(LoadingScreen.instance.Close);
            initEvent.AddListener(() => {
                SceneLoadManager.instance.unloadCurrentScenes();
                UnityEngine.Debug.Log("Loading "+levelMetaDataForEditLevel.FullName);
                LevelDataManager.Load(levelMetaDataForEditLevel);

                SceneLoadManager.instance.load(new SceneLoadManager.SceneIndex[] { SceneLoadManager.SceneIndex.edit, SceneLoadManager.SceneIndex.test }, editLoadFinishedEvent);
            });

            SetLogic();

            LoadingScreen.instance.openEvent = initEvent;
            LoadingScreen.instance.Open();
        };

        Action<GameAction> fromMainMenuToEditNewLevel = x =>
        {
            wonLastGame = false;
            UnityEvent initEvent = new UnityEvent();


            LevelDataManager.instance.loadType = LevelDataManager.LoadType.All;

            UnityEvent editLoadFinishedEvent = new UnityEvent();
            editLoadFinishedEvent.AddListener(LoadingScreen.instance.Close);
            initEvent.AddListener(() => {
                SceneLoadManager.instance.unloadCurrentScenes();
                LevelDataManager.New(levelMetaDataForNewLevel);


                SceneLoadManager.instance.load(new SceneLoadManager.SceneIndex[]{ SceneLoadManager.SceneIndex.edit, SceneLoadManager.SceneIndex.test}, editLoadFinishedEvent);
            });

            SetLogic();

            LoadingScreen.instance.openEvent = initEvent;
            LoadingScreen.instance.Open();
        };

        Action<GameAction> fromEditToMainMenu = x =>
        {
            // Ask for safe?

            wonLastGame = false;
            UnityEvent initEvent = new UnityEvent();

            LevelDataManager.instance.loadType = LevelDataManager.LoadType.Near;

            UnityEvent menuLoadFinishedEvent = new UnityEvent();
            menuLoadFinishedEvent.AddListener(LoadingScreen.instance.Close);
            initEvent.AddListener(() => {
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
            // Ask for safe?

            wonLastGame = false;
            UnityEvent initEvent = new UnityEvent();

            UnityEvent menuLoadFinishedEvent = new UnityEvent();
            menuLoadFinishedEvent.AddListener(LoadingScreen.instance.Close);
            initEvent.AddListener(() => {
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

            SceneLoadManager.SetSceneState(SceneLoadManager.SceneIndex.edit,false);
            SceneLoadManager.SetSceneState(SceneLoadManager.SceneIndex.test, true);
            SetLogic();
        };

        Action<GameAction> fromTestToEdit = x =>
        {
            LevelManager.ResetDynamicState();

            SceneLoadManager.SetSceneState(SceneLoadManager.SceneIndex.edit, true);
            SceneLoadManager.SetSceneState(SceneLoadManager.SceneIndex.test, false);
            SetLogic();
        };

        Action<GameAction> actResetDisconnect = x =>
        {
            wonLastGame = false;
            UnityEvent connectionResetEvent = new UnityEvent();

            //NetworkManager.instance.Reset();

            connectionResetEvent.AddListener(ResetGame);

            LoadingScreen.instance.openEvent = connectionResetEvent;
            LoadingScreen.instance.Open();
        };

        Action<GameAction> actLevelClear = x =>
        {
            wonLastGame = false;
            UnityEvent initEvent = new UnityEvent();
            LevelManager.Clear();
            LoadingScreen.instance.openEvent = initEvent;
            LoadingScreen.instance.Open();
        };


        Action<GameAction> actClearAfterGame = x =>
        {
            wonLastGame = false;
            UnityEvent initEvent = new UnityEvent();

            SetLogic();
            initEvent.AddListener(ResetGame);
            PlayerManager.playerManager.Reset();
            //NetworkManager.instance.Reset();
            UnityEngine.Debug.Log("Leaving lobby");
            LevelManager.Clear();
            LoadingScreen.instance.openEvent = initEvent;
            LoadingScreen.instance.Open();
        };

        Action<GameAction> actPrepareGame = x =>
        {
            LevelManager.Clear();
            UnityEvent initEvent = new UnityEvent();
            initEvent.AddListener(() => {
                SceneLoadManager.instance.unloadCurrentScenes();
                SceneLoadManager.instance.load(SceneLoadManager.SceneIndex.play);
            });

            LoadingScreen.instance.openEvent = initEvent;
            LoadingScreen.instance.Open();
        };

        Action<GameAction> actStartGame = x =>
        {
            wonLastGame = false;
            UnityEvent initEvent = new UnityEvent();
            UnityEngine.Debug.LogError("Starte Runde in State " + GetState());
            SetLogic();


            LoadingScreen.instance.Close();
        };

        Action<GameAction> actWin = x =>
        {
            wonLastGame = true;
            LevelManager.Clear();

            UnityEvent onWinEvent = new UnityEvent();


            LoadingScreen.instance.openEvent = onWinEvent;
            LoadingScreen.instance.Open();
        };

        Action<GameAction> nop = x =>
        {
        };
        







        gameStateFSM.transitions.Add(new Tuple<State,GameAction>(Init,LoadGameFromBoot), new Tuple<Action<GameAction>,State>(fromInitToMainMenu,MainMenu));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(MainMenu, Reset), new Tuple<Action<GameAction>, State>(actResetDisconnect, MainMenu)); 
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(MainMenu, EnterMainMenu), new Tuple<Action<GameAction>, State>(nop, MainMenu));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(MainMenu, EnterTestFromMainMenu), new Tuple<Action<GameAction>, State>(fromMainMenuToEdit, Test));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(MainMenu, EnterEditFromMainMenu), new Tuple<Action<GameAction>, State>(fromMainMenuToEdit, Edit));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(MainMenu, EnterEditFromMainMenuNewLevel), new Tuple<Action<GameAction>, State>(fromMainMenuToEditNewLevel, Edit));


        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(MainMenu, PrepareGameFromMainMenu), new Tuple<Action<GameAction>, State>(actPrepareGame, PrepareGame));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(PrepareGame, StartPlay), new Tuple<Action<GameAction>, State>(actStartGame, Play));

        //Hier evtl mod
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(Play, EnterMainMenu), new Tuple<Action<GameAction>, State>(actClearAfterGame, MainMenu));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(Play, BackToLobbyAfterWin), new Tuple<Action<GameAction>, State>(actWin, MainMenu));


        //Reset Level
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(Edit, EnterMainMenu), new Tuple<Action<GameAction>, State>(fromEditToMainMenu, MainMenu));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(Test, EnterMainMenu), new Tuple<Action<GameAction>, State>(fromTestToMainMenu, MainMenu));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(Edit, EnterTestFromEdit), new Tuple<Action<GameAction>, State>(fromEditToTest, Test));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(Test, EnterEditFromTest), new Tuple<Action<GameAction>, State>(fromTestToEdit, Edit));


    }

    //this is ugly need better way
    void Update()
    {
        if(currentLogic!=null)
        if(currentLogic.GetType() == typeof(TestLogic))
        {
            (currentLogic as TestLogic).Update();
        }
    }


    public void performAction(GameAction action)
    {
        UnityEngine.Debug.Log("GameManager: "+action.ToString());
        gameStateFSM.Move(action);
    }

    public void createLevel(LevelMetaData newLevelData)
    {
        levelMetaDataForNewLevel = newLevelData;
        performAction(EnterEditFromMainMenuNewLevel);
    }
    
    public void editLevel(LevelMetaData data)
    {
        levelMetaDataForEditLevel = data;
        performAction(EnterEditFromMainMenu);
    }

    //Disgusting but no better option really with custom enums

    public void UpdateLogic()
    {
        if(currentLogic!=null)
        {
            if (currentLogic.running == true) currentLogic.Stop();
            currentLogic.DeInit();
        }


            if(GetState() == MainMenu)
            { 
            currentLogic = new LobbyLogic();
            }
            else
            if (GetState() == Edit)
                currentLogic = new EditLogic();
            else    
            if (GetState() == Test)
                currentLogic = new TestLogic();
            else    
            if (GetState() == Play)
                currentLogic = new PlayLogic();
            else    
            if (GetState() == Init)
                currentLogic = null;

        if (currentLogic != null)
        {
            currentLogic.Init();
        }
    }
}
