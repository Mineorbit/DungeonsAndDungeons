using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Text;
 using System;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager current;
    public static  int dataBufferSize = 4096;
    public string ip = "127.0.0.1";
    public int port = 13565;
    public static int localId;
    public TCP tcp;
    Text networkInfo;
    private void Awake()
    {
        if(current == null)
        {
            current = this;
        }else if(current != this)
        {
            Destroy (this);
        }


    }
    public void StatusUpdate(string s)
    {
        networkInfo.text = s;
    }
    public void StatusUpdateError(string s)
    {
        
    }
    private void Start()
    {
        networkInfo = GameObject.Find("Overlay").transform.Find("NetworkInfo").GetComponent<Text>();
        tcp = new TCP();
        ConnectToServer();
    }
    public void ConnectToServer()
    {
        tcp.Connect();
    }



    public class TCP{
        public TcpClient socket;
        private NetworkStream stream;
        private byte[] receiveBuffer;
        public void Connect()
        {
            current.StatusUpdate($"Connecting to {current.ip}");
            socket =  new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };
            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(current.ip, current.port,ConnectCallback,socket);
        }
        private void ConnectCallback(IAsyncResult res)
        {
            socket.EndConnect(res);

            current.StatusUpdate($"Connected");
            if(!socket.Connected)
            {
                return;
            }
            stream = socket.GetStream();
            stream.BeginRead(receiveBuffer,0,dataBufferSize,ReceiveCallback,null);
        }
        private void ReceiveCallback(IAsyncResult res)
        {
           try
                {
                    int _byteLength = stream.EndRead(res);
                    if (_byteLength <= 0)
                    {
                        // TODO: disconnect
                        return;
                    }

                    byte[] _data = new byte[_byteLength];
                    Array.Copy(receiveBuffer, _data, _byteLength);

                    // TODO: handle data
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch (Exception _ex)
                {
                    NetworkManager.current.StatusUpdateError($"Error receiving TCP data: {_ex}");
                    // TODO: disconnect
                }
        }
    }
}
