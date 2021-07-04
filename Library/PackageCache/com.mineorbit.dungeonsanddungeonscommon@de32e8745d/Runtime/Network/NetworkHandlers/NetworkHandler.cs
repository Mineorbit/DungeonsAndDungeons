using System;
using System.Collections.Generic;
using Game;
using General;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using UnityEngine;
using UnityEngine.Events;
using Type = System.Type;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class NetworkHandler : MonoBehaviour
    {
        public delegate void ParamsAction(Dictionary<string, object> arguments);

        public static bool isOnServer;


        public static List<Type> loadedTypes = new List<Type>();

        // first type in  key (Packet) second (Handler)
        public static Dictionary<Tuple<Type, Type>, UnityAction<Packet>> globalMethodBindings =
            new Dictionary<Tuple<Type, Type>, UnityAction<Packet>>();

        private static int count;
        public static Dictionary<int, NetworkHandler> bindRequests = new Dictionary<int, NetworkHandler>();

        public string _Identity;

        public bool identified;

        public Component observed;

        public string Identity
        {
            get => _Identity;
            set
            {
                identified = true;
                _Identity = value;
                OnIdentify();
            }
        }

        public virtual void OnIdentify()
        {
        }

        public virtual void OnDestroy()
        {
            NetworkManager.networkHandlers.Remove(this);
        }

        //Fetch Methods
        public virtual void Awake()
        {

            enabled = Level.instantiateType == Level.InstantiateType.Play ||
                      Level.instantiateType == Level.InstantiateType.Default ||
                      Level.instantiateType == Level.InstantiateType.Online;
            
            if (!enabled) return;
            isOnServer = Server.instance != null;
            NetworkManager.networkHandlers.Add(this);
            
            
            
            SetupLocalMarshalls();
            if (isOnServer)
                Identity = GetInstanceID().ToString();
        }


        public virtual void Start()
        {
            if (!isOnServer)
                StartRequestBind(this);
        }


        public void ConnectLevelObject(int rn)
        {
            var connectLevelObject = new ConnectLevelObject
            {
                Identity = Identity,
                HandlerType = GetType().FullName,
                ResponseNumber = rn
            };
            Marshall(GetType(), connectLevelObject);
        }


        private NetworkHandler FindIdentifiedInParents(NetworkHandler h)
        {
            var p = h.transform.parent.GetComponentInParent<NetworkHandler>();
            while (p != null && !p.identified) p = h.transform.parent.GetComponentInParent<NetworkHandler>();
            return p;
        }

        public static void StartRequestBind(NetworkHandler handler)
        {
            if (!handler.identified)
            {
                ConnectLevelObjectRequest connectLevelObjectRequest = null;
                NetworkHandler parentNetworkHandler = null;
                if (handler.transform.parent != null)
                    parentNetworkHandler = handler.transform.parent.GetComponentInParent<NetworkHandler>();
                Debug.Log(handler + " has " + parentNetworkHandler);
                if (parentNetworkHandler != null && parentNetworkHandler.identified)
                {
                    Debug.Log("Connecting " + handler.gameObject.name + " via parent");
                    connectLevelObjectRequest = new ConnectLevelObjectRequest
                    {
                        ParentIdentity = parentNetworkHandler.Identity,
                        HandlerType = handler.GetType().FullName,
                        RequestNumber = count,
                        RequestType = ConnectLevelObjectRequest.Types.RequestType.ByParent
                    };
                }
                else if (parentNetworkHandler == null)
                {
                    Debug.Log("Connecting " + handler.gameObject.name + " via position");
                    connectLevelObjectRequest = new ConnectLevelObjectRequest
                    {
                        X = handler.transform.position.x,
                        Y = handler.transform.position.y,
                        Z = handler.transform.position.z,
                        HandlerType = handler.GetType().FullName,
                        RequestNumber = count,
                        RequestType = ConnectLevelObjectRequest.Types.RequestType.ByPosition
                    };
                }
                else
                {
                    handler.Invoke("TryAfter", 2f);
                    return;
                }


                bindRequests.Add(count, handler);
                count++;
                Marshall(typeof(LevelObjectNetworkHandler), connectLevelObjectRequest);
            }
        }


        private void TryAfter()
        {
            Debug.Log(this + " trying again");
            StartRequestBind(this);
        }


        [PacketBinding.Binding]
        public static void OnConnectLevelObject(Packet p)
        {
            ConnectLevelObject connectLevelObjectResponse;
            if (p.Content.TryUnpack(out connectLevelObjectResponse))
            {
                NetworkHandler bindedHandler;
                if (bindRequests.TryGetValue(connectLevelObjectResponse.ResponseNumber, out bindedHandler))
                    MainCaller.Do(() =>
                    {
                        Debug.Log("Processing Rebind Response");
                        bindedHandler.Identity = connectLevelObjectResponse.Identity;
                        bindRequests.Remove(connectLevelObjectResponse.ResponseNumber);
                    });
            }
        }

        [PacketBinding.Binding]
        public static void OnConnectLevelObjectRequestFail(Packet p)
        {
            ConnectLevelObjectFail connectLevelObjectFail;
            if (p.Content.TryUnpack(out connectLevelObjectFail))
            {
                NetworkHandler networkHandler;
                if (bindRequests.TryGetValue(connectLevelObjectFail.ResponseNumber, out networkHandler))
                    MainCaller.Do(() => { StartRequestBind(networkHandler); });
            }
        }


        [PacketBinding.Binding]
        public static void OnConnectLevelObjectRequest(Packet p)
        {
            MainCaller.Do(() =>
            {
                var eps = 0.25f;
                ConnectLevelObjectRequest levelObjectConnect;
                if (p.Content.TryUnpack(out levelObjectConnect))
                {
                    Debug.Log("Handling " + levelObjectConnect.HandlerType + " " + levelObjectConnect.RequestType);
                    NetworkHandler fittingHandler = null;
                    var handlerType = Type.GetType(levelObjectConnect.HandlerType);
                    if (levelObjectConnect.RequestType == ConnectLevelObjectRequest.Types.RequestType.ByPosition)
                    {
                        var handlerPosition = new Vector3(levelObjectConnect.X, levelObjectConnect.Y,
                            levelObjectConnect.Z);
                        fittingHandler = NetworkManager.networkHandlers.Find(x =>
                        {
                            if (x != null)
                            {
                                var distance = (handlerPosition - x.transform.position).magnitude;
                                return x.GetType() == handlerType && distance < eps;
                            }
                            else
                                return false;
                        });
                    }
                    else if (levelObjectConnect.RequestType == ConnectLevelObjectRequest.Types.RequestType.ByParent)
                    {
                        fittingHandler = NetworkManager.networkHandlers.Find(x =>
                        {
                            // THIS MIGHT BE JANKY
                            NetworkHandler p = null;
                            if(x != null)
                            {
                            if (x.transform.parent != null)
                            {
                                p = x.transform.parent.GetComponentInParent<NetworkHandler>();
                            }
                            else
                            {
                                p = x.transform.GetComponentInParent<NetworkHandler>();
                            }
                            return x.GetType() == handlerType && p.identified && p.Identity
                                == levelObjectConnect.ParentIdentity;
                            }
                            else
                            {
                                return false;
                            }
                        });
                    }


                    if (fittingHandler != null)
                    {
                        fittingHandler.ConnectLevelObject(levelObjectConnect.RequestNumber);
                    }
                    else
                    {
                        Debug.Log("No " + handlerType + " found");

                        var connectLevelObjectFail = new ConnectLevelObjectFail
                        {
                            HandlerType = levelObjectConnect.HandlerType,
                            ResponseNumber = levelObjectConnect.RequestNumber
                        };

                        //THIS NEEDS TO BE FOUND IN PACKET

                        var requester = p.Sender;
                        Marshall(typeof(LevelObjectNetworkHandler), connectLevelObjectFail, requester);
                    }
                }
            });
        }


        public virtual void SetupLocalMarshalls()
        {
        }

        public void AddMethodMarshalling(Type packetType, UnityAction<Packet> process)
        {
            var handlerType = GetType();
            var t = new Tuple<Type, Type>(packetType, handlerType);
            if (!globalMethodBindings.ContainsKey(t))
                globalMethodBindings.Add(t, process);
        }


        public static void UnMarshall(Packet p)
        {
            var packetTypeString = p.Type;
            var packetType = Type.GetType(packetTypeString);
            var packetHandlerString = p.Handler;
            var networkHandlerType = Type.GetType(packetHandlerString);


            var handlerType = networkHandlerType;
            UnityAction<Packet> handle = null;
            while (!(globalMethodBindings.TryGetValue(new Tuple<Type, Type>(packetType, handlerType), out handle) ||
                     handlerType == typeof(NetworkHandler))) handlerType = handlerType.BaseType;
            if (handle != null)
            {
                handle.Invoke(p);
            }
        }

        public static T FindByIdentity<T>(string identity) where T : NetworkHandler
        {
            return (T) NetworkManager.networkHandlers.Find(x => x.Identity == identity);
        }


        /*
        public void AddMethodMarshalling(Action<Dictionary<string, object>> a, string name = "")
        {
            string actionName = name;
            if(name == "")
            {
                actionName = a.Method.Name;
            }
            methods.Add(actionName,a);
        }
        */


        // eventually type strings are not jet correcty matched
        public void Marshall(IMessage message, bool TCP = true)
        {
            var packet = new Packet
            {
                Type = message.GetType().FullName,
                Handler = GetType().FullName,
                Content = Any.Pack(message),
                Identity = Identity
            };

            if (!isOnServer)
                NetworkManager.instance.client.WritePacket(packet, TCP);
            else
                Server.instance.WriteAll(packet, TCP);
        }


        // THIS IS FOR UNIDENTIFIED CALLS ONLY
        public static void Marshall(Type sendingHandler, IMessage message, bool TCP = true)
        {
            var packet = new Packet
            {
                Type = message.GetType().FullName,
                Handler = sendingHandler.FullName,
                Content = Any.Pack(message)
            };

            if (!isOnServer)
            {
                if (NetworkManager.instance != null)
                {
                    if (NetworkManager.instance.client != null)
                    {
                        NetworkManager.instance.client.WritePacket(packet, TCP);
                    }
                }
            }
            else
            {
                if(Server.instance != null)
                    Server.instance.WriteAll(packet, TCP);
            }
        }

        // THIS IS FOR UNIDENTIFIED CALLS ONLY
        public static void Marshall(Type sendingHandler, IMessage message, string identity, bool TCP = true)
        {
            var packet = new Packet
            {
                Type = message.GetType().FullName,
                Handler = sendingHandler.FullName,
                Content = Any.Pack(message),
                Identity = identity
            };

            if (!isOnServer)
                NetworkManager.instance.client.WritePacket(packet, TCP);
            else
                Server.instance.WriteAll(packet, TCP);
        }


        public static void Marshall(Type sendingHandler, IMessage message, int target, bool TCP = true)
        {
            var packet = new Packet
            {
                Type = message.GetType().FullName,
                Handler = sendingHandler.FullName,
                Content = Any.Pack(message)
            };

            if (!isOnServer)
            {
                NetworkManager.instance.client.WritePacket(packet, TCP);
            }
            else
            {
                if (Server.instance.clients[target] != null)
                    Server.instance.clients[target].WritePacket(packet, TCP);
            }
        }


        public static void Marshall(Type sendingHandler, IMessage message, int target, string identity, bool TCP = true)
        {
            var packet = new Packet
            {
                Type = message.GetType().FullName,
                Handler = sendingHandler.FullName,
                Content = Any.Pack(message),
                Identity = identity
            };

            if (!isOnServer)
            {
                NetworkManager.instance.client.WritePacket(packet);
            }
            else
            {
                if (Server.instance.clients[target] != null)
                    Server.instance.clients[target].WritePacket(packet, TCP);
            }
        }


        public void Marshall(IMessage message, int target, bool toOrWithout = true, bool TCP = true)
        {
            var packet = new Packet
            {
                Type = message.GetType().ToString(),
                Handler = GetType().ToString(),
                Content = Any.Pack(message),
                Identity = Identity
            };

            if (!isOnServer)
            {
                NetworkManager.instance.client.WritePacket(packet, TCP);
            }
            else
            {
                if (toOrWithout)
                {
                    if (Server.instance.clients[target] != null)
                        Server.instance.clients[target].WritePacket(packet, TCP);
                }
                else
                {
                    Server.instance.WriteAll(packet, target, TCP);
                }
            }
        }
    }
}
