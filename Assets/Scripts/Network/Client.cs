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

//Client used in game Client
public class Client : MonoBehaviour
{
    string name;

    int dataBufferSize = 8192;
    byte[] receiveBuffer;
    public string ip = "127.0.0.1";
    public int port = 13586;
    public TcpClient tcp;
    NetworkStream ns;

    //Connecting
    bool retry = true;
    int tries;

    //Events
    
    UnityEvent connectEvent;
    UnityEvent disconnectEvent;

    public void Setup(string nip, int nport,string nname)
    {
        ip = nip;
        port = nport;
        name = nname;
    }

    public void Kill()
    {
        Disconnect();
        Dispose();
    }
    public void Dispose()
    {

        ns.Close();
        tcp.Close();
        Destroy(this);
    }

    void TryConnect(UnityEvent onConnectEvent)
    {
        connectEvent = onConnectEvent;
        retry = true;
        tries = 0;
        TryConnect();
    }
    void TryConnect()
    {
        tcp.BeginConnect(IPAddress.Parse(ip), port, new AsyncCallback(ConnectAttempt), null);
    }
    void ConnectAttempt(IAsyncResult result)
    {
        tries++;
        if(tries == 256)
        {
            retry = false;
        }
        if(!tcp.Connected && retry)
        {
            Report("Failed to Connect, trying again");
            TryConnect();
        }else if(!retry)
        {
            Report("Failed to Connect, not trying again");
            ThreadManager.ExecuteOnMainThread(() =>
            {
                AlertScreen.alert.Close();
            });
        }
        else
        {
            ThreadManager.ExecuteOnMainThread(()=> {

                Report("Connected");
                ns = tcp.GetStream();
                connectEvent.Invoke();
                StartRead();
            });
        }
    }

    public void Connect(UnityEvent onConnectEvent)
    {
        if (tcp == null)
        {
            tcp = new TcpClient();
        }
        if(!tcp.Connected)
        {
        tries = 0;
        Debug.Log($"[{name}] Connecting on {port}");
        retry = true;
        TryConnect(onConnectEvent);
        }
        
    }
    public void Report(string m)
    {
        if (tcp != null)
        {
            Debug.Log($"[{name}] {ip} : {port}  | {tcp.Connected} | "+m);
        }
        else
        {
            Debug.Log($"[{name}] {ip} : {port}  | No internal TcpClient |"+m);
        }
    }

    public void Report()
    {
        if(tcp!=null)
        {
            Debug.Log($"[{name}] {ip} : {port}  | {tcp.Connected}");
        }
        else
        {
            Debug.Log($"[{name}] {ip} : {port}  | No internal TcpClient");
        }
    }
    public void CancelConnect()
    {
        retry = false;
        Disconnect();
    }

    // hier noch einen try catch für SocketException USW
    public void StartRead()
    {
        if (tcp.Connected)
        {
            ns = tcp.GetStream();
            receiveBuffer = new byte[dataBufferSize];
            try
            {
            Report("Waiting for new Packet");
            ns.BeginRead(receiveBuffer, 0, dataBufferSize, new AsyncCallback(HandleRead), null);
            }catch(Exception e)
            {
                StartRead();
            }
        }
    }


    public void HandleRead(IAsyncResult r)
    {
        if (!tcp.Connected) return;
        int len = ((int)receiveBuffer[0]) * 256 + (int)receiveBuffer[1];
        if(len == 0)
        {
            Disconnect();
        }
        byte[] packetData = new byte[len + 2];
        Array.Copy(receiveBuffer, 0, packetData, 0, len + 2);
        ProcessPacket(packetData);
        StartRead();
    }

    public void OnConnect()
    {

    }

    public void OnDisconnect()
    {
        Debug.Log("Disposing");
        if(disconnectEvent!=null)
        disconnectEvent.Invoke();
        Dispose();
    }

    //REWRITE
    void ProcessPacket(byte[] data)
    {
        ThreadManager.ExecuteOnMainThread(() => {
            Packet p = Packet.Parse(data);
            string s = "";
            foreach (byte b in data)
            {
                s += " " + b;
            }
            Report("Received Packet "+p+" | "+s);
            //Hier andere behandlung für den fall das packet nicht parsebar
            if(p!=null)
            p.OnReceive();
        });
    }


    public void Send(Packet p)
    {
        if(tcp!=null)
        {
            if(tcp.Connected)
            {
                byte[] data = p.Compose();
                ns.Write(data,0,data.Length);
                ns.Flush();
            }
        }
    }
    public void Disconnect(string r)
    {
        if (tcp != null)
            if (tcp.Connected)
            {
                PlayerDisconnectedPacket p = new PlayerDisconnectedPacket(r,NetworkManager.instance.localId);
                Send(p);
                Report("Disconnected");
                //NetworkManager.instance.Reset();
            }
    }
    public void Disconnect()
    {
        Disconnect("default");
    }
    public void Disconnect(UnityEvent onDisconnectEvent)
    {
        disconnectEvent = onDisconnectEvent;
        Disconnect();
        OnDisconnect();
    }
    public void OnDisable()
    {
        Disconnect("Player quit game");
    }
    

}