using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    private static LevelData _current;
	LevelObject[] levelObject;
	public static LevelData current
	{
		get
		{
			if(_current == null) _current = new LevelData();
				return _current;
			
		}

	}
}
