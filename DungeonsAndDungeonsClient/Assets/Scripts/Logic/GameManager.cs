using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    bool instanceSet = false;
    public Logic currentLogic;

    UnityEvent afterMenuLoad;
    UnityEvent afterTestLoad;
    UnityEvent afterEditLoad;
    public enum State {Init = 0, MainMenu, PlayLocal, PlayOnline, Edit , Test};
    public enum GameAction { LoadGameFromBoot = 0,Reset,EnterMainMenu ,EnterTestFromMainMenu, EnterEditFromMainMenuNewLevel, EnterEditFromMainMenu, EnterTestFromEdit,EnterEditFromTest};

    FSM<State,GameAction> gameStateFSM;

    UnityEvent[] asyncEvent;

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
        Setup();
    }
    void Setup()
    {
        if (instance != null && !instanceSet) Destroy(this);
        instance = this;
        if (!instanceSet) instanceSet = true;
        SetupGameStateFSM();
        setupAfterLoadEvents();
    }

    public void setupAfterLoadEvents()
    {
        afterMenuLoad = new UnityEvent();
        afterMenuLoad.AddListener(LoadingScreen.instance.closeLoadingScreen);
        afterMenuLoad.AddListener(SetLogic);
        afterMenuLoad.AddListener(LevelManager.UpdateLocalLevels);
        afterTestLoad = new UnityEvent();
        afterTestLoad.AddListener(LoadingScreen.instance.closeLoadingScreen);
        afterTestLoad.AddListener(SetLogic);
        afterTestLoad.AddListener(LevelManager.UpdateLocalLevels);


        afterEditLoad = new UnityEvent();
        afterEditLoad.AddListener(LoadingScreen.instance.closeLoadingScreen);
        afterEditLoad.AddListener(SetLogic);
        afterEditLoad.AddListener(LevelManager.UpdateLocalLevels);

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
            UnityEvent initEvent = new UnityEvent();
            selectAsyncLoad(initEvent);
            LoadingScreen.instance.setLoadingScreenOpen(initEvent);
        };
        Action<GameAction> actEditTest = x =>
        {
            UnityEvent swapEvent = new UnityEvent();
            selectAsyncSwap(swapEvent);
            swapEvent.Invoke();
            SetLogic();
        };
        Action<GameAction> actResetDisconnect = x =>
        {
            UnityEvent initEvent = new UnityEvent();
            selectAsyncLoad(initEvent);

            NetworkManager.instance.Reset();

            initEvent.AddListener(ResetGame);
            LoadingScreen.instance.setLoadingScreenOpen(initEvent);
        };

        Action<GameAction> actLevelClear = x =>
        {
            UnityEvent initEvent = new UnityEvent();
            selectAsyncLoad(initEvent);
            Level.Clear();
            LoadingScreen.instance.setLoadingScreenOpen(initEvent);
        };


        gameStateFSM.transitions.Add(new Tuple<State,GameAction>(State.Init,GameAction.LoadGameFromBoot), new Tuple<Action<GameAction>,State>(act,State.MainMenu));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.MainMenu, GameAction.Reset), new Tuple<Action<GameAction>, State>(actResetDisconnect, State.MainMenu));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.MainMenu, GameAction.EnterTestFromMainMenu), new Tuple<Action<GameAction>, State>(act, State.Test));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.MainMenu, GameAction.EnterEditFromMainMenu), new Tuple<Action<GameAction>, State>(act, State.Edit));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.MainMenu, GameAction.EnterEditFromMainMenuNewLevel), new Tuple<Action<GameAction>, State>(act, State.Edit));


        //Reset Level
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.Edit, GameAction.EnterMainMenu), new Tuple<Action<GameAction>, State>(actLevelClear, State.MainMenu));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.Edit, GameAction.EnterTestFromEdit), new Tuple<Action<GameAction>, State>(actEditTest, State.Test));
        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.Test, GameAction.EnterEditFromTest), new Tuple<Action<GameAction>, State>(actEditTest, State.Edit));

        gameStateFSM.transitions.Add(new Tuple<State, GameAction>(State.Test, GameAction.EnterMainMenu), new Tuple<Action<GameAction>, State>(actLevelClear, State.MainMenu));

    }

    void selectAsyncLoad(UnityEvent e)
    {
        switch(gameStateFSM.state)
        {
            case State.MainMenu:
                e.AddListener(asyncMenuLoad);
                break;
            case State.Edit:
                e.AddListener(asyncEditLoad);
                break;
            case State.Test:
                e.AddListener(asyncTestLoad);
                break;
            case State.PlayOnline:
                e.AddListener(asyncPlayLoad);
                break;

        }

    }
    void selectAsyncSwap(UnityEvent e)
    {
        switch(gameStateFSM.state)
        {
            case State.Test:
                e.AddListener(EditToTest);
                break;
            case State.Edit:
                e.AddListener(TestToEdit);
                break;
        }
    }
    
    void asyncMenuLoad()
    {
        SceneLoadManager.instance.unloadCurrentScenes();
        SceneLoadManager.instance.load(1, afterMenuLoad);
    }

    void EditToTest()
    {
        UnityEngine.Debug.Log("Loading Test Scene");

        SceneLoadManager.instance.unload(1);
        SceneLoadManager.instance.unload(2);
        SceneLoadManager.instance.load(2);
    }
    void TestToEdit()
    {
        UnityEngine.Debug.Log("Loading Edit Scene");
        SceneLoadManager.instance.unload(1);
        SceneLoadManager.instance.unload(3);
        SceneLoadManager.instance.load(3);
    }

    void asyncEditLoad()
    {
        UnityEngine.Debug.Log("Moin Meister");
        SceneLoadManager.instance.unloadCurrentScenes();
        SceneLoadManager.instance.load(3,afterEditLoad);
    }
    void asyncTestLoad()
    {
    SceneLoadManager.instance.unloadCurrentScenes();

    SceneLoadManager.instance.load(2, afterTestLoad);
    }
    void asyncPlayLoad()
    {
        SceneLoadManager.instance.unloadCurrentScenes();

        //Connect to Game Server

        SceneLoadManager.instance.load(2, afterTestLoad);
    }
    public void performAction(GameAction action)
    {
        gameStateFSM.Move(action);
    }
    UnityAction lastNewLevelAction;
    public void createLevel(LevelData.LevelMetaData data)
    {
        if (lastNewLevelAction != null) afterEditLoad.RemoveListener(lastNewLevelAction);
        if (lastEditLevelAction != null) afterEditLoad.RemoveListener(lastEditLevelAction);
        UnityAction NewLevelAction = () => { LevelManager.New(data); };
        afterEditLoad.AddListener(NewLevelAction);
        lastNewLevelAction = NewLevelAction;

        performAction(GameAction.EnterEditFromMainMenuNewLevel);
    }
    UnityAction lastEditLevelAction;
    public void editLevel(LevelData.LevelMetaData data)
    {
        if (lastNewLevelAction != null) afterEditLoad.RemoveListener(lastNewLevelAction);
        if (lastEditLevelAction != null) afterEditLoad.RemoveListener(lastEditLevelAction);
        UnityAction EditLevelAction = () => { LevelManager.Load(data); };
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
            case State.PlayOnline:
                currentLogic = new PlayLogic();
                break;
            case State.PlayLocal:
                currentLogic = new PlayLogic();
                break;
        }
        if (currentLogic != null)
        {
            currentLogic.Init();
        }
    }
}
