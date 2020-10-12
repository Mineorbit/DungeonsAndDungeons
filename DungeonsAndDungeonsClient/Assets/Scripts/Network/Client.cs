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

    int dataBufferSize = 1024;
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


    public void Setup(string nip, int nport,string nname)
    {
        ip = nip;
        port = nport;
        name = nname;
    }

    void TryConnect(UnityEvent onConnectEvent)
    {
        connectEvent = onConnectEvent;
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
            Report("Connected");
            ThreadManager.ExecuteOnMainThread(()=> {
                ns = tcp.GetStream();
                connectEvent.Invoke();
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
    public void Send(Packet p)
    {
        if(tcp!=null)
        {
            if(tcp.Connected)
            {
                byte[] data = p.Compose();
                string s = "";
                foreach(byte b in data)
                {
                    s += b;
                }
                Report("Sending: "+s);
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
                ns.Close();
                tcp.Close();
                ns = null;
                tcp = null;
            }
        Report("Disconnected");
    }
    public void Disconnect()
    {
        Disconnect("default");
    }
    public void Disconnect(UnityEvent onDisconnectEvent)
    {
        Disconnect();
        onDisconnectEvent.Invoke();
    }
    

}