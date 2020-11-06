using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class ServerManager : MonoBehaviour
{
    public static ServerManager instance;
    public enum State{Setup,Prepare,Lobby,Play,GameOver};
    public enum GameAction {GoLive, Prepare,StartGame};
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


    public State GetState()
    {
        return serverState.state;
    }
    void SetupFSM()
    {
        serverState = new FSM<State, GameAction>();
        serverState.state = State.Setup;
        Action<GameAction> actSetup = x => {

            Debug.Log("Setting up");

            GameLogic.PrepareRound(this.transform);

            SetupServer();
            Debug.Log("Setup done");
            serverState.Move(GameAction.GoLive);

        };
        Action<GameAction> actLive = x => {
            Debug.Log("Opening Socket");
            server.Start();
        };


        Action<GameAction> actStartGame = x => {



            Debug.Log("Starting Round, no new connections");
            GameReadyPacket answerPacket = new GameReadyPacket(true);
            server.StopListen();
            Server.SendPacketToAll(answerPacket);
            GameLogic.current.StartRound();

        };



        serverState.transitions.Add(new Tuple<State, GameAction>(State.Setup, GameAction.Prepare), new Tuple<Action<GameAction>, State>(actSetup, State.Prepare));

        serverState.transitions.Add(new Tuple<State,GameAction>(State.Prepare,GameAction.GoLive),new Tuple<Action<GameAction>,State>(actLive,State.Lobby));
        serverState.transitions.Add(new Tuple<State, GameAction>(State.Lobby, GameAction.StartGame), new Tuple<Action<GameAction>, State>(actStartGame, State.Play));
    }

    void Stop()
    {
        Server.DisconnectAll();
        server.StopListen();
    }

    public void performAction(GameAction action)
    {
        serverState.Move(action);
    }

    void OnDisable()
    {
        Debug.Log("Server stopping");
        Stop();
    }
    

}
