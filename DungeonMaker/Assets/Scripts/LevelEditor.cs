using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
	GameManager gameManager;
	public Object[] mapPrefabs;
	//LevelData 
	public GameObject levelHook;
	public Level	currentLevel;
	
    void Start()
    {
        GameObject gManager = GameObject.Find("GameManager");
	gameManager = gManager.GetComponent<GameManager>();
    }
    public void startEdit()
	{
	prepareMapPrefabs();
	levelHook = GameObject.Find("Level");
	currentLevel = levelHook.GetComponent<Level>();
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

	public bool checkPositionValid()
	{
	
	return true;
	}

    	public void open()
	{

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
		if(checkPositionValid())
		{
		e.orientation = LevelObject.Orientation.North;
		e.location = gameManager.cursor.transform.position;
		Object prefab = e.prefab;
		if(e.type==null)
		{
		e.type = gameManager.selectedPrefab;
		}
		GameObject newObject = Instantiate(prefab,levelHook.transform) as GameObject;
		newObject.transform.position = e.location;
		//Auch in  Level noch eintragen
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
		Debug.Log("Hallo!");
		foreach(Transform t in levelHook.transform)
		{
		Destroy(t.gameObject);
		}
	}
	
}
