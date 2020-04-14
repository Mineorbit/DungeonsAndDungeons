using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server
{
    public static int Port { get; private set; }

    public static Client[] clients = new Client[4];
    public static int[] globalIds = new int[4];
    public delegate void PacketHandler(int _fromClient, Packet _packet);
    public static Dictionary<int, PacketHandler> packetHandlers;

    private static TcpListener tcpListener;

    public static void Start(int _port)
    {
        Port = _port;

        InitializeServerData();

        tcpListener = new TcpListener(IPAddress.Any, Port);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);


    }

    private static void TCPConnectCallback(IAsyncResult _result)
    {
        TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
        
        Debug.Log("Neue Verbindung: "+_client.ToString());
        //Handle new Client
        int _globalId;
        int _localId;

        NetworkStream stream;
        byte[] receivedData = new byte[8];
        Packet connectPacket = null;
        _client.ReceiveBufferSize = 4096;
        _client.SendBufferSize = 4096;

        stream = _client.GetStream();

        //Empfange willkommensPaket
        int toRead = 8;
        int read = toRead;
        

        AsyncCallback asyncCallback = null;
        asyncCallback = ar =>
        {
            Debug.Log("test");
                //Check globalId
        int[] data = ReadPlayerConnectPacket(receivedData);
        if(!checkIncomingPlayerValid(data)) return;

        _globalId = data[2];
        _localId = data[3];

        //Setup Client
        Client c = new Client(_globalId);
        c.localId = _localId;
        c.tcp.Connect(_client);
        
        //LOCK NECESSARY
        clients[c.localId] = c;
        globalIds[_localId] = _globalId; 
        
        Debug.Log($"Spieler {_localId} verbunden");
        return;
        };


        stream.BeginRead(receivedData, 0, 8, asyncCallback, null);

        foreach(byte b in receivedData)
        {
            Debug.Log(b);
        }
        
         

     }
        private void ConnectPacketReceiveCallback(IAsyncResult _result)
        {

        
            
        }


    private static int[] ReadPlayerConnectPacket(byte[] d)
    {
        int[] r = new int[4];
        r[0] = 256*(int)d[0]+(int)d[1];
        r[1] = (int) d[2];
        r[2] = 16777216*(int)d[3]+65536*(int)d[4]+256*(int)d[5]+(int)d[6];
        r[3] = (int) d[7];
        return r;
    }


     private static bool checkIncomingPlayerValid(int[] data)
     {
         if(data[0]!=6) return false;
         if(data[1]!=(int)ClientPackets.PlayerGameConnect) return false; 
         return true;
     }

   

    /// <summary>Initializes all necessary server data.</summary>
    private static void InitializeServerData()
    {

        packetHandlers = new Dictionary<int, PacketHandler>()
        {
           // { (int)ClientPackets.playerShoot, ServerHandle.PlayerShoot }
        };
        Debug.Log("Initialized packets.");
    }

    public static void Stop()
    {
        tcpListener.Stop();
    }
}