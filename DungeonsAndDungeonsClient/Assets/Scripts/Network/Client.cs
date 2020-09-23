using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using UnityEngine.UI;
using TMPro;

public class Client : MonoBehaviour
{
    public static Client instance;

    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 13565;
    public TcpClient tcp;
    public void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }
    public void Start()
    {
        Setup();
    }

    public void Setup()
    {

    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) ConnectToLobbyServer();
    }
    public void ConnectToLobbyServer()
    {
        tcp = new TcpClient(ip,port);
    }
    public void ConnectToGameServer(string serverIp)
    {

    }

}