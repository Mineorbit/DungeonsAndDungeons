using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class Packet
{
    Type[] types;
    object[] content;
    
    public bool TypeCheck()
    {
        for(int i = 0;i<types.Length;i++)
        {
            Type s = types[i];
            if (content[i].GetType() !=  s) return false;
        }
        return true;
    }
    static Packet setPacketType()
    {
        return null;
    }
    public static Packet Parse(byte[] data)
    {
        return null;
    }
    public virtual void OnReceive()
    {

    }
}
