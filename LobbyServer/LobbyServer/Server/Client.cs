using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

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
    
    public void ConnectCallBack(IAsyncResult result)
    {

        tcp.EndConnect(result);
        
        if (!tcp.Connected)
        {
            return;
        }

        ns = tcp.GetStream();
        receiveBuffer = new byte[dataBufferSize];
        
        ns.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        

    }
    private void ReceiveCallback(IAsyncResult _result)
    {
        int length = ns.EndRead(_result);
            byte[] data = new byte[length];
            Array.Copy(receiveBuffer, data, length);
            Packet p = Packet.Parse(data);
            //Hier andere behandlung für den fall das packet nicht parsebar
            if(p != null)
            p.OnReceive();
    }
    
    public void Disconnect()
    {
        //Sende hier noch PlayerDisconnected Packet an alle in Lobby
        ns.Close();
        tcp.Close();
    }
    public void Send(Packet p)
    {
        byte[] data = p.Compose();
        ns.Write(data,0,data.Length);
    }
    

}