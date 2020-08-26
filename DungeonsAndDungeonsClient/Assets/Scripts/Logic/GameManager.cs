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
 
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        } else instance = this;
        SetupGameStateFSM();
        gameStateFSM.Move(1);
    }
    public void SetupGameStateFSM()
    {
        UnityEvent closeLoadingScreen = new UnityEvent();
        closeLoadingScreen.AddListener(LoadingScreen.instance.closeLoadScreen);
        int[] afterInit = {1,1,1};
        int[][] gameStateTranslationTable =  {afterInit,afterInit,afterInit,afterInit};
        Action<int>[] stateTable =      { x => 
                                        {
                                            PauseMenu.pauseMenuAvailable = true;
                                            UnityEngine.Debug.Log("Reinitliasing Game"); 
                                        }
                                        , x => 
                                        {
                                            PauseMenu.pauseMenuAvailable = false;
                                            LoadingScreen.instance.openLoadScreen();
                                            UnityEngine.Debug.Log("Loading MainMenu");
                                            currentGameState = State.MainMenu;
                                            SceneLoadManager.instance.load(1,closeLoadingScreen);
                                        }
                                        , x => { }
                                        , x => { LoadingScreen.instance.openLoadScreen(); UnityEngine.Debug.Log("Loading Test"); currentGameState = State.Test; SceneLoadManager.instance.load(2); } };
        gameStateFSM = new FSM("GameState",gameStateTranslationTable,stateTable);
    }
    
    void Update()
    {
        
    }
}
