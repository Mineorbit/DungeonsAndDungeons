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

public class Client
{

    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 13565;
    public TcpClient tcp;
    

    
    public void Connect()
    {
        tcp = new TcpClient(ip,port);
    }

}