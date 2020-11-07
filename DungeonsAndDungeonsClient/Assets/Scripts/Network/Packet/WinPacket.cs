using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        //Hier noch Open von AlertScreen mit Win Message.
        AlertScreen.alert.Open("You win!");
        GameManager.instance.performAction(GameManager.GameAction.EnterMainMenuFromWin);
    }
}
