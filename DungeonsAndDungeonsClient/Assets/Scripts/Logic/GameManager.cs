using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

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
        int[] afterInit = {1,1,1};
        int[][] gameStateTranslationTable =  {afterInit,afterInit,afterInit,afterInit};
        Action<int>[] stateTable =      { x => { UnityEngine.Debug.Log("Reinitliasing Game"); }
                                        , x => { UnityEngine.Debug.Log("Loading MainMenu");currentGameState = State.MainMenu; SceneManager.load(1); }
                                        , x => { }
                                        , x => { UnityEngine.Debug.Log("Loading Test"); currentGameState = State.Test; SceneManager.load(2); } };
        gameStateFSM = new FSM("GameState",gameStateTranslationTable,stateTable);
    }
    
    void Update()
    {
        
    }
}
