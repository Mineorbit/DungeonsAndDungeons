using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    
    //Für jede PaketArt einen Handler
    public static void ConnectInfo(Packet _packet)
    {

        Debug.Log($"Verbunden mit Server");
        int _myId = _packet.ReadInt();

        Client.instance.globalId = _myId;
        Client.updateNetworkMessage("Vebunden zum Server");
        ClientSend.PlayerConnect(Client.instance.name);
        ConnectBar.current.close();
        LobbyBar.current.Start();
        LobbyBar.current.open();

    }
    public static void Information(Packet _packet)
    {

        string info = _packet.ReadString();

        Debug.Log($"Message: {info}");
        Client.updateNetworkMessage(info);

    }
    public static void GameReady(Packet _packet)
    {
        PlayerData[] pData = new PlayerData[4];
        Debug.Log("Moin Client Packet empfangen");
        for(int i = 0;i<4;i++)
        {
            byte d = _packet.ReadByte();
            byte e = _packet.ReadByte();
            if(d==0)
            {
                pData[i] = null;
            }else
            {
                byte leftHand = d;
                byte rightHand = e;
                PlayerData data = new  PlayerData();
                data.localId = i;
                data.leftHand = (PlayerData.Item) ((int) leftHand);
                data.rightHand = (PlayerData.Item) ((int) rightHand);
                pData[i] =  data;
            }
        }
        GameManager.current.playerData = pData;
        PlayLogic.current.startRound();
    }
    public static void PlayerLocomotionData(Packet _packet)
    {
        int locId =_packet.ReadByte();
        float x = _packet.ReadFloat();
        float y = _packet.ReadFloat();
        float z = _packet.ReadFloat();
        float qx = _packet.ReadFloat();
        float qy = _packet.ReadFloat();
        float qz = _packet.ReadFloat();
        float qw = _packet.ReadFloat();
        Vector3 loc = new Vector3(x,y,z);
        Quaternion rot = new  Quaternion(qx,qy,qz,qw);
        PlayLogic.current.playerComps[locId].set(loc,rot);
    }
    public static void PlayerGameDisconnect(Packet _packet)
    {
        int localId = (int) _packet.ReadByte();
        Destroy(GameLogic.current.players[localId].gameObject);
    }
}