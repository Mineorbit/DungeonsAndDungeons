using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    FSM gameStateFSM;
    public enum State {Init = 0, MainMenu, PlayLocal, PlayOnline, Edit , Test};
    public State currentGameState;
    public enum GameAction { LoadGameFromBoot = 0,EnterTestFromMainMenu };

    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        } else instance = this;
        SetupGameStateFSM();
        performAction(GameAction.LoadGameFromBoot);
    }
    public void SetupGameStateFSM()
    {
        UnityEvent closeLoadingScreen = new UnityEvent();
        closeLoadingScreen.AddListener(LoadingScreen.instance.closeLoadScreen);
        int[] afterInit = {1,1,1};
        int[] afterMenu = {1,3,1};
        int[][] gameStateTranslationTable =  {afterInit,afterMenu,afterInit,afterInit};
        //x ist GameAction int
        //Wichtige lücke: aktuell können unloading und neues szenen laden passieren obwohl loading screen noch nicht voll da
        Action<int>[] stateTable =      { x => 
                                        {
                                            PauseMenu.pauseMenuAvailable = true;
                                            currentGameState = State.Init;
                                            UnityEngine.Debug.Log("Reinitliasing Game"); 
                                        }
                                        , x => 
                                        {
                                            LoadingScreen.instance.openLoadScreen();
                                            SceneLoadManager.instance.unloadCurrentScene();
                                            PauseMenu.pauseMenuAvailable = false;
                                            UnityEngine.Debug.Log("Loading MainMenu");
                                            currentGameState = State.MainMenu;
                                            SceneLoadManager.instance.load(1,closeLoadingScreen);
                                        }
                                        , x => { }
                                        , x => 
                                        {
                                            LoadingScreen.instance.openLoadScreen();
                                            SceneLoadManager.instance.unloadCurrentScene();
                                            PauseMenu.pauseMenuAvailable = true;
                                            UnityEngine.Debug.Log("Loading Test");
                                            currentGameState = State.Test;

                                            SceneLoadManager.instance.load(2,closeLoadingScreen);
                                        }
        };
        gameStateFSM = new FSM("GameState",gameStateTranslationTable,stateTable);
    }
    public void performAction(GameAction action)
    {
        gameStateFSM.Move((int) action);
    }
    void Update()
    {
        
    }
}
