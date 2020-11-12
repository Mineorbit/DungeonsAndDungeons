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

    int dataBufferSize = 4096*4;
    byte[] receiveBuffer;
    public string ip = "127.0.0.1";
    public int port = 13586;
    public TcpClient tcp;
    NetworkStream ns;

    int localId;

    public string name;

    public Client(int lId, TcpClient client)
    {
        tcp = client;
        ns = client.GetStream();
        localId = lId;
    }

   public void Kill()
    {
        ns.Close();
        tcp.Close();
        ns = null;
        tcp = null;
    }
  //REWRITE

    public void StartRead()
    {
        if (tcp.Connected)
        {
            //ns = tcp.GetStream();
            receiveBuffer = new byte[dataBufferSize];
            Report("Waiting for Packet");
            ns.BeginRead(receiveBuffer,0,dataBufferSize,new AsyncCallback(HandleRead),null);
        }
    }


    public void HandleRead(IAsyncResult r)
    {

        if (!tcp.Connected) return;
        int len = ((int) receiveBuffer[0])*256 + (int) receiveBuffer[1];

        byte[] packetData = new byte[len+2];
        Array.Copy(receiveBuffer,0,packetData,0, len+2);
        ProcessPacket(packetData);
        StartRead();
    }
  //REWRITE
    void ProcessPacket(byte[] data)
    {
        ThreadManager.ExecuteOnMainThread(() => {
            Packet p = Packet.Parse(data);
            //Hier andere behandlung für den fall das packet nicht parsebar
            if (p != null)
                p.OnReceive(localId);
            if (localId == -1)
                p.OnReceive();
        });
    }


    public void Disconnect(UnityEvent unityEvent)
    {
        unityEvent.Invoke();
        Disconnect();
    }
    public void Disconnect()
    {
        if(tcp.Connected)
        {
        PlayerDisconnectedPacket packet = new PlayerDisconnectedPacket("Default",localId);
        Send(packet);
        Kill();
        }
    }
    
    
    public void Report(string m)
    {
        if (tcp != null)
        {
            Debug.Log($"[{localId}] {ip} : {port}  | {tcp.Connected} | " + m);
        }
        else
        {
            Debug.Log($"[{localId}] {ip} : {port}  | No internal TcpClient |" + m);
        }
    }

    public void Report()
    {
        if (tcp != null)
        {
            Debug.Log($"[{localId}] {ip} : {port}  | {tcp.Connected}");
        }
        else
        {
            Debug.Log($"[{localId}] {ip} : {port}  | No internal TcpClient");
        }
    }
    public void Send(Packet p)
    {
        if(Server.GetClient(localId)!=null)
        if (tcp != null)
        {
            if (tcp.Connected)
            {
                byte[] data = p.Compose();
                    string s = "";
                    foreach (byte b in data)
                    {
                        s += " " + b;
                    }
                    Report("Sending Packet " + p+" "+data.Length+ " | "+s);
                    
                    ns.Write(data, 0, data.Length);
                ns.Flush();
            }
        }
    }


}