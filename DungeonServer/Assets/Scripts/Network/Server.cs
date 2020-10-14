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


    static Client[] clients;
    int port;
    public Server()
    {
        clients = new Client[4];
        int lport = 13587;
        port = lport;
        IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        server = new TcpListener(localAddr,port);
    }
    public static Client GetClient(int l)
    {
        return clients[l];
    }
    public static void RemoveClient(int local)
    {
        clients[local] = null;
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
    public static int GetFreeId()
    {
        int i = 0;
        while (clients[i] != null)
        {
            i++;
        }
        return i;
    }
    async Task HandleConnection(TcpClient c)
    {
        int localId;
        Debug.Log($"Neue Verbindung {c}");
        Client client;
        //Das hier mutex
        lock (idLock)
        {
            localId = GetFreeId();
            client = new Client(localId, c);
            clients[localId] = client;
            client.StartRead();
        }
        return;
    }
    public void StopListen()
    {
        server.Stop();
    }
   
    public static void Disconnect(int localId)
    {
        if(clients[localId]!=null)
        clients[localId].Disconnect();
    }
    public static void DisconnectAll()
    {
        for(int i = 0;i<4; i++)
        {
            if (clients[i] != null)
                clients[i].Disconnect();
        }
    }
    public static void SendPacketToAllExcept(int localId, Packet p)
    {
        for(int i = 0;i<4;i++)
        {
            if (i != localId)
                if (clients[i] != null)
                {
                    SendPacket(i, p);
                }
        }
    }
    public static void SendPacket(int localId, Packet p)
    {
        
        if(clients[localId]!=null)
        {
        clients[localId].Send(p);
        }
    }
}
