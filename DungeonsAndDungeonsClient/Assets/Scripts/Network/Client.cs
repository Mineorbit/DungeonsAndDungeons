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

public class Client
{

    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 13565;
    public TcpClient tcp;
    NetworkStream ns;

    public void Connect()
    {

        BlurScreen.blurScreen.Open();
        tcp = new TcpClient();
        tcp.BeginConnect(ip,port, new AsyncCallback(ConnectCallBack),tcp);
    }
    public void ConnectCallBack(IAsyncResult result)
    {

        tcp.EndConnect(result); 
        if (!tcp.Connected)
        {
            return;
        }

        ns = tcp.GetStream();
        Debug.Log("Verbunden");
        ns.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);


    }
    private void ReceiveCallback(IAsyncResult _result)
    {
        int length = ns.EndRead(_result);
    }
    public void Disconnect()
    {
    }
    

}