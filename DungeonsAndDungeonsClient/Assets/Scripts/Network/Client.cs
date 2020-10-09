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

public class Client
{

    int dataBufferSize = 1024;
    byte[] receiveBuffer;
    public string ip = "127.0.0.1";
    public int port = 13565;
    public TcpClient tcp;
    NetworkStream ns;

    bool retry = true;
    public void Connect(UnityEvent onConnectEvent)
    {
        retry = true;
        SocketConnect(onConnectEvent);
    }
    
    public void CancelConnect()
    {
        retry = false;
    }
    public async Task SocketConnect(UnityEvent onConnectEvent)
    {
        var tcpClient = new TcpClient();
        while (retry)
        {
            try
            {
                await tcpClient.ConnectAsync(ip, port);
            }
            catch (Exception ex)
            {
                //handle errors
                continue;
            }
            if (tcpClient.Connected) { retry = false; break; }
        }
        if (tcpClient.Connected)
        {
            retry = false;
            ns = tcpClient.GetStream();
            receiveBuffer = new byte[dataBufferSize];
            onConnectEvent.Invoke();
            ns.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }
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
        byte[] data = p.Compose();
        ns.Write(data,0,data.Length);
    }
    

}