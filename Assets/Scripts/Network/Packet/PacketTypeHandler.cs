using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
public class PacketTypeHandler : MonoBehaviour
{
    public static Type[] SubTypes;
    void Awake()
    {
        Type[] types = Assembly.GetAssembly(typeof(Packet)).GetTypes();
        Type[] subtypes;
        List<Type> list = new List<Type>();
        foreach (Type t in types)
        {
            if(t.BaseType == typeof(Packet))
            {
                list.Add(t);
            }
        }
        SubTypes = list.ToArray();
    }

    
}
