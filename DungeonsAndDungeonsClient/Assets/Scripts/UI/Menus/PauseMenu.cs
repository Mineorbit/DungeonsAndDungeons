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

    void Open()
    {
        open = true;
        if(freezeGame)
        //Inform GameManager of Attempt to (stop simulation in local play)
        
        if(freezePlayer)
        {
                PlayerController.acceptInput = false;
        }
        animation.Play();
    }
    void Close()
    {
        if (freezeGame)
            //Inform GameManager of Attempt to (unfreeze Sim)

                PlayerController.acceptInput = true;
        animation.Play();
        open = false;
    }

    void Update()
    {
    if(pauseMenuAvailable&&Input.GetKeyDown(KeyCode.Escape))
        {
            if(!open)
            { 
            Open();
            }
            else
            {
            Close();
            }
        }
       }
    }
