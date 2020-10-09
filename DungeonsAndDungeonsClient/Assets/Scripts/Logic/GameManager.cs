using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{


    public static GameManager instance;

    public Logic currentLogic;

    UnityEvent afterMenuLoad;
    UnityEvent afterTestLoad;
    UnityEvent afterEditLoad;
    public enum State {Init = 0, MainMenu, PlayLocal, PlayOnline, Edit , Test};
    public enum GameAction { LoadGameFromBoot = 0,EnterMainMenu ,EnterTestFromMainMenu,EnterEditFromMainMenu, EnterTestFromEdit,EnterEditFromTest};

    FSM<State,GameAction> gameStateFSM;

    UnityEvent[] asyncEvent;

    public State GetState()
    {
        return gameStateFSM.state;
    }
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        } else instance = this;

        SetupGameStateFSM();
        setupAfterLoadEvents();
        performAction(GameAction.LoadGameFromBoot);
    }
    public void setupAfterLoadEvents()
    {
        afterMenuLoad = new UnityEvent();
        afterMenuLoad.AddListener(LoadingScreen.instance.closeLoadingScreen);
        afterTestLoad = new UnityEvent();
        afterTestLoad.AddListener(LoadingScreen.instance.closeLoadingScreen);
        afterTestLoad.AddListener(SetLogic);
        afterEditLoad = new UnityEvent();
        afterEditLoad.AddListener(LoadingScreen.instance.closeLoadingScreen);
        afterEditLoad.AddListener(SetLogic);

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

        Action<GameAction> actMenu = x =>
        {
            UnityEngine.Debug.Log(x);
            UnityEvent initEvent = new UnityEvent();
            initEvent.AddListener(asyncMenuLoad);
            LoadingScreen.instance.setLoadingScreenOpen(initEvent);
        };
        gameStateFSM.transitions.Add(new Tuple<State,GameAction>(State.Init,GameAction.LoadGameFromBoot), new Tuple<Action<GameAction>,State>(actMenu,State.MainMenu));
    }

    
    void asyncMenuLoad()
    {
        SceneLoadManager.instance.unloadCurrentScenes();
        SceneLoadManager.instance.load(1, afterMenuLoad);
    }
    void asyncEditLoad()
    {

        SceneLoadManager.instance.unloadCurrentScenes();
        int[] toLoad = new int[2];
        toLoad[0] = 2;
        toLoad[1] = 3;
        SceneLoadManager.instance.load(toLoad,afterEditLoad);
    }
    void asyncTestLoad()
    {
    SceneLoadManager.instance.unloadCurrentScenes();

        //Connect to Game Server

    SceneLoadManager.instance.load(2, afterTestLoad);
    }
    public void performAction(GameAction action)
    {
        gameStateFSM.Move(action);
    }
    
    public void createLevel(LevelData.LevelMetaData data)
    {
        UnityEngine.Debug.Log("Creating new Level");
        //Das in Init von EditLogic Tun mit Create
        afterEditLoad.AddListener(()=> { LevelManager.New(data); });
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
