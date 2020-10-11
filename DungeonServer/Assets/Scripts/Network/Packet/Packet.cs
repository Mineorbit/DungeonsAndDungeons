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
                //Aufpassen wegen Encoding
                
            }
            else
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
    
    public List<byte> parseContent(List<byte> contentData)
    {
        for (int i = 0; i < types.Length; i++)
        {
            Type t = types[i];
            object o = content[i];
            Debug.Log(t);
            if (t is int)
            {
                byte[] intData = BitConverter.GetBytes((int)o);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(intData);
                contentData.AddRange(intData);
            }
            else if (t == typeof(string))
            {
                //Hier noch das common encoding raussuchen
                byte[] stringData = Encoding.ASCII.GetBytes((string)o);
                byte[] lengthData = BitConverter.GetBytes((short)stringData.Length);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(lengthData);
                byte[] elementData = new byte[lengthData.Length + stringData.Length];
                Array.Copy(lengthData, elementData, 2);
                Array.Copy(stringData, 0, elementData, 2, stringData.Length);


                contentData.AddRange(elementData);
            }
        }
        return contentData;
    }

    public byte[] Compose()
    {
        List<byte> contentData = new List<byte>();
        contentData = parseContent(contentData);
        byte[] contentResult = contentData.ToArray();
        short length = ( (short) ( contentResult.Length + 1));
        byte[] front = {0,0 , packetId  };
        front[1] = (byte)(length & 0xff);
        front[0] = (byte)((length >> 8) & 0xff);
        byte[] packetData = new byte[3+contentResult.Length];
        Array.Copy(front,0,packetData,0,front.Length);
        Array.Copy(contentResult,0, packetData, 3,contentResult.Length);

        foreach (byte b in packetData)
        {
            Debug.Log(b + " " + ((char)b));
        }
        return packetData;
    }
    
    public virtual void OnReceive()
    {

    }
}
