using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NetLevel;
using UnityEngine;
using UnityEngine.Events;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class NetworkManager : MonoBehaviour
    {
        public static NetworkManager instance;

        public static List<NetworkHandler> networkHandlers = new List<NetworkHandler>();

        public static bool isConnected;

        public Option useUDPOption;
        
        public static List<Client> allClients = new List<Client>();
        public static string userName;

        public static UnityEvent connectEvent = new UnityEvent();
        public static UnityEvent<LobbyRequest> lobbyRequestEvent = new UnityEvent<LobbyRequest>();
        public static UnityEvent disconnectEvent = new UnityEvent();
        public static UnityEvent prepareRoundEvent = new UnityEvent();
        public static UnityEvent startRoundEvent = new UnityEvent();
        public static UnityEvent winEvent = new UnityEvent();
        public static UnityEvent<Tuple<int, bool>> readyEvent = new UnityEvent<Tuple<int, bool>>();

        private static Action onConnectAction;

        public List<PacketBinding> packetBindings = new List<PacketBinding>();

        public int localId;
        
        public bool ready;

        public static List<Thread> threadPool = new List<Thread>();
        
        public Client client;

        
        // Temporary fix

        public bool useUDP;
        
        // Start is called before the first frame update
        private void Start()
        {
            if (instance != null)
                Destroy(this);
            instance = this;


            // THIS IS A STUPID PLACE BUT WILL CHANGE LATER
            Time.fixedDeltaTime = 0.02f;

            foreach (var p in packetBindings) p.AddToBinding();

            useUDP = (bool) useUDPOption.Value;

        }


        public void FixedUpdate()
        {
            foreach (var c in allClients) c.FixedUpdate();
        }

        //Factor this out into GameLogic


        public void OnDestroy()
        {
            Disconnect();
            KillThreads();
        }

        public void Connect(string ip, string playerName, Action onConnect)
        {
            if (!isConnected)
            {
                onConnectAction = onConnect;
                userName = playerName;
                client = Client.Connect(IPAddress.Parse(ip), 13565);
                GameConsole.Log("Set new client "+client);
                client.onConnectEvent.AddListener(OnConnected);
                client.onConnectEvent.AddListener((x) => { NetworkManager.networkHandlers = new List<NetworkHandler>();});
            }
        }

        public void OnConnected(int id)
        {
            MainCaller.Do(() =>
            {
                localId = id;
                isConnected = true;
                SetNetworkHandlers(isConnected);
                onConnectAction.Invoke();
            });
        }

        private void SetNetworkHandlers(bool v)
        {
            foreach (var h in networkHandlers) 
                {
                    if(h!=null)
                        h.enabled = v;
                }
        }

        public void Disconnect(bool respond = true)
        {
            if (!isConnected)
            {
                return;
            }

            isConnected = false;
                if (client != null)
                    client.Disconnect(respond);
                disconnectEvent.Invoke();
                
            KillThreads();
            PlayerManager.playerManager.Remove(localId);
        }

        public void CallReady(bool r)
        {
            ready = r;
            NetworkManagerHandler.RequestReadyRound();
        }

        public void CallLobbyReady()
        {
            NetworkManagerHandler.RequestReadyLobby();
        }

        public void CallSelected(LevelMetaData metaData)
        {
            NetworkManagerHandler.RequestLobbyUpdate(metaData);
        }

        public void KillThreads()
        {
            foreach (Thread t in threadPool)
            {
                if (t.IsAlive)
                {
                    GameConsole.Log($"Killing Subthread {t.Name}");
                    t.Abort();
                }
            }
        }

        public void OnApplicationQuit()
        {
            KillThreads();
        }

        public void OnDisable()
        {
            KillThreads();
        }
    }
}