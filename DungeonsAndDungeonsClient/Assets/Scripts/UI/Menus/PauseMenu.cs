using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    public static bool pauseMenuAvailable;
    public bool freezeGame;
    public UIAnimation animation;
    public bool open;
    public bool freezePlayer = true;

    public Button backToMainMenuButton;
    void Start()
    {
        Setup();
    }

    void Setup()
    {
        animation = new FadeAndGrow();
        animation.target = this.transform;
        backToMainMenuButton = transform.Find("Main").GetComponent<Button>();
        backToMainMenuButton.onClick.AddListener(GotoMainMenu);
    }

    void GotoMainMenu()
    {
        Close();
        GameManager.instance.performAction(GameManager.GameAction.EnterMainMenu);
    }
    bool checkAvailability()
    {
        switch(GameManager.instance.GetState())
        {
            case GameManager.State.Init:
                pauseMenuAvailable = false;
                break;
            case GameManager.State.MainMenu:
                pauseMenuAvailable = false;
                break;
            default:
                pauseMenuAvailable = true;
                break;
        }
        if (LoadingScreen.instance.isOpen())
        {
            pauseMenuAvailable = false;
        }
        return pauseMenuAvailable;
    }
    void Open()
    {
        if (!checkAvailability()) return;
        open = true;
        if(freezeGame)
        //Inform GameManager of Attempt to (stop simulation in local play)
        
        if(freezePlayer)
        {
                PlayerController.acceptInput = false;
        }
        BlurScreen.blurScreen.Open();
        animation.Play();
        MouseStateController.UnlockBlocking();
    }
    void Close()
    {
        if (freezeGame)
            //Inform GameManager of Attempt to (unfreeze Sim)
                PlayerController.acceptInput = true;
        BlurScreen.blurScreen.Close();
        animation.Play();
        open = false;
        MouseStateController.LockUnblocking();
    }

    void Update()
    {
    if(Input.GetKeyDown(KeyCode.Escape))
        {
            if((!open && !animation.isOpen()) && !BlurScreen.blurScreen.isOpen())
            { 
            Open();
            }
            else if((open && animation.isOpen()) && BlurScreen.blurScreen.isOpen())
            {
            Close();
            }
        }
       }
    }
