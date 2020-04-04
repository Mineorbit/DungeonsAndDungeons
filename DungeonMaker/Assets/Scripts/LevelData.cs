using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData {
	private static LevelData _current;
	LevelObject[] levelObjects;
	int spawnIndex;
	int goalIndex;

	public LevelData fromLevel(Level l)
	{
		foreach(KeyValuePair<string,LevelObject> levelObject in l.levelItems)
		{
			
		}
	}
	public Level toLevel(Level currentLevel)
	{
		foreach(LevelObject levelObject in levelObjects)
		{
			currentLevel.addObject(levelObject.location,levelObject);
		}
		currentLevel.spawn = (Spawn) levelObjects[spawnIndex];
		currentLevel.goal = (Goal) levelObjects[goalIndex];
		return currentLevel;
	}

}