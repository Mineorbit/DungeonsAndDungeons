using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    enum State {Loading,Running,Overload};
    enum GameState {Lobby,Play,Win};
    public enum LevelObjectType  { cursor = 0,  enemy = 1 , floor = 2, spawn = 3, wall = 4, goal = 5};
    public float tickRate = 64;
    void Start()
    {
        QualitySettings.vsyncCount = 0;
        Application.targetFrameRate = (int)tickRate;
        Time.fixedDeltaTime = 1/tickRate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
