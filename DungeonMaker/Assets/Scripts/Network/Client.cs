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

//Courtesy of Tom Weiland, still a lot required tho
public class Client : MonoBehaviour
{
    public static Client instance;

    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 13565;
    public TCP tcp;

    public bool isConnectedGame =  false;
    private bool isConnected = false;
    public bool lobbyConnect = false;
    public bool gameConnect = false;
    private delegate void PacketHandler(Packet _packet);
    private static Dictionary<int, PacketHandler> packetHandlers;


    //SpielInfo

    public static TMP_Text info;
    public int globalId = 0;
    public int localId = 0;
    public string name = "Test";



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        InitializeClientData();
    }

    private void Start()
    {
        tcp = new TCP();
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    //Disconnected
    private void Disconnect()
    {
        //if (isConnected)
        //{
            Debug.Log("Test");
            if(lobbyConnect)
            {
                LobbyManager.instance.Stop();
    lobbyConnect = false;
            }
            if(gameConnect)
            {
                clearGameContext();
            }
            isConnected = false;
            tcp.socket.Close();
            updateNetworkMessage("Connection closed");
        //}
    }
  
    public void clearGameContext()
    {
            ClientSend.PlayerGameDisconnect();
    }
    public static void updateNetworkMessage(string infoT)
    {
        if(info==null)
        {
        GameObject i = GameObject.Find("Canvas").transform.Find("Debug").transform.Find("Info").gameObject;
        info = i.transform.GetComponent<TMP_Text>();
        }
    info.SetText(infoT);
    }
    public void ConnectToMainServer()
    {
        if(isConnected)
        {
            Disconnect();
        }
        Client.updateNetworkMessage($"Connecting to {ip}:{port}");

        port = 13565;
        isConnected = true;
        gameConnect = false;
        tcp.Connect(); 
    }
    public void ConnectToGameServer(string tip)
    {
        if(isConnectedGame) return;
        if(isConnected)
        {
            Disconnect();
        }
         Client.updateNetworkMessage($"Connecting to {ip}:{port}");

        ip = tip;
        port =  45565;
        isConnected = true;
        tcp.Connect();
        gameConnect = true;
    }

    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ServerPackets.ConnectInfo, ClientHandle.ConnectInfo },
            { (int)ServerPackets.GameReady, ClientHandle.GameReady },
            { (int)ServerPackets.Information, ClientHandle.Information },
            { (int)ServerPackets.PlayerLocomotionData, ClientHandle.PlayerLocomotionData},
            { (int)ServerPackets.PlayerGameDisconnect, ClientHandle.PlayerGameDisconnect}
        };
    }
    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
        }

        private void ConnectCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);

            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            receivedData = new Packet();
            if(Client.instance.gameConnect)
            {
                Client.instance.gameConnect = false;

                ClientSend.PlayerGameConnect(Client.instance.globalId,Client.instance.localId);
                
                ThreadManager.ExecuteOnMainThread(() =>
                {
                GameManager.current.startPlayMode();
                });
                
                

                Client.instance.isConnectedGame = true;
            }
            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to server via TCP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    instance.Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);
                receivedData.Reset(HandleData(_data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch
            {
                Disconnect();
            }
        }

        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 2)
            {
                _packetLength = (int) receivedData.ReadShort();
                if (_packetLength <= 0)
                {
                    return true; 
                }
            }
            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = (int)_packet.ReadByte();
                        packetHandlers[_packetId](_packet); 
                    }
                });

                _packetLength = 0; 
                if (receivedData.UnreadLength() >= 4)
                {
                    
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        
                        return true;
                    }
                }
            }

            if (_packetLength <= 1)
            {
                return true;
            }

            return false;
        }

        
        private void Disconnect()
        {
            instance.Disconnect();
            Debug.Log("Disconnecting TCP");
            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }
    }

    
}