using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Level : MonoBehaviour{
    public Dictionary<string,LevelObject> levelItems;
    public GameObject levelHook;
    public string name;
    public Spawn spawn = null;
    public Goal  goal = null;
    
    GameManager gameManager;
    bool setup = false;
    Dictionary<GameManager.Selectable,UnityEngine.Object> prefabs;


    public void Awake() {
     setupLevel();
    }

    void setupLevel()
    {
        levelHook = this.gameObject;
        gameManager = GameObject.Find("GameManager").transform.GetComponent<GameManager>();
        levelItems = new Dictionary<string,LevelObject>();
        spawn = null;
    }
    void setupPrefabDictionary()
    {

    }
    public bool Valid()
    {
        return (levelItems.Count>0)&&(spawn!=null)&&(goal!=null);
    }
    //Type and location must be  set
    public void addObject(Vector3 location, LevelObject obj) {
     if(!setup) {setupLevel(); setup = true;}
    
    GameManager.Selectable type =  obj.type;
    //Klasse Konfigurieren
    obj = LevelObject.setClass(type);
    obj.type = type;
    //Dringend optimieren
    obj = setPrefab(obj);


    if(obj.type==GameManager.Selectable.spawn)
    {
    spawn =  (Spawn) obj;
    }
    if(obj.type==GameManager.Selectable.goal)
    {
    goal = (Goal) obj;
    }

    LevelObject entry = instantiateObject(location,obj);
    int x  = (int) location.x;
    int y  = (int) location.y;
    int z  = (int) location.z;
    entry.place(this);
    levelItems.Add(x+"|"+y+"|"+z,entry);
    }
    
    LevelObject setPrefab(LevelObject o)
    {
        o.prefab = Resources.Load("Map/"+(int)o.type);
        return o;
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