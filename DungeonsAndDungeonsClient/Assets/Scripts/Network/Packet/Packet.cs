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

    public byte[] Compose()
    {
        return null;
    }
    byte[] Concat(byte[][] data)
    {
        int length = 0;
        for(int i = 0;i<data.Length;i++)
        {
            length  += data[i].Length;
        }
        byte[] returnData = new byte[length];
        int z = 0;
        for (int i = 0;i<length;i += data[i].Length)
        {
            for(int j = 0;j<data[i].Length;j++)
            {
                returnData[i + j] = data[z][j];
            }
            z++;
        }
        return returnData;
    }
    public virtual void OnReceive()
    {

    }
}
