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
	public string LevelName;
    void Start()
    {
    GameObject gManager = GameObject.Find("GameManager");
	gameManager = gManager.GetComponent<GameManager>();
	levelHook = GameObject.Find("Level");
    }
   

	public void startEdit()
	{
	prepareMapPrefabs();
	setupCurrentLevel();
	}


	void setupCurrentLevel()
	{
	if(levelHook.GetComponent<Level>()==null)
	{
	create();
	}else open();

		foreach(KeyValuePair<string,LevelObject> p in currentLevel.levelItems)
			{
				Debug.Log("P:"+p.Key);
			}

	}

	public void open()
	{
		currentLevel = levelHook.GetComponent<Level>();
	}

	public void create()
	{
		currentLevel = levelHook.AddComponent(typeof(Level)) as Level;
	}

    public void prepareMapPrefabs()	
	{
	mapPrefabs = Resources.LoadAll("Map",typeof(GameObject));
	}
	
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
		{
			
		}
    }

	public Object getCurrent()
	{
	return mapPrefabs[(int)gameManager.selectedPrefab];
	}

	public bool checkPositionValid(Vector3 loc,LevelObject o, Level l)
	{
	if(currentLevel==null) return false;
	//Additional Rules for each object type specifically
	return o.checkPosition(loc,l);
	}

	public void save()
	{
		LevelSaver ls = new LevelSaver();
		LevelData data = new LevelData();
		data.fromLevel(currentLevel);
		ls.save("map/"+LevelName,data);
	}
	
	public void add()
	{

	LevelObject newObject;


	newObject = LevelObject.setClass(gameManager.selectedPrefab);
	newObject.type = gameManager.selectedPrefab;
	newObject.prefab = getCurrent();

	add(newObject);
	}
	


	public void add(LevelObject e)
	{
		Vector3 target = gameManager.cursor.transform.position;
		if(checkPositionValid(target,e,currentLevel))
		{
		currentLevel.addObject(target,e);
		}
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
		Destroy(levelHook.GetComponent<Level>());
		currentLevel = null;
		gameManager.currentLevel = null;
		LevelName = null;
		gameManager.newLevelName = null;
		gameManager.TargetLevelName = null;
		Debug.Log("Cleared everything");
	}
	
}
