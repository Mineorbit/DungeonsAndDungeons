using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData {
	public LevelObjectData[] levelObjects;
	public string name;

	public void fromLevel(Level l)
	{
		name = l.name;
		levelObjects = new LevelObjectData[l.levelItems.Count];
		int i = 0;
		foreach(KeyValuePair<string,LevelObject> levelObject in l.levelItems)
		{
			LevelObjectData d = levelObject.Value.toLevelObjectData();
			levelObjects[i] =  d;
			i++;
		}
	}
	public Level toLevel(Level currentLevel)
	{
		currentLevel.name =  name;
		foreach(LevelObjectData levelObjectData in levelObjects)
		{
			Vector3 loc = new Vector3(levelObjectData.location[0],levelObjectData.location[1],levelObjectData.location[2]);
			currentLevel.addObject(loc,levelObjectData);
			
		}
		
		return currentLevel;
	}

}