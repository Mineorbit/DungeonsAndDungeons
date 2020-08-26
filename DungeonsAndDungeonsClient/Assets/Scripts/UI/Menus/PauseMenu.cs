using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool pauseMenuAvailable;
    public bool freezeGame;
    public UIAnimation animation;
    void Start()
    {
        Setup();
    }

    void Setup()
    {
        animation = new FadeAndGrow();
        animation.target = this.transform;
    }
    void Open()
    {
        if(freezeGame)
        //Inform GameManager of Attempt to (stop simulation in local play)
        
        
        animation.Play();
    }
    void Update()
    {

    if(pauseMenuAvailable&&Input.GetKeyDown(KeyCode.Escape))
        {
            Open();
        }
    }
}
