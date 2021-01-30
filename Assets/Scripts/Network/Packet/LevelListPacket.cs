using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class LevelListPacket : Packet
{
    public LevelListPacket()
    {
        packetId = 10;
        types = new Type[8];
        types[0] = typeof(long);
        types[1] = typeof(string);
        types[2] = typeof(string);
        types[3] = typeof(string);
        types[4] = typeof(bool);
        types[5] = typeof(bool);
        types[6] = typeof(bool);
        types[7] = typeof(bool);
        content = new object[8];
    }
    public LevelListPacket(LevelData.LevelMetaData levelMetaData)
    {
        packetId = 10;
        types = new Type[8];
        types[0] = typeof(long);
        types[1] = typeof(string);
        types[2] = typeof(string);
        types[3] = typeof(string);
        types[4] = typeof(bool);
        types[5] = typeof(bool);
        types[6] = typeof(bool);
        types[7] = typeof(bool);
        content = new object[8];
        content[0] = levelMetaData.ulid;
        content[1] = levelMetaData.name;
        content[2] = levelMetaData.description;
        content[3] = levelMetaData.creationDate;
        content[4] = levelMetaData.availBlue;
        content[5] = levelMetaData.availYellow;
        content[6] = levelMetaData.availRed;
        content[7] = levelMetaData.availGreen;

    }
    public override void OnReceive()
    {
        LevelData.LevelMetaData levelData = new LevelData.LevelMetaData("");
        levelData.ulid = (long)content[0];
        levelData.name = (string)content[1];
        levelData.description = (string)content[2];
        levelData.creationDate = (string)content[3];
        levelData.availBlue = (bool)content[4];
        levelData.availYellow = (bool)content[5];
        levelData.availRed = (bool)content[6];
        levelData.availGreen = (bool)content[7];
        LevelManager.AddOnlineLevel(levelData);
        Debug.Log("Added Level to List"+levelData.ulid+" "+levelData.name);
        LevelList.UpdateDisplay();
    }
}
