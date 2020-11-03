using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : LevelObject
{
    public override void OnCreate()
    {
        int i = ObjectDataID - 4;

               if (Level.currentLevel.spawn[i] != null) Level.currentLevel.Remove(this);

                Level.currentLevel.spawn[i] = this;
        
    }
}
