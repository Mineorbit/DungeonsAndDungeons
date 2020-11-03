using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : LevelObject
{
    public override void OnCreate()
    {
        if (Level.currentLevel.goal != null) Level.currentLevel.Remove(this);

        Level.currentLevel.goal = this;
    }
}
