using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : LevelObject
{
   
    public override void place(Level data)
    {
        data.spawn = this;
    }
    public void doAction(GameObject[] players)
    {
        Debug.Log("Testers");
        for(int i = 0;i<players.Length;i++)
		{
			players[i].transform.position = location;
		}
    }
}
