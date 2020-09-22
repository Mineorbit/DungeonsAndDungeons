using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class Server : MonoBehaviour
{
    public static Server server;
    Client[] clients;
    TcpListener tcpListener;
    void Start()
    {
        if (server != null) Destroy(this);
        server = this;
    }



    public  void Stop()
    {
        foreach (Client c in clients)
        {
            c.Disconnect();
        }
        Destroy(Server.server);
    }


    public static void CreateServer(Transform t)
    {
        t.gameObject.AddComponent<Server>();
    }


}
