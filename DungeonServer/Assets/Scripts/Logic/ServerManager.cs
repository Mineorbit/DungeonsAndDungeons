using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class ServerManager : MonoBehaviour
{
    public static ServerManager instance;
    public enum State{Setup,Idle,Prepare,Lobby,Play,GameOver};
    public enum GameAction {GoLive};
    FSM<State, GameAction> serverState;

    //Networking
    Server server;
    Client[] clients;
    //Settings
    GameLogic currentGame;
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
        SetupServer();
        serverState.Move(GameAction.GoLive);
    }
    void SetupState()
    {
        clients = new Client[4];
    }

    void SetupServer()
    {
        server = new Server();
    }

    public int GetFreeId()
    {
        int i = 0;
        while(clients[i]!=null)
        {
            i++;
        }
        return i;
    }

    public void AddClient(int localId, Client c)
    {

    }

    public void RemoveClient(int localid)
    {
        clients[i].Disconnect();
        currentGame.RemovePlayer(i);
    }


    void SetupFSM()
    {
        serverState = new FSM<State, GameAction>();
        serverState.state = State.Idle;
    }



}
