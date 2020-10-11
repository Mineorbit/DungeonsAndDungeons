using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Server
{
    public static object idLock = new object();
    TcpListener server;
    int port;
    public Server()
    {
        int lport = 13587;
        port = lport;
        IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        server = new TcpListener(localAddr,port);
    }
    public async Task Start()
    {
        server.Start();

        Debug.Log($"Socket {port} open");
        while(true)
        {
            TcpClient client = await server.AcceptTcpClientAsync(); 
            HandleConnection(client);
        }
    }
    async Task HandleConnection(TcpClient c)
    {
        int localId;
        Debug.Log($"Neue Verbindung {c}");
        Client client = new Client(c);

        //Das hier mutex
        lock (idLock)
        {
            localId = ServerManager.instance.GetFreeId();
            ServerManager.instance.AddClient(localId,client);
        }
        return;
    }
    public void StopListen()
    {
        server.Stop();
    }
}
