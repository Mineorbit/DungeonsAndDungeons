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

    public enum State {Init = 0, MainMenu, Play, Edit , Test};
    public enum GameAction { LoadGameFromBoot = 0 ,Reset ,EnterMainMenu ,BackToLobbyAfterWin , EnterTestFromMainMenu, EnterEditFromMainMenuNewLevel, EnterEditFromMainMenu, EnterTestFromEdit,EnterEditFromTest, StartPlay};

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
        performAction(GameAction.LoadGameFromBoot);
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

        afterMenuLoad.AddListener(OptionsMenu.HandleSimpleLobbyChange);

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


   


    public void SetupGameStateFSM()
    {
    gameStateFSM = new FSM<State,GameAction>();
    gameStateFSM.state = State.Init;
    gameStateFSM.name = "GameState";

        Action<GameAction> act = x =>
        {
            wonLastGame = false;
            UnityEvent initEvent = new UnityEvent();

            UnityEvent menuLoadFinishedEvent = new UnityEvent();
            menuLoadFinishedEvent.AddListener(LoadingScreen.instance.Close);
            initEvent.AddListener(()=> {
            SceneLoadManager.instance.load(SceneLoadManager.SceneIndex.menu,menuLoadFinishedEvent);
            });
            
            LoadingScreen.instance.openEvent = initEvent;
            LoadingScreen.instance.Open();
        };
        Action<GameAction> actEditTest = x =>
        {
            LevelManager.StartRound();

            UnityEvent swapEvent = new UnityEvent();
            swapEvent.Invoke();
            SetLogic();
        };
        Action<GameAction> actTestEdit = x =>
        {
            LevelManager.Reset();
            UnityEvent swapEvent = new UnityEvent();
            swapEvent.Invoke();
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
            

            initEvent.AddListener(ResetGame);
            PlayerManager.playerManager.Reset();
            //NetworkManager.instance.Reset();
            LevelManager.Clear();
            LoadingScreen.instance.openEvent = initEvent;
            LoadingScreen.instance.Open();
        };


        Action<GameAction> actStartPlay = x =>
        {
            wonLastGame = false;
            UnityEvent initEvent = new UnityEvent();
            UnityEngine.Debug.LogError("Guten Morgen: " + GetState());
            LevelManager.Clear();
            UpdateLogic();


            LoadingScreen.instance.openEvent = initEvent;
            LoadingScreen.instance.Open();
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
        



        gameStateFSM.transitions.Add(new Tuple<State,GameAction>(State.Init,GameAction.LoadGameFromBoot), new Tuple<Action<GameAction>,State>(act,State.MainMenu));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.MainMenu, GameAction.Reset), new Tuple<Action<GameAction>, State>(actResetDisconnect, State.MainMenu)); 
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.MainMenu, GameAction.EnterMainMenu), new Tuple<Action<GameAction>, State>(nop, State.MainMenu));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.MainMenu, GameAction.EnterTestFromMainMenu), new Tuple<Action<GameAction>, State>(act, State.Test));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.MainMenu, GameAction.EnterEditFromMainMenu), new Tuple<Action<GameAction>, State>(act, State.Edit));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.MainMenu, GameAction.EnterEditFromMainMenuNewLevel), new Tuple<Action<GameAction>, State>(act, State.Edit));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.MainMenu, GameAction.StartPlay), new Tuple<Action<GameAction>, State>(actStartPlay, State.Play));
        //Hier evtl mod
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.Play, GameAction.EnterMainMenu), new Tuple<Action<GameAction>, State>(actClearAfterGame, State.MainMenu));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.Play, GameAction.BackToLobbyAfterWin), new Tuple<Action<GameAction>, State>(actWin, State.MainMenu));


        //Reset Level
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.Edit, GameAction.EnterMainMenu), new Tuple<Action<GameAction>, State>(actLevelClear, State.MainMenu));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.Edit, GameAction.EnterTestFromEdit), new Tuple<Action<GameAction>, State>(actEditTest, State.Test));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.Test, GameAction.EnterEditFromTest), new Tuple<Action<GameAction>, State>(actTestEdit, State.Edit));

        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.Test, GameAction.EnterMainMenu), new Tuple<Action<GameAction>, State>(actLevelClear, State.MainMenu));

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

    UnityAction lastNewLevelAction;
    public void createLevel(LevelMetaData data)
    {
        if (lastNewLevelAction != null) afterEditLoad.RemoveListener(lastNewLevelAction);
        if (lastEditLevelAction != null) afterEditLoad.RemoveListener(lastEditLevelAction);
        UnityAction NewLevelAction = () => { LevelDataManager.New(data); };
        afterEditLoad.AddListener(NewLevelAction);
        lastNewLevelAction = NewLevelAction;

        performAction(GameAction.EnterEditFromMainMenuNewLevel);
    }
    
    UnityAction lastEditLevelAction;
    public void editLevel(LevelMetaData data)
    {
        if (lastNewLevelAction != null) afterEditLoad.RemoveListener(lastNewLevelAction);
        if (lastEditLevelAction != null) afterEditLoad.RemoveListener(lastEditLevelAction);
        UnityAction EditLevelAction = () => { LevelDataManager.Load(data); };
        afterEditLoad.AddListener(EditLevelAction);
        lastEditLevelAction = EditLevelAction;

        performAction(GameAction.EnterEditFromMainMenu);
    }
    public void UpdateLogic()
    {
        if(currentLogic!=null)
        {
            if (currentLogic.running == true) currentLogic.Stop();
            currentLogic.DeInit();
        }
        switch(gameStateFSM.state)
        {
            case State.MainMenu:
                currentLogic = new Lobby();
                break;
            case State.Edit:
                currentLogic = new EditLogic();
                break;
            case State.Test:
                currentLogic = new TestLogic();
                break;
            case State.Play:
                currentLogic = new PlayLogic();
                break;
            case State.Init:
                currentLogic = null;
                break;
        }

        if (currentLogic != null)
        {
            currentLogic.Init();
        }
    }
}
