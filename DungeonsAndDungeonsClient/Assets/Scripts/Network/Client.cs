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

using UnityEngine.Events;

public class Client
{

    int dataBufferSize = 1024;
    byte[] receiveBuffer;
    public string ip = "127.0.0.1";
    public int port = 13565;
    public TcpClient tcp;
    NetworkStream ns;

    public void Connect()
    {
        tcp = new TcpClient();
        tcp.BeginConnect(ip,port, new AsyncCallback(ConnectCallBack),tcp);
    }
    public void Connect(UnityEvent onConnectEvent)
    {
        tcp = new TcpClient();
        tcp.BeginConnect(ip, port, new AsyncCallback(ConnectCallBack), onConnectEvent);
    }
    public void ConnectCallBack(IAsyncResult result)
    {

        tcp.EndConnect(result);
        
        if (!tcp.Connected)
        {
            return;
        }

        ns = tcp.GetStream();
        receiveBuffer = new byte[dataBufferSize];
        if (result.AsyncState is UnityEvent)
        {

            ThreadManager.ExecuteOnMainThread(
                () => {
                UnityEvent connectEvent = (UnityEvent)result.AsyncState;
                connectEvent.Invoke();
                }
            );
        }
        ns.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        

    }
    private void ReceiveCallback(IAsyncResult _result)
    {
        int length = ns.EndRead(_result);
        ThreadManager.ExecuteOnMainThread(()=> {
            byte[] data = new byte[length];
            Array.Copy(receiveBuffer, data, length);
            Packet p = Packet.Parse(data);
            //Hier andere behandlung für den fall das packet nicht parsebar
            if(p != null)
            p.OnReceive();
        });
    }
    
    public void Disconnect(UnityEvent unityEvent)
    {
        PlayerDisconnectedPacket packet = new PlayerDisconnectedPacket();
        Send(packet);
        ns.Close();
        tcp.Close();
        unityEvent.Invoke();
    }
    public void Send(Packet p)
    {

    }
    

}