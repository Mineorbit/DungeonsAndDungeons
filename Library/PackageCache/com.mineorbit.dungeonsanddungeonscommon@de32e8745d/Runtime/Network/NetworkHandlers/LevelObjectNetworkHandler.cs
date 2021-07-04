using System;
using System.Collections.Generic;
using Game;
using General;
using Google.Protobuf.WellKnownTypes;
using NetLevel;
using UnityEngine;
using Type = System.Type;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class LevelObjectNetworkHandler : NetworkHandler
    {

        private List<string> availableActions = new List<string>();

        public bool disabled_observed = true;


        public virtual LevelObject GetObserved()
        {
            return (LevelObject) observed;
        }
        
        public override void Awake()
        {
            observed = GetComponent<NetworkLevelObject>();
            base.Awake();
            // currently not updated
            if (observed != null)
                GetObserved().enabled = !disabled_observed || (!NetworkManager.isConnected || isOnServer);
        }


        // CALLABLE METHODS MUST BE MARKED PUBLIC TO BE USABLE
        [PacketBinding.Binding]
        public virtual void ProcessAction(Packet p)
        {
            LevelObjectAction levelObjectAction;
            if (p.Content.TryUnpack(out levelObjectAction))
            {
                var methodInfo = observed.GetType().GetMethod(levelObjectAction.ActionName);

                var parameters = new List<object>();
                foreach (var k in levelObjectAction.Params)
                {
                    var actionParam = ActionParam.Unpack(new Tuple<int, Parameter>(k.Key, k.Value));
                    parameters.Add(actionParam.data);
                }

                var paramObjects = new object[levelObjectAction.Params.Count];
                if (methodInfo != null)
                    MainCaller.Do(() => { methodInfo.Invoke(observed, parameters.ToArray()); });
                else
                    GameConsole.Log("The Method with the given name " + levelObjectAction.ActionName +
                              " could not be found, is it private?");
            }
        }


        // THIS NEEDS A SAFETY LIST LATER

        // NOT COMPLETED
        // THIS NEEDS TO PACK ARGUMENTS INTO ANY
        public virtual void SendAction(string actionName, ActionParam argument)
        {
            var action = new LevelObjectAction
            {
                ActionName = actionName
            };
            var arguments = new Dictionary<int, Parameter>();

            var packedArgument = argument.Pack();

            arguments.Add(packedArgument.Item1, packedArgument.Item2);

            action.Params.Add(arguments);
            Marshall(action,TCP: true);
        }


        public void SendAction(string actionName)
        {
            var action = new LevelObjectAction
            {
                ActionName = actionName
            };
            Marshall(action,TCP: true);
        }


        public class ActionParam
        {
            public object data;
            public int FieldPlace;
            public Type type;

            internal static ActionParam From<T>(T argument, int fieldPlace = 0)
            {
                var actionParam = new ActionParam();
                actionParam.FieldPlace = fieldPlace;
                actionParam.type = argument.GetType();
                actionParam.data = argument;
                return actionParam;
            }


            public static ActionParam Unpack(Tuple<int, Parameter> data)
            {
                var actionParam = new ActionParam();
                actionParam.FieldPlace = data.Item1;

                
                actionParam.type = Type.GetType(data.Item2.Type);

                if (actionParam.type == null)
                {
                    actionParam.type = Type.GetType(data.Item2.Type+", UnityEngine", true);
                }

                if(actionParam.type != null)
                {
                if (actionParam.type == typeof(ChunkData))
                {
                    var netChunkData = data.Item2.Value.Unpack<NetLevel.ChunkData>();
                    actionParam.data = netChunkData;
                }else if (actionParam.type == typeof(Vector3))
                {
                    Location l = data.Item2.Value.Unpack<Location>();
                    actionParam.data = new Vector3(l.X, l.Y, l.Z);
                }
                else if (actionParam.type == typeof(bool))
                {
                    actionParam.data = data.Item2.Value.Unpack<BoolValue>().Value;
                }
                else if (actionParam.type.IsSubclassOf(typeof(NetworkLevelObject)) || actionParam.type == typeof(NetworkLevelObject))
                {
                    var identityOfParam = data.Item2.Value.Unpack<StringValue>().Value;

                    var h = FindByIdentity<LevelObjectNetworkHandler>(identityOfParam);

                    // Debug.Log("Matched with Reference: " + identityOfParam + " " + h.observed);
                    actionParam.data = h.observed;
                }
                }
                else
                {
                    // Debug.Log("Actionparam was not defined "+data.Item2);
                    // Debug.Break();
                    
                }

                return actionParam;
            }


            public (int, Parameter) Pack()
            {
                Any x = null;


                // Debug.Log("Packing " + type);

                if (type == typeof(ChunkData))
                {
                    ChunkData chunkData = (ChunkData) data;
                    x = Any.Pack(chunkData);
                }
                else if (type == typeof(bool))
                {
                    var bv = new BoolValue();
                    bv.Value = (bool) data;
                    x = Any.Pack(bv);
                }else if (type == typeof(Vector3))
                {
                    Vector3 v = (Vector3) data;
                    Location l = new Location();
                    l.X = v.x;
                    l.Y = v.y;
                    l.Z = v.z;
                    x = Any.Pack(l);
                }
                else if (type.IsSubclassOf(typeof(NetworkLevelObject)) || type == typeof(NetworkLevelObject))
                {
                    var h = ((NetworkLevelObject) data).levelObjectNetworkHandler;
                    var sv = new StringValue();
                    sv.Value = h.Identity;
                    Debug.Log("Matched with Reference: " + h.Identity);
                    x = Any.Pack(sv);
                }

                var p = new Parameter
                {
                    Type = type.FullName,
                    Value = x
                };
                return (FieldPlace, p);
            }
        }


        // Not yet needed
        /*
        public void SendAction(string actionName, object param)
        {
            if (availableActions.Contains(actionName))
            {

            }
        }
        */
    }
}