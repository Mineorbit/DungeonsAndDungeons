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
    FSM gameStateFSM;
    public enum State {Init = 0, MainMenu, PlayLocal, PlayOnline, Edit , Test};
    public State currentGameState;
    public enum GameAction { LoadGameFromBoot = 0,EnterMainMenu ,EnterTestFromMainMenu,EnterEditFromMainMenu, EnterTestFromEdit,EnterEditFromTest};

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
        int[] afterInit = {0,1,1,1,1};
        int[] afterMenu = {1,1,3,2,3};
        int[] afterEdit = { 1, 1, 3, 2, 3 };
        int[] afterTest = { };
        int[][] gameStateTranslationTable =  {afterInit,afterMenu,afterInit,afterInit,afterEdit,afterInit};
        //x ist GameAction int
        //Wichtige lücke: aktuell können unloading und neues szenen laden passieren obwohl loading screen noch nicht voll da
        //Consistency tests in den Transfers einbauen
        Action<int>[] stateTable =      { x =>
                                        {
                                            UnityEngine.Debug.Log("Initiliasing Game");
                                            UnityEvent initEvent = new UnityEvent();
                                            initEvent.AddListener(asyncInit);
                                            LoadingScreen.instance.setLoadingScreenOpen(initEvent);
                                            
                                        }
                                        , x =>
                                        {
                                            UnityEngine.Debug.Log("Loading MainMenu");

                                            UnityEvent menuLoad = new UnityEvent();
                                            menuLoad.AddListener(asyncMenuLoad);

                                            if(!LoadingScreen.instance.isOpen())
                                            {
                                                UnityEngine.Debug.Log("Nochmal öffnen");
                                                LoadingScreen.instance.openLoadingScreen(menuLoad);
                                            }else
                                            {
                                                asyncMenuLoad();
                                            }



                                        }
                                        , x => 
                                        {
                                            UnityEngine.Debug.Log("Loading Edit");
                                            if(!LoadingScreen.instance.isOpen())
                                            {
                                                UnityEvent editLoad = new UnityEvent();
                                                editLoad.AddListener(asyncEditLoad);
                                                LoadingScreen.instance.openLoadingScreen(editLoad);
                                            }else
                                            {
                                                asyncEditLoad();
                                            }
                                        }
                                        , x =>
                                        {
                                            UnityEngine.Debug.Log("Loading Test");

                                            if(!LoadingScreen.instance.isOpen())
                                            {
                                                UnityEvent testLoad = new UnityEvent();
                                                testLoad.AddListener(asyncTestLoad);
                                                LoadingScreen.instance.openLoadingScreen(testLoad);
                                            }else
                                            {
                                                asyncTestLoad();
                                            }
                                        }
        };
        gameStateFSM = new FSM("GameState",gameStateTranslationTable,stateTable);
    }
    void asyncInit()
    {
        currentGameState = State.Init;
        performAction(GameAction.EnterMainMenu);
    }
    void asyncMenuLoad()
    {
        SceneLoadManager.instance.unloadCurrentScenes();
        currentGameState = State.MainMenu;
        SceneLoadManager.instance.load(1, afterMenuLoad);
    }
    void asyncEditLoad()
    {

        SceneLoadManager.instance.unloadCurrentScenes();
        currentGameState = State.Edit;
        int[] toLoad = new int[2];
        toLoad[0] = 2;
        toLoad[1] = 3;
        SceneLoadManager.instance.load(toLoad,afterEditLoad);
    }
    void asyncTestLoad()
    {
    SceneLoadManager.instance.unloadCurrentScenes();
    currentGameState = State.Test;

        //Connect to Game Server

    SceneLoadManager.instance.load(2, afterTestLoad);
    }
    public void performAction(GameAction action)
    {
        gameStateFSM.Move((int) action);
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
        switch(currentGameState)
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
