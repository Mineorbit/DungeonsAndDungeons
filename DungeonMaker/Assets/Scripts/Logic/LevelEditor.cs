using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour {
	public static LevelEditor current;
	GameManager gameManager;
	public Object[] mapPrefabs;
	//LevelData 
	public GameObject levelHook;
	public Level currentLevel;
	public string LevelName;
	void Start () {
		current = this;

		gameManager = GameManager.current;
		levelHook = GameObject.Find ("Level");

	}

	public void startEdit () {

		gameManager.dummy = gameManager.cursor.gameObject.AddComponent<LevelObject>();
		prepareMapPrefabs ();
		setupCurrentLevel ();
	}

	void setupCurrentLevel () {
		if (levelHook.GetComponent<Level> () == null) {
			create ();
		} else open ();
	}

	public void open () {
		currentLevel = levelHook.GetComponent<Level> ();
	}

	public void create () {
		currentLevel = levelHook.AddComponent (typeof (Level)) as Level;
	}

	public void prepareMapPrefabs () {
		mapPrefabs = Resources.LoadAll ("Map", typeof (GameObject));
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.F)) {

		}
	}

	public Object getCurrent () {
		return mapPrefabs[(int) gameManager.selectedPrefab];
	}

	public bool checkPositionValid (Vector3 loc, LevelObject o, Level l) {
		if (currentLevel == null) return false;
		return l.Placeable (o,loc);
	}

	public void save () {
		LevelSaver ls = new LevelSaver ();
		LevelData data = new LevelData ();
		data.fromLevel (currentLevel);
		ls.save ("map/" + LevelName, data);
	}
	public void add()
	{
		LevelObjectData d = new LevelObjectData();
		d.type = gameManager.selectedPrefab;
		Vector3 loc = gameManager.cursorLocation;
		d.location = new float[] {loc.x,loc.y,loc.z};
		add(d);
	}
	public void remove()
	{
		Vector3 loc = gameManager.cursorLocation-new Vector3(0,5,0);
		remove(loc);
	}
	public void remove(Vector3 location)
	{
		currentLevel.removeObject(location);
	}
	public void add(LevelObjectData d)
	{
		Vector3 loc = new Vector3(d.location[0],d.location[1],d.location[2]);
		currentLevel.addObject(loc,d);
	}
	
	public void clear () {
		Destroy(gameManager.dummy);
		foreach (Transform t in levelHook.transform) {
			Destroy (t.gameObject);
		}
		Destroy (levelHook.GetComponent<Level> ());
		currentLevel = null;
		gameManager.currentLevel = null;
		LevelName = null;
		gameManager.newLevelName = null;
		gameManager.TargetLevel = null;
	}

}