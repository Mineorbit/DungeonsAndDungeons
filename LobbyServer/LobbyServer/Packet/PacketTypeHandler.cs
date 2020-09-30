using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
public class PacketTypeHandler
{
    public static Type[] SubTypes;
    public PacketTypeHandler()
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
