using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelObjectType { cursor = 0,  enemy = 1 , floor = 2, spawn = 3, wall = 4, goal = 5};
public class LevelObject:MonoBehaviour
{
	public Object prefab;
	public enum Orientation {North,East,South,West};
	public Orientation orientation;
    	public Vector3 location;
	//Wichtig prefab name ist Zahl des types
	public LevelObjectType type;
	public GameObject instance;

	public void Start()
	{
		instance = this.gameObject;
	}

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
	public LevelObjectData toLevelObjectData()
	{
		LevelObjectData d = new LevelObjectData();
		d.location = new float[]{location.x,location.y,location.z};
		d.orientation = orientation;
		d.type = type;
		return d;
	}
	
}
