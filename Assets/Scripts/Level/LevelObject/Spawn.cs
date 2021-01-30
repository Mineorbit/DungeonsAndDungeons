using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : LevelObject
{
    public override void OnCreate()
    {
        int i = ObjectDataID - 4;

        if (Level.currentLevel.spawn[i] != null) { Level.currentLevel.Remove(this); }
        else
        { 

        Level.currentLevel.spawn[i] = this;
        switch(i)
        {
            case 0:
                Level.currentLevel.levelMetaData.availBlue = true;
                break;
            case 1:
                Level.currentLevel.levelMetaData.availYellow = true;
                break;
            case 2:
                Level.currentLevel.levelMetaData.availRed = true;
                break;
            case 3:
                Level.currentLevel.levelMetaData.availGreen = true;
                break;
        }
        }

    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        int i = ObjectDataID - 4;
        switch (i)
        {
            case 0:
                Level.currentLevel.levelMetaData.availBlue = false;
                break;
            case 1:
                Level.currentLevel.levelMetaData.availYellow = false;
                break;
            case 2:
                Level.currentLevel.levelMetaData.availRed = false;
                break;
            case 3:
                Level.currentLevel.levelMetaData.availGreen = false;
                break;
        }
    }
}
