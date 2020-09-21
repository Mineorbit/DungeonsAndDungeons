using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client
{

    public int globalId;

    public int localId;
    public Player player;
    public TcpClient tcpClient;

    public Client(int _clientId)
    {
        globalId = _clientId;
    }
}