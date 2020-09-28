using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Text;
public class Packet
{
    byte packetId;
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
        byte[][] contentData = new byte[content.Length][];
        for(int i = 0;i<content.Length;i++)
        {
            object o = content[i];
            if(o.GetType() is int)
            { 
                    contentData[i] = BitConverter.GetBytes((int) o);
            }else if(o.GetType() is string)
            { 
                //Hier noch das common encoding raussuchen
                    contentData[i] = Encoding.ASCII.GetBytes((string) o);                     
            }
        }
        byte[] contentResult = Concat(contentData);
        short length = (short) contentResult.Length;
        byte[] front = {0,0 , packetId  };
        front[0] = (byte)(length & 0xff);
        front[1] = (byte)((length >> 8) & 0xff);
        byte[] packetData = new byte[2+contentResult.Length];
        front.CopyTo(packetData,0);
        contentResult.CopyTo(packetData, 3 + contentResult.Length);
        return packetData;
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
