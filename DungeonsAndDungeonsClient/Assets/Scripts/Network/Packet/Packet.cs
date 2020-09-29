using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Text;
public class Packet
{
    public byte packetId;
    public Type[] types;
    public object[] content;
    
    public bool TypeCheck()
    {
        for(int i = 0;i<types.Length;i++)
        {
            Type s = types[i];
            if (content[i].GetType() !=  s) return false;
        }
        return true;
    }

    static Packet setPacketType(byte id)
    {
        Type[] subtypes = PacketTypeHandler.SubTypes;
        foreach (Type t in subtypes)
        {
            Packet instance = (Packet) Activator.CreateInstance(t);
            Debug.Log(instance.packetId);
            if(instance.packetId == id)
            {
                return instance;
            }

        }
        return null;
    }
    Packet parseContent(byte[] data)
    {
        int z = 0;
        for(int i = 0; i < types.Length;i++)
        {
            if(types[i] == typeof(string))
            {

            }else
            if(types[i] == typeof(int))
            {
                byte[] intData = {data[z], data[z+1], data[z+2], data[z+3] };
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(intData);
                content[i] = BitConverter.ToInt32(intData, 0);
                z += 4;
            }
        }
        return this;
    }
    public static Packet Parse(byte[] data)
    {
        Debug.Log("Parse packet");
        short length = (short) ( (256 * (int) data[0]) + (int) data[1]);
        byte[] shortened = new byte[data.Length-2];
        Array.Copy(data,2,shortened,0,length);
        byte id = shortened[0];
        byte[] content = new byte[shortened.Length-1];
        Array.Copy(shortened,1,content,0,shortened.Length-1);
        Packet newPacket = Packet.setPacketType(id);
        //nicht parsbarkeit besser handlen
        if (newPacket == null)
            return null;
        Packet packedPacket = newPacket.parseContent(content);
        return packedPacket;
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
