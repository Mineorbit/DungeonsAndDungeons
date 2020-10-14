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
        for (int i = 0; i < types.Length; i++)
        {
            if (types[i] == typeof(string))
            {
                int len = (int)data[z] * 256 + (int)data[z + 1];
                //HIER ENCODING
                byte[] stringData = new byte[len];
                Array.Copy(data, z + 2, stringData, 0, len);

                string t = Encoding.UTF8.GetString(stringData);
                content[i] = t;
                z += 2 + len;
            }
            else
            if (types[i] == typeof(int))
            {
                byte[] intData = { data[z], data[z + 1], data[z + 2], data[z + 3] };
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
        if (data.Length - 2 <= 0) return null;

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
        List<byte> contentData = new List<byte>();
        Debug.Log("Composing packet "+this.GetType());
        for(int i = 0;i<types.Length;i++)
        {
            Type t = types[i];
            object o = content[i];
            contentData.AddRange(AddOfType(t,o));
        }
        byte[] contentResult = contentData.ToArray();
        short length = ( (short) ( contentResult.Length + 1));
        byte[] front = {0,0 , packetId  };
        front[1] = (byte)(length & 0xff);
        front[0] = (byte)((length >> 8) & 0xff);
        byte[] packetData = new byte[3+contentResult.Length];
        Array.Copy(front,0,packetData,0,front.Length);
        Array.Copy(contentResult,0, packetData, 3,contentResult.Length);

        return packetData;
    }
    
    byte[] AddOfType(Type t, object o)
    {
        byte[] elementData = new byte[0];
        if (t == typeof(int))
        {
            elementData = new byte[4];
            elementData = BitConverter.GetBytes((int)o);
            foreach(byte b in elementData) Debug.Log(b);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(elementData);
        }
        else if (t == typeof(string))
        {
            //Hier noch das common encoding raussuchen
            byte[] stringData = Encoding.UTF8.GetBytes((string)o);
            byte[] lengthData = BitConverter.GetBytes((short)stringData.Length);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(lengthData);
            elementData = new byte[lengthData.Length + stringData.Length];
            Array.Copy(lengthData, elementData, 2);
            Array.Copy(stringData, 0, elementData, 2, stringData.Length);


        }
        return elementData;
    }

    public virtual void OnReceive()
    {

    }
    public virtual void OnReceive(int localId)
    {

    }
}
