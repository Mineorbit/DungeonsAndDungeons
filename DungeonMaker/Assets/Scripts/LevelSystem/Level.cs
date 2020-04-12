using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    public Dictionary<string, LevelObject> levelItems;
    public GameObject levelHook;
    public string name;
    public Spawn spawn = null;
    public Goal goal = null;

    GameManager gameManager;
    bool setup = false;
    Dictionary<int, UnityEngine.Object> prefabs;

    public void Awake () {
        setupLevel ();

    }

    void setupLevel () {
        levelHook = this.gameObject;
        gameManager = GameObject.Find ("GameManager").transform.GetComponent<GameManager> ();
        levelItems = new Dictionary<string, LevelObject> ();
        spawn = null;
        setupPrefabDictionary ();
    }
    public void setupRound()
    {
        foreach (KeyValuePair<string,LevelObject> p in levelItems)
        {
            p.Value.onRoundStart();
        }
    }

    void setupPrefabDictionary () {
        prefabs = new Dictionary<int, UnityEngine.Object> ();
        int length = Enum.GetNames (typeof (GameManager.Selectable)).Length;
        for (int i = 0; i < length; i++) {
            UnityEngine.Object p = Resources.Load ("Map/" + i);
            prefabs.Add (i, p);
        }
    }
    public bool Valid () {
        return (levelItems.Count > 0) && (spawn != null) && (goal != null);
    }

    /*
        Type and location must be  set
        Instantiates GameObject newObject under Level and signs it in.
        Creates LevelObject onto Object newObject
    */
    public void addObject (Vector3 location, LevelObjectData objData) {
        if (!setup) { setupLevel (); setup = true; }
        UnityEngine.Object prefab = prefabs[(int) objData.type];
        GameObject newObject = (GameObject) Instantiate (prefab, levelHook.transform) as GameObject;
        Vector3 loc = new Vector3(objData.location[0],objData.location[1],objData.location[2]);
        newObject.transform.position = loc;
        LevelObject lvlObj = addLevelObject (newObject, objData);
        lvlObj.type = objData.type;
        lvlObj.prefab = prefab;
        lvlObj.location = loc;

        string s = loc.x + "|" + loc.y + "|" + loc.z;
        levelItems.Add (s, lvlObj);
    }
    public LevelObject addLevelObject (GameObject g, LevelObjectData d) {
        if (d.type == GameManager.Selectable.spawn) {
            Spawn lspawn = g.AddComponent<Spawn> ();
            spawn = lspawn;
            return lspawn;
        }
        if (d.type == GameManager.Selectable.goal) {
            Goal lgoal = g.AddComponent<Goal> ();
            goal = lgoal;
            return lgoal;
        }
        LevelObject o = g.AddComponent<LevelObject> ();
        return o;
    }

    public bool contains (Vector3 location) {
        int x = (int) location.x;
        int y = (int) location.y;
        int z = (int) location.z;
        string c = x + "|" + y + "|" + z;
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

    public void remove (Vector3 location) {

    }

}