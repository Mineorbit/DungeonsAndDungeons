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
    public Button optionsButton;

    void Start()
    {
        Setup();
    }

    void Setup()
    {
        animation = new FadeAndGrow();
        animation.target = this.transform;
        backToMainMenuButton = transform.Find("Main").GetComponent<Button>();
        optionsButton = transform.Find("Opt").GetComponent<Button>();
        optionsButton.onClick.AddListener(GotoOptions);
        backToMainMenuButton.onClick.AddListener(GotoMainMenu);
    }
    void GotoOptions()
    {
        OptionsMenu.options.Open();
    }
    void GotoMainMenu()
    {
        Close();
        UnityEngine.Debug.Log("Entering Main Menu from Pause Menu");
        GameManager.instance.performAction(GameManager.GameAction.EnterMainMenu);
    }
    bool checkAvailability()
    {
        switch(GameManager.GetState())
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
        if (LoadingScreen.instance.open)
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
                if (PlayerController.currentPlayer != null)
                    PlayerManager.acceptInput = false;
            }
        BlurScreen.blurScreen.Open();
        animation.Play();
        MouseStateController.UnlockBlocking();
    }
    void Close()
    {

        if (!checkAvailability()) return;

        if (freezeGame)
        //Inform GameManager of Attempt to (unfreeze Sim)
        {
            if(PlayerController.currentPlayer!=null)
            PlayerManager.acceptInput = true;
        }
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
