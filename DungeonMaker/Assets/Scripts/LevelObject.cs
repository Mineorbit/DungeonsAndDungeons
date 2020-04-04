using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject
{
	public Object prefab;
	public enum Orientation {North,East,South,West};
	public Orientation orientation;
    	public Vector3 location;
	//Wichtig prefab name ist Zahl des types
	public GameManager.Selectable type;
	public GameObject instance;
	//True if valid
	public virtual bool checkPosition(Vector3 target, Level data)
	{
		return !data.contains(target);
	}
	public virtual void place(Level data)
	{

	}
	public virtual void doAction()
	{
		
	}
	public static LevelObject fromLevelObjectData(LevelObjectData d)
	{
		LevelObject l = new LevelObject();
		l.orientation = d.orientation;
		Vector3 locationT  = new Vector3(d.location[0],d.location[1],d.location[2]);
		l.location = locationT;
		l.type = d.type;
		return l;
	}

	public virtual LevelObjectData toLevelObjectData()
	{
		LevelObjectData d =  new LevelObjectData();
		d.location = new float[] {location.x,location.y,location.z};
		d.orientation =  orientation;
		d.type = type;
		return d;
	}
}
