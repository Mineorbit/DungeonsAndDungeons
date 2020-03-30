using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
	GameManager 		gameManager;
	public Object[] 	mapPrefabs;
	//LevelData 
	public GameObject 	levelHook;
	public Level		currentLevel;
	
    void Start()
    {
    GameObject gManager = GameObject.Find("GameManager");
	gameManager = gManager.GetComponent<GameManager>();
    }
    public void startEdit()
	{
	prepareMapPrefabs();
	levelHook = GameObject.Find("Level");
	create();
	}

    public void prepareMapPrefabs()	
	{
	mapPrefabs = Resources.LoadAll("Map",typeof(GameObject));
	}
	
    // Update is called once per frame
    void Update()
    {
        
    }

	public Object getCurrent()
	{
	return mapPrefabs[(int)gameManager.selectedPrefab];
	}

	public bool checkPositionValid(Vector3 loc)
	{
		if(currentLevel==null) return false;
	//Additional Rules
	return !currentLevel.contains(loc);
	}

    public void load()
	{

	}
	public void save()
	{

	}
	
	public void add()
	{
	LevelObject newObject =	new LevelObject();
	newObject.type = gameManager.selectedPrefab;
	newObject.prefab = getCurrent();
	add(newObject);
	}

	public void add(LevelObject e)
	{
		Vector3 target = gameManager.cursor.transform.position;
		if(checkPositionValid(target))
		{
		currentLevel.addObject(target,e);
		}
	}

    public void open()
	{
		currentLevel = levelHook.AddComponent(typeof(Level)) as Level;
	}

	public void create()
	{
		currentLevel = levelHook.AddComponent(typeof(Level)) as Level;
	}

	public void remove(LevelObject e)
	{


	}

	public void remove(Vector3 location)
	{

	}
	public void clear()
	{
		foreach(Transform t in levelHook.transform)
		{
		Destroy(t.gameObject);
		}
	}
	
}
