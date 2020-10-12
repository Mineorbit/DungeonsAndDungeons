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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Events;

//Client from View of Server
public class Client
{

    int dataBufferSize = 32;
    byte[] receiveBuffer;
    public string ip = "127.0.0.1";
    public int port = 13586;
    public TcpClient tcp;
    NetworkStream ns;


    int localId;

    public Client(int lId, TcpClient client)
    {
        tcp = client;
        ns = client.GetStream();
        localId = lId;
    }

   
  //REWRITE

    public void StartRead()
    {
        if (tcp.Connected)
        {
            ns = tcp.GetStream();
            receiveBuffer = new byte[dataBufferSize];
            ns.ReadAsync(receiveBuffer, 0, 32);
            StartRead();
        }
    }

  //REWRITE
    void ProcessPacket(byte[] data)
    {
        ThreadManager.ExecuteOnMainThread(() => {
            Packet p = Packet.Parse(data);
            Debug.Log("Hussa");
            //Hier andere behandlung für den fall das packet nicht parsebar
            if (p != null)
                p.OnReceive(localId);
            if (localId == -1)
                p.OnReceive();
        });
    }
    public void Disconnect(UnityEvent unityEvent)
    {
        Disconnect();
        unityEvent.Invoke();
    }
    public void Disconnect()
    {
        PlayerDisconnectedPacket packet = new PlayerDisconnectedPacket("Default",localId);
        Send(packet);
        ns.Close();
        tcp.Close();
        Remove();
    }

    public void Remove()
    {
        ns = null;
    }
    public void Send(Packet p)
    {
        byte[] data = p.Compose();
        ns.Write(data, 0, data.Length);
    }


}