using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : LevelObject
{

    public bool[] inside;
    public override void OnCreate()
    {
        if (Level.currentLevel.goal != null) Level.currentLevel.Remove(this);

        Level.currentLevel.goal = this;
        inside = new bool[4];
    }

    public void OnTriggerEnter(Collider other)
    {
        Player p = other.gameObject.GetComponent<Player>();
        if(p!=null)
        {
            inside[p.localId] = true;
            GameLogic.CheckForWin(inside);
        }
    }
    public void OnTriggerExit(Collider other)
    {

        Player p = other.gameObject.GetComponent<Player>();
        if (p != null)
        {
            inside[p.localId] = false;
        }
    }

}
