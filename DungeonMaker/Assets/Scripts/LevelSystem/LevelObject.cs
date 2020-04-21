using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject:MonoBehaviour
{
	public Object prefab;
	public enum Orientation {North,East,South,West};
	public Orientation orientation;
    public Vector3 location;
	//Wichtig prefab name ist Zahl des types
	public GameManager.Selectable type;
	public GameObject instance;
	//every gridPoint, thats considered  part of the mesh
	public Vector3[] inPoints;
	public void Start()
	{
		instance = this.gameObject;
		setInpoints();
	}
	public void setType(GameManager.Selectable targetType)
	{
		type = targetType;
		setInpoints();
	}


	public void setInpoints()
	{
		inPoints = new Vector3[1]{new Vector3(0,0,0)};
	}

	//True if target is  inside of the mesh
	public bool checkPosition(Vector3 target)
	{
		if(instance==null) return false;

		if(location == target) return true;
		foreach(Vector3 offset in inPoints)
		{
			if((location+offset) == target) return true;
		}
		return false;
	}
	public virtual void place(Level data)
	{

	}

	public virtual void setupEdit()
	{

	}
	public virtual void setupTest()
	{

	}
	public virtual void setupPlay()
	{

	}
	public virtual void updateTest()
	{
		
	}
	public virtual void updateEdit()
	{

	}

	public virtual void updatePlay()
	{

	}
	public virtual void doAction()
	{
		
	}
	public virtual void onRoundStart()
	{

	}
	public void Update()
	{
		if(GameManager.current.currentState  == GameManager.State.play)
    	{
			updatePlay();
    	}
		if(GameManager.current.currentState  == GameManager.State.edit)
		{
			updateEdit();
		}
		if(GameManager.current.currentState  == GameManager.State.test)
		{
			updateTest();
		}
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
