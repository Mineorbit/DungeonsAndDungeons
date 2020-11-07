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
    public TcpListener server;


    static Server instance;

    public Client[] clients;
    int port;
    bool accept;

    static Client[] GetClients()
    {
        return instance.clients;
    }

    public Server()
    {

        if (instance == null) instance = this;
        clients = new Client[4];
        int lport = 13587;
        port = lport;
        IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        server = new TcpListener(localAddr,port);
    }
    public static Client GetClient(int l)
    {
        return instance.clients[l];
    }
    public static void RemoveClient(int local)
    {
        instance.clients[local] = null;
    }
    public async Task Start()
    {
        server.Start();
        accept = true;
        Debug.Log($"Socket {port} open");
        for(int i = 0;(i<4)&&accept;i++)
        {
            TcpClient client = await server.AcceptTcpClientAsync(); 
            HandleConnection(client);
        }
    }
    public static int GetFreeId()
    {
        int i = 0;
        while (instance.clients[i] != null)
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
        accept = false;
        server.Stop();
    }
   
    public static void Disconnect(int localId)
    {
        if(instance.clients[localId]!=null)
        instance.clients[localId].Disconnect();
    }
    public static void DisconnectAll()
    {
        for(int i = 0;i<4; i++)
        {
            Disconnect(i);
        }
    }
    public static void SendPacketToAll(Packet p)
    {
        for (int i = 0; i < 4; i++)
        {
                if (instance.clients[i] != null)
                {
                    SendPacket(i, p);
                }
        }
    }
    public static void SendPacketToAllExcept(int localId, Packet p)
    {
        for(int i = 0;i<4;i++)
        {
            if (i != localId)
                    SendPacket(i, p);
        }
    }
    public static void SendPacket(int localId, Packet p)
    {
        
        if(instance.clients[localId]!=null)
        {
        instance.clients[localId].Send(p);
        }
    }
}
