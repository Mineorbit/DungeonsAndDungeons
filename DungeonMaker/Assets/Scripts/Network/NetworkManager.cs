using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager current;
    TcpClient client;
    NetworkStream stream;
    // Start is called before the first frame update
    void Start()
    {
        current = this;


    }
    public void ConnectToMain(string userName)
    {
        client = new TcpClient("127.0.0.1",13565);
        stream = client.GetStream();
        byte[] data  = new byte[32];
        data[0] = 42;
        byte[] namebytes = System.Text.Encoding.ASCII.GetBytes(userName); 
        for(int i = 0;i<namebytes.Length;i++)
        {
            data[i+1] = namebytes[i];
        }
        stream.Write(data, 0, data.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
