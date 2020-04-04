using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Level : MonoBehaviour{
    public Dictionary<string,LevelObject> levelItems;
    public GameObject levelHook;
    public Spawn spawn;
    public Goal  goal;
    
    GameManager gameManager;
    public void Start() {
        levelHook = this.gameObject;
        gameManager = GameObject.Find("GameManager").transform.GetComponent<GameManager>();
        levelItems = new Dictionary<string,LevelObject>();
        spawn = null;
    }
    public void addObject(Vector3 location, LevelObject obj) {
    LevelObject entry = instantiateObject(location,obj);
    int x  = (int) location.x;
    int y  = (int) location.y;
    int z  = (int) location.z;
    entry.place(this);
    levelItems.Add(x+"|"+y+"|"+z,entry);
    }

    public LevelObject instantiateObject(Vector3 target, LevelObject e)
    {
        LevelObject result =  null;
        
		e.orientation = LevelObject.Orientation.North;
		e.location = target;
		UnityEngine.Object prefab = e.prefab;
		if(e.type==null)
		{
		e.type = gameManager.selectedPrefab;
		}
		GameObject newObject = Instantiate(prefab,levelHook.transform) as GameObject;

		e.instance = newObject;
		newObject.transform.position = e.location;
        result = e;
		
        return result;
    }


    public bool contains(Vector3 location) {  
    int x  = (int) location.x;
    int y  = (int) location.y;
    int z  = (int) location.z;
    string c = x+"|"+y+"|"+z;
        LevelObject temp;
        if(levelItems.TryGetValue(c, out temp))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void remove(Vector3 location) {
    int x  = (int) location.x;
    int y  = (int) location.y;
    int z  = (int) location.z;
    string c = x+"|"+y+"|"+z;
    LevelObject r = levelItems[c];
    Destroy(r.instance);
    levelItems[c] = null;
    }

  
}