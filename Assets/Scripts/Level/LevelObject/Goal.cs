using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : LevelObject
{
    bool[] Inside;
    public override void OnCreate()
    {
        base.OnCreate();
        if (Level.currentLevel.goal != null)
        {
            Destroy(this.gameObject);
        }else
        { 
        Level.currentLevel.goal = this;
            Inside = new bool[4];
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(GameManager.GetState() == GameManager.State.Test)
        {
            Player p = other.gameObject.GetComponent<Player>();
            if(p != null)
            {
                Inside[p.localId] = true;
            CheckWin();
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (GameManager.GetState() == GameManager.State.Test)
        {
            Player p = other.gameObject.GetComponent<Player>();
            if (p != null)
            {
                Inside[p.localId] = false;
            }
        }
    }

    public void CheckWin()
    {
        bool anyone = false;
        bool win = true;
        for(int i = 0;i<4;i++)
        {
            if(PlayerManager.playerManager.players[i] != null)
            {
                anyone = true;
                win = win && Inside[i];
            }
        }
        if (anyone && win) Action();
    }

    public override void Action()
    {
        base.Action();
        Debug.Log("Win in Test");
    }
}
