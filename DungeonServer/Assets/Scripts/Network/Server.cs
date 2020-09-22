using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading.Tasks;

public class Server : MonoBehaviour
{
    public static Server server;
    Client[] clients;
    int connected;
    int maxConnected = 4;
    TcpListener tcpListener;

    int port = 13565;


    void Start()
    {
        if (server != null) Destroy(this);
        server = this;

        Restart();
    }

    public void Setup()
    {
        connected = 0;
        clients = new Client[maxConnected];
        IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        tcpListener = new TcpListener(localAddr,port);
    }

    public void Open()
    {
        tcpListener.Start();
        Debug.Log("Listening on Network Port: "+port);
        Task t = AcceptClient();
    }

    public void Restart()
    {
        Setup();
        Open();
    }

    public  void Stop()
    {
        foreach (Client c in clients)
        {
            c.Disconnect();
        }
        Destroy(Server.server);
    }

    async Task AcceptClient()
    {
        Debug.Log("Waiting for Client");
        TcpClient client = await tcpListener.AcceptTcpClientAsync();
        Debug.Log("Client connected");
        connected++;
        Client c = Client.Connect(0,client);
        if(ServerManager.state != ServerManager.State.Connect)
        {
            c.Disconnect("Server is not accepting connections"); 
        }
        if(connected>maxConnected)
        {
            c.Disconnect("Server is full");
        }
        //Send Accept

        //Setup Async Read

        await AcceptClient();
    }

    public static void CreateServer(Transform t)
    {
        t.gameObject.AddComponent<Server>();
    }


}
