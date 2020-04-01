using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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

}
