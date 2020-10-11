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
    TcpListener server;
    public Server()
    {
        int port = 13565;
        IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        server = new TcpListener(localAddr,port);
    }
    public void Start()
    {
        server.Start();
        HandleConnection();
    }
    async Task HandleConnection()
    {
        int localId = ServerManager.instance.GetFreeId();
        return;
    }
    public void StopListen()
    {
        server.Stop();
    }
}
