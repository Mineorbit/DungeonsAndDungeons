using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using General;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PacketBinding", order = 1)]
    public class PacketBinding : ScriptableObject
    {
        public List<string> possiblePacketTypes = new List<string>();
        public List<string> possibleHandlerTypes = new List<string>();
        public List<string> possibleMethods = new List<string>();


        public string PacketType;


        public string HandlerType;


        public List<string> BindedMethods;

        public bool withIdentity;

        public List<MethodInfo> bindedMethods;

        public Type handlerType;

        public Type packetType;

        public UnityAction<Packet> test;

        public void Awake()
        {
            GetLists();
            AddToBinding();
        }


        public void OnEnable()
        {
            GetLists();
            AddToBinding();
        }


        private void OnValidate()
        {
            packetType = Type.GetType(PacketType);
            handlerType = Type.GetType(HandlerType);

            GetPossibleMethods();
        }


        //Multiple action calls not yet supported but can be  done via combination
        public void AddToBinding()
        {
            packetType = Type.GetType(PacketType);
            handlerType = Type.GetType(HandlerType);
            if (packetType != null && handlerType != null)
                bindedMethods = BindedMethods.Select(x => { return handlerType.GetMethod(x); }).ToList();

            if (bindedMethods != null && bindedMethods.Count > 0)
                foreach (var methodInfo in bindedMethods)
                {
                    UnityAction<Packet> action = null;
                    if (withIdentity)
                        action = x =>
                        {
                            MainCaller.Do(
                                () =>
                                {
                                    var handler = NetworkHandler.FindByIdentity<NetworkHandler>(x.Identity);
                                    if(handler != null)
                                    {
                                        Delegate.CreateDelegate(typeof(UnityAction<Packet>), handler, methodInfo.Name, true, true).DynamicInvoke(x);
                                    }
                                    else
                                    {
                                        // GameConsole.Log("Handler for "+x.Identity+" not found");
                                    }
                                }
                            );
                        };
                    else
                        action = (UnityAction<Packet>) Delegate.CreateDelegate(typeof(UnityAction<Packet>), methodInfo);

                    var key = new Tuple<Type, Type>(packetType, handlerType);
                    if (!NetworkHandler.globalMethodBindings.ContainsKey(key))
                        NetworkHandler.globalMethodBindings.Add(key, action);
                }
        }

        private void GetLists()
        {
            List<Type> types;
            List<Type> networkHandlerTypes;
            possiblePacketTypes = new List<string>();
            possibleHandlerTypes = new List<string>();

            types = (
                from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                // alternative: from domainAssembly in domainAssembly.GetExportedTypes()
                from assemblyType in domainAssembly.GetTypes()
                where typeof(IMessage).IsAssignableFrom(assemblyType)
                // alternative: where assemblyType.IsSubclassOf(typeof(B))
                // alternative: && ! assemblyType.IsAbstract
                select assemblyType).ToList();


            networkHandlerTypes = (
                from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                // alternative: from domainAssembly in domainAssembly.GetExportedTypes()
                from assemblyType in domainAssembly.GetTypes()
                where typeof(NetworkHandler).IsAssignableFrom(assemblyType)
                // alternative: where assemblyType.IsSubclassOf(typeof(B))
                // alternative: && ! assemblyType.IsAbstract
                select assemblyType).ToList();

            possiblePacketTypes.AddRange(types.Select(x => { return x.FullName; }).ToList());
            possibleHandlerTypes.AddRange(networkHandlerTypes.Select(x => { return x.FullName; }).ToList());

            GetPossibleMethods();
        }

        private void GetPossibleMethods()
        {
            if (handlerType != null)
            {
                possibleMethods = new List<string>();
                var methods = handlerType.GetMethods()
                    .Where(m => m.GetCustomAttributes(typeof(Binding), false).Length > 0)
                    .ToList();
                possibleMethods.AddRange(methods.Select(x => { return x.Name; }).ToList());
            }
        }

        public class Binding : Attribute
        {
        }
    }
}