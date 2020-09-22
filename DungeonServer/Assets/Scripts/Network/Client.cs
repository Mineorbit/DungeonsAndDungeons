using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client
{


    public byte[] buffer;
    int bufferSize = 4096;

    public int globalId;

    public int localId;
    public Player player;
    public TcpClient tcpClient;

    public Client(int _clientId)
    {
        globalId = _clientId;
        buffer = new byte[bufferSize];
    }

    public void Send(byte[] data)
    {

    }
    public static Client Connect(int id, TcpClient client)
    {
        Client c = new Client(id);
        c.tcpClient = client;
        return c;
    }
    public void Disconnect()
    {

    }
}