using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData {
	public LevelObjectData[] levelObjects;
	public string name;
	public int spawnIndex;
	public int goalIndex;
	public void fromLevel(Level l)
	{
		name = l.name;
		levelObjects = new LevelObjectData[l.levelItems.Count];
		int i = 0;
		foreach(KeyValuePair<string,LevelObject> levelObject in l.levelItems)
		{
			LevelObjectData d = levelObject.Value.toLevelObjectData();
			levelObjects[i] =  d;
			if(d.type == GameManager.Selectable.spawn) spawnIndex = i;
			if(d.type == GameManager.Selectable.goal) goalIndex = i;
			
			i++;
		}
	}
	public Level toLevel(Level currentLevel)
	{
		currentLevel.name =  name;
		LevelObject[] store = new LevelObject[levelObjects.Length];
		int i = 0;
		foreach(LevelObjectData levelObjectData in levelObjects)
		{
			LevelObject levelObject = LevelObject.fromLevelObjectData(levelObjectData);

			store[i] = levelObject;
			Debug.Log("Adding: "+levelObject.location+" "+levelObject);
			currentLevel.addObject(levelObject.location,levelObject);
			Debug.Log(currentLevel.name);
			i++;
		}
			currentLevel.spawn = (Spawn) store[spawnIndex];

			currentLevel.goal = (Goal) store[goalIndex];
		
		return currentLevel;
	}

}