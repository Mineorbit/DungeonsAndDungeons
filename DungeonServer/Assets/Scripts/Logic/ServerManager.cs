using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class ServerManager : MonoBehaviour
{
    public static ServerManager instance;
    public bool Local = true;
    public enum State{Setup,Idle,Prepare,Connect,Play,GameOver};
    public static State state;

    void Start()
    {

        if(instance==null)
        {
            instance = this;
        }else if(instance!=this)
        {
            Destroy(this);
        }

        SetupGameServer();

       
    }

    void OpenGameServer()
    {
        if(GameLogic.current!=null && state == State.Prepare)
        ServerManager.state = State.Connect;
    }

    void SetupGameServer()
    {
        state = State.Idle;
        GameLogic.StartRound(this.transform);
        Server.CreateServer(this.transform);
    }


}
