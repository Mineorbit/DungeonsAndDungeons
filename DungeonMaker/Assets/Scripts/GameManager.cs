using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	//GameLogic
	public enum SceneIndex { Starting = 0, MainMenu = 1, Edit = 2, Play = 3, Option = 4}
	SceneIndex currentScene = SceneIndex.Starting;
	public enum State { start, mainmenu, loadplay, loadedit, play, edit, option,test }

	public State currentState = State.start;

	public bool loading = true;

	public string levelToLoad =  "";

	//EditState
	//Describes wether to create new Level in edit mode
	public bool newLevel =  true;
	Vector3 lastPosition;
	public LevelObject dummy;
	LevelEditor editor;
	public Cursor cursor;
	public PreviewData cursorData;
	public Vector3 cursorLocation;
	public enum Selectable { cursor = 0,  enemy = 1 , floor = 2, spawn = 3, wall = 4, goal = 5};

	public Selectable selectedPrefab = Selectable.cursor;
	Object[] examplePrefabs;
	public GameObject[] instantiatedPlacePrefabs;
	Mesh[] cursorMeshes;
	public bool clearLevel = true;
	public bool backToEdit = false;

	public int previewTextureResolution = 512;
	public Dictionary<PreviewData,Texture> renderTextures;
	public bool renderTexturesSet = false;
	Vector3 prefabPlace = new Vector3 (0, -40, 0);

	Vector3 prefabOffset = new Vector3 (0, -20, 0);

	//PlaySate/Networkdata
	int hp;
	int mp;
	public bool roundStarted = false;
	public Level currentLevel;
	GameObject[] players;

	// Start is called before the first frame update
	void Start () {
		clearLevel = true;
		editor = this.GetComponent<LevelEditor> ();
		startMainMenuMode ();
	}
	public LevelEditor getEditor()
	{
		return editor;
	}
	public void startMainMenuMode () {
		clearLevel = true;
		backToEdit = false;
		clear ();
		currentState = State.mainmenu;
		StartCoroutine (load (SceneIndex.MainMenu));
	}

	IEnumerator load (SceneIndex i) {
		loading = true;
		AsyncOperation op = SceneManager.LoadSceneAsync ((int) i, LoadSceneMode.Additive);

		currentScene = i;

		while (!op.isDone) {
			yield return null;
		}
		postSceneLoadAction();

		loading = false;
	}

	void clear () {
		openLoadingScreen();
		if (currentState == State.edit) {
			stopEditMode ();
		}
		if (currentState == State.play) {
			stopPlayMode ();
		}
		if (currentState == State.test) {
			stopTestMode ();
		}
		if (currentScene != SceneIndex.Starting) {
			SceneManager.UnloadSceneAsync ((int) currentScene);
		}
	}

	public void startPlayMode () {
		clear ();
		currentState = State.play;
		loadLevel();
		StartCoroutine (load (SceneIndex.Play));

	}
	void loadLevel(){
		GameObject levelHook = GameObject.Find("Level");
		Level level = (levelHook.GetComponent<Level>()==null)?levelHook.AddComponent<Level>():levelHook.GetComponent<Level>();
		LevelLoader loader = new LevelLoader();
		LevelData lD = loader.load(levelToLoad);
		level = lD.toLevel(level);
		currentLevel = level;
	}

	public void startEditMode () {
		clear ();
		currentState = State.edit;
		StartCoroutine (load (SceneIndex.Edit));
		GenerateExamplePrefabs ();
		setupMeshes ();
	}
	public void startTestMode() {
		clearLevel = false;
		backToEdit = true;
		clear();
		currentState = State.test;
		currentLevel = editor.currentLevel;
		StartCoroutine (load(SceneIndex.Play));


	}

	void stopEditMode () {
		lastPosition = GameObject.Find("Builder").transform.position;
		foreach (Transform prefab in transform) {
			Destroy (prefab.gameObject);
		}
		if (clearLevel) {
			editor.clear();
		}
	}
	void stopTestMode () {
		clearLevel = !backToEdit;
		if (clearLevel) {
			editor.clear ();
		}
	}
	void stopPlayMode () {
		if (clearLevel) {
			Transform level = GameObject.Find("Level").transform;
			foreach(Transform o in level)
			{
				Destroy(o.gameObject);
			}
		}

	}
	void postSceneLoadAction () {
		if (currentState == State.edit) {
			
			startEdit();
		}
		if(currentState == State.play){
			startGame();
		}
		if(currentState == State.test){
			startTest();
		}
	closeLoadingScreen();
	}

	void openLoadingScreen()
	{

	}
	void closeLoadingScreen()
	{

	}

	void startEdit()
	{
			if(!newLevel)
			{
			loadLevel();
			editor.currentLevel = currentLevel;
			}
			GameObject cur = GameObject.Find ("Cursor");
			cursor = cur.GetComponent<Cursor> ();
			editor.startEdit ();

	}
	void startTest()
	{
		Debug.Log("Test");
		//Setup TestPlayer
		players = new GameObject[1];
		InstantiateTestPlayer();
		players[0] = GameObject.Find("Player");
		
			if(currentLevel.spawn==null)
			{
				Debug.Log("Testerfuck");
			players[0].transform.position = lastPosition + new Vector3(0,5,0);
			}else
			{
			spawnPlayers(players);	
			}
	}
	void startGame(){
		players = new GameObject[1];
		InstantiatePlayers();
		players[0] = GameObject.Find("Player");
		spawnPlayers(players);
		roundStarted = true;
	}
	void InstantiatePlayers()
	{

	}
	void InstantiateTestPlayer()
	{

	}
	void updateGame()
	{

	}

	void spawnPlayers(GameObject[] players)
	{
		currentLevel.spawn.doAction(players);
	}

	void setupMeshes () {
		cursorMeshes = new Mesh[examplePrefabs.Length];

		for (int i = 0; i < cursorMeshes.Length; i++) {
			cursorMeshes[i] = instantiatedPlacePrefabs[i].GetComponent<PreviewData> ().previewMesh;
		}
	}

	void GenerateExamplePrefabs () {
		renderTexturesSet = false;
		examplePrefabs = Resources.LoadAll ("CursorMesh", typeof (GameObject));
		Object rotCamera = Resources.Load ("Main/RotatingCamera");
		renderTextures = new Dictionary<PreviewData,Texture>();
		Vector3 location = new Vector3 (0, 0, 0);
		instantiatedPlacePrefabs = new GameObject[examplePrefabs.Length];
		for (int i = 0; i < examplePrefabs.Length; i++) {
			location = prefabPlace + (i * prefabOffset);
			instantiatedPlacePrefabs[i] = (GameObject) Instantiate (examplePrefabs[i], location, Quaternion.identity, this.transform);
			PreviewData pd = instantiatedPlacePrefabs[i].GetComponent<PreviewData> ();
			pd.apply();
			GameObject cameraRig = (GameObject) Instantiate (rotCamera, location, Quaternion.identity, this.transform);

			Camera cam = cameraRig.transform.Find ("Camera").gameObject.GetComponent<Camera> ();
			RenderTexture rt = new RenderTexture (previewTextureResolution, previewTextureResolution, 16, RenderTextureFormat.ARGB32);
			rt.Create ();
		    renderTextures.Add(pd , rt);
			cam.targetTexture = rt;
		}
		renderTexturesSet = true;
	}

	void Update () {
		updateEditMode ();
		if(roundStarted)
		{
			updateGame();
		}
	}

	void updateEditMode () {

	}

	public void updateCursor () {
		
		dummy.type = selectedPrefab;
		bool valid = editor.checkPositionValid (cursorLocation,dummy,editor.currentLevel);
		cursor.setCursor(valid);
		if ((int) selectedPrefab >= 1) cursor.setCursor (valid, cursorMeshes[((int) selectedPrefab) - 1], cursorData);
	}
}