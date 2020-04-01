using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : LevelObject
{
    public override bool checkPosition(Vector3 target, Level data)
	{
        if(data.spawn!=null) return false;

		return !data.contains(target);
	}
    public override void place(Level data)
    {
        data.spawn = this;
    }

}
