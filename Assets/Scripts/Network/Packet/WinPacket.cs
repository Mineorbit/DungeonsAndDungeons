using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Reflection;

public class WinPacket : Packet
{
    public WinPacket()
    {
        packetId = 9;
        types = new Type[0];
        content = new object[0];
    }
    public override void OnReceive()
    {
        //Hier noch Open von AlertScreen mit Win Message
        GameManager.instance.performAction(GameManager.GameAction.BackToLobbyAfterWin);
        AlertScreen.alert.Open("You win!");
    }
}
