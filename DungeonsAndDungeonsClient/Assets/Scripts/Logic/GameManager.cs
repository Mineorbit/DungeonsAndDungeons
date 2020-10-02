using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{


    public static GameManager instance;

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
    }
    public void SetupGameStateFSM()
    {
        int[] afterInit = {0,1,1};
        int[] afterMenu = {1,1,3};
        int[] afterEdit = { };
        int[] afterTest = { };
        int[][] gameStateTranslationTable =  {afterInit,afterMenu,afterInit,afterInit};
        //x ist GameAction int
        //Wichtige lücke: aktuell können unloading und neues szenen laden passieren obwohl loading screen noch nicht voll da
        //Consistency tests in den Transfers einbauen
        Action<int>[] stateTable =      { x =>
                                        {
                                            UnityEngine.Debug.Log("Initliasing Game");
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
        PauseMenu.pauseMenuAvailable = false;
        currentGameState = State.Init;
        performAction(GameAction.EnterMainMenu);
    }
    void asyncMenuLoad()
    {
        SceneLoadManager.instance.unloadCurrentScene();
        PauseMenu.pauseMenuAvailable = false;
        currentGameState = State.MainMenu;
        SceneLoadManager.instance.load(1, afterMenuLoad);
    }
    void asyncEditLoad()
    {

        SceneLoadManager.instance.unloadCurrentScene();
        PauseMenu.pauseMenuAvailable = false;
        currentGameState = State.Edit;
        SceneLoadManager.instance.load(2);
        SceneLoadManager.instance.load(3,afterEditLoad);
    }
    void asyncTestLoad()
    {
    SceneLoadManager.instance.unloadCurrentScene();
    PauseMenu.pauseMenuAvailable = true;
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
        LevelManager.levelManager.New(data);
    }

    void Update()
    {
        
    }
}
