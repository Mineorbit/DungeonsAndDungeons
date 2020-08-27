using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    UnityEvent closeLoadingScreen;
    FSM gameStateFSM;
    public enum State {Init = 0, MainMenu, PlayLocal, PlayOnline, Edit , Test};
    public State currentGameState;
    public enum GameAction { LoadGameFromBoot = 0,EnterMainMenu ,EnterTestFromMainMenu };

    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        } else instance = this;

        SetupGameStateFSM();
        setupLoadingScreen();
        performAction(GameAction.LoadGameFromBoot);
    }
    public void setupLoadingScreen()
    {
        closeLoadingScreen = new UnityEvent();
        closeLoadingScreen.AddListener(LoadingScreen.instance.closeLoadingScreen);
    }
    public void SetupGameStateFSM()
    {
        int[] afterInit = {0,1,1};
        int[] afterMenu = {1,1,3};
        int[][] gameStateTranslationTable =  {afterInit,afterMenu,afterInit,afterInit};
        //x ist GameAction int
        //Wichtige lücke: aktuell können unloading und neues szenen laden passieren obwohl loading screen noch nicht voll da
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
                                        , x => { }
                                        , x =>
                                        {
                                            UnityEngine.Debug.Log("Loading Test");
                                            UnityEvent testLoad = new UnityEvent();
                                            testLoad.AddListener(asyncTestLoad);
                                             
                                            if(!LoadingScreen.instance.isOpen())
                                            {
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
        SceneLoadManager.instance.load(1, closeLoadingScreen);
    }
    void asyncTestLoad()
    {
    SceneLoadManager.instance.unloadCurrentScene();
    PauseMenu.pauseMenuAvailable = true;
    currentGameState = State.Test;
    SceneLoadManager.instance.load(2, closeLoadingScreen);
    }
    public void performAction(GameAction action)
    {
        gameStateFSM.Move((int) action);
    }


    void Update()
    {
        
    }
}
