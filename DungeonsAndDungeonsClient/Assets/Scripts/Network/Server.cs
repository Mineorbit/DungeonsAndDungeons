using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

   

    public static void StopServer()
    {
        foreach(Client c in clients)
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
