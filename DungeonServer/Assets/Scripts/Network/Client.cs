using System;

using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client
{

    Encoding enc = Encoding.Unicode;

    public byte[] buffer;
    int bufferSize = 4096;

    public int globalId;

    public int localId;
    public Player player;
    public TcpClient tcpClient;

    public enum PacketType { Message = 1};

    public Client(int _clientId)
    {
        globalId = _clientId;
        buffer = new byte[bufferSize];
    }

    public void Send(byte[] data)
    {

    }
    public static Client Connect(int id, TcpClient client)
    {
        Client c = new Client(id);
        c.tcpClient = client;
        return c;
    }
    public void Disconnect()
    {
        tcpClient.GetStream().Close();
        tcpClient.Close();
    }
    Byte[] StringToByte(String s)
    {
        byte[] stringData = enc.GetBytes(s);
        return stringData;
    }
    Byte[] ShortToByte(short s)
    {
        return BitConverter.GetBytes(s);
    }
    public void sendMessage(string message)
    {
        int messageLength = System.Text.ASCIIEncoding.Unicode.GetByteCount(message);
        byte[] data = new byte[2+message.Length+messageLength];
        byte[] idData = ShortToByte((short) PacketType.Message);
        data[0] = idData[0];
        data[1] = idData[1];
        byte[] mData = StringToByte(message);
        for(int i = 0; i < messageLength; i++)
        {
            data[2 + i] = mData[i];
        } 
        
        
    }

    public void SendPacket(byte[] data)
    {
        tcpClient.GetStream().Write(data,0,data.Length);
    }

    public void Disconnect(string message)
    {

        Disconnect();
    }
}