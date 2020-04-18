using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ServerManager : MonoBehaviour
{
    public static ServerManager instance;
    public bool Local = true;
    public enum State{Setup,Idle,Prepare,Play,GameOver};
    public static State s;
    public Player[] players;

    void Start()
    {
        if(instance==null)
        {
            instance = this;
        }else if(instance!=this)
        {
            Destroy(this);
        }


        Setup();

       
    }

   

    void Idle()
    {
        s = State.Idle;
        //Do house keeping tasks like preloading favorite levels, manage memory etc


    }
    void Play()
    {
        s = State.Play;
        //Block entries communicate to  lobby everything necessary
    }


    void  GameOver()
    {
        s = State.GameOver;
        //Redirect Players and go back to  start

    }

    void Prepare()
    {
        s = State.Prepare;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 64;
        //Download and load map
        Debug.Log("Server wird gestartet");
        Server.Start(45565);
    }


    private void OnApplicationQuit()
    {
        Server.Stop();
        //Deallocate everything / delete garbage / create logs
    }
    void Setup()
    {

        s = State.Setup;

        Directory.CreateDirectory(Application.persistentDataPath+"/map");

        if(Local) 
        {
        GameLogic.current.playerCount = 1;
        }
        Prepare();
    }

    
}
