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

}
