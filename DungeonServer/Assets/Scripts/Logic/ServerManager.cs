using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class ServerManager : MonoBehaviour
{
    public static ServerManager instance;
    public enum State{Setup,Prepare,Lobby,Play,GameOver};
    public enum GameAction {GoLive, Prepare};
    FSM<State, GameAction> serverState;

    public InstantionTarget playerTarget;

    //Networking
    Server server;

    //Settings
    public bool Local = true;
    string password = "Test";


    void Start()
    {

        if(instance==null)
        {
            instance = this;
        }else if(instance!=this)
        {
            Destroy(this);
        }
        SetupFSM();
        serverState.Move(GameAction.Prepare);
    }
    void SetupLogic()
    {
        gameObject.AddComponent<GameLogic>();
    }

   
    void SetupServer()
    {
        server = new Server();
    }

    

    public void AddClient(int localId, Client c)
    {
        GameLogic.current.AddPlayer(localId,c);
    }

    public void RemoveClient(int localid)
    {
        Server.Disconnect(localid);
        GameLogic.current.RemovePlayer(localid);
        Server.RemoveClient(localid);
        
    }



    void SetupFSM()
    {
        serverState = new FSM<State, GameAction>();
        serverState.state = State.Setup;
        Action<GameAction> actSetup = x => {
            Debug.Log("Setting up");
            SetupLogic();
            SetupServer();
            Debug.Log("Setup done");
            serverState.Move(GameAction.GoLive);
        };
        Action<GameAction> actLive = x => {
            Debug.Log("Opening Socket");
            server.Start();
        }; 
        
        serverState.transitions.Add(new Tuple<State, GameAction>(State.Setup, GameAction.Prepare), new Tuple<Action<GameAction>, State>(actSetup, State.Prepare));

        serverState.transitions.Add(new Tuple<State,GameAction>(State.Prepare,GameAction.GoLive),new Tuple<Action<GameAction>,State>(actLive,State.Lobby));
    }

    void Stop()
    {
        Server.DisconnectAll();
        server.StopListen();
    }

    void OnDisable()
    {
        Debug.Log("Server stopping");
        Stop();
    }
    

}
