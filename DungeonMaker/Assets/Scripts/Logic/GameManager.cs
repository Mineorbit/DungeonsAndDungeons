using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager current;
	//GameState
	public enum SceneIndex { Starting = 0, MainMenu = 1, Edit = 2, Play = 3, Option = 4}
	public enum State { start, mainmenu, loadplay, loadedit, play, edit, option,test }

	SceneIndex currentScene = SceneIndex.Starting;
	public State currentState = State.start;

	public bool loading = true;

	public string levelToLoad =  "";

	GameObject loadingScreen;
	UnityEngine.Object loadingScreenPrefab;
	//EditState

	int playerId;

	public LevelData[] localLevels;
	public bool newLevel =  true;
	public string newLevelName = "test";
	public string TargetLevelName = "";
	//Describes wether to create new Level in edit mode
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
	public bool Host = false;
	public Level currentLevel;


    public PlayerData[] playerData;

	void Awake()
	{
		setupGame();
		current = this;

        playerData = new PlayerData[4];
	}


	void setupGame()
	{
        Directory.CreateDirectory(Application.persistentDataPath+"/map");
		loadingScreenPrefab = Resources.Load("UI/LoadingScreen");
	}


	// Start is called before the first frame update
	void Start () {
		dummy = new LevelObject();
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
		//if(TestLogic.current!=null) TestLogic.current.startUnpause();
		openLoadingScreen();


		if(GameLogic.current!=null) Destroy(GameLogic.current);
		if (currentState == State.edit) {
			stopEditMode ();
		}
		if (currentState == State.play) {
			stopPlayMode ();
		}
		if (currentState == State.test) {
			stopTestMode ();
		}
		if(currentState == State.mainmenu)
		{
			stopMainMenuMode();
		}
		if(currentState == State.start)
		{
			closeLoadingScreen();
		}

		if (currentScene != SceneIndex.Starting) {
			SceneManager.UnloadSceneAsync((int)currentScene);
		}
	}
	
// Important Mode starters


	public void startPlayMode () {

		clear ();
		currentState = State.play;
		levelToLoad = "/map/Test​.lev";
		loadLevel();

		clearForGame();
		StartCoroutine (load (SceneIndex.Play));
		
	}

	//Replace all LevelObjects with NetworkLevelObjects
	void clearForGame()
	{

	}
	void loadLevel(){

		GameObject levelHook = GameObject.Find("Level");
		editor.levelHook = levelHook;
		Level level = (levelHook.GetComponent<Level>()==null)?levelHook.AddComponent<Level>():levelHook.GetComponent<Level>();
		level.levelHook = levelHook;
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
		this.gameObject.AddComponent<TestLogic>();
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
		GameLogic.current.stopRound();
	}
	void stopPlayMode () {
		if (clearLevel) {
			Transform level = GameObject.Find("Level").transform;
			foreach(Transform o in level)
			{
				Destroy(o.gameObject);
			}
		}
		GameLogic.current.stopRound();
	}
	void stopMainMenuMode() {
		clearLevel = true;

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

	}
	public void startGame()
	{
		this.gameObject.AddComponent<PlayLogic>();
		ClientSend.PlayerReady(Client.instance.localId);
	}

	public void openLoadingScreen()
	{
		loadingScreen = (GameObject) Instantiate(loadingScreenPrefab) as GameObject;
	}
	public void closeLoadingScreen()
	{
		Destroy(loadingScreen);
	}

	public void startEdit()
	{
			GameObject cur = GameObject.Find ("Cursor");
			cursor = cur.GetComponent<Cursor> ();
			if(!backToEdit)
			{
			if(!newLevel)
			{
			levelToLoad = "/map/"+TargetLevelName+".lev";
			loadLevel();
			editor.currentLevel = currentLevel;
			
			currentLevel.name = TargetLevelName;
			editor.LevelName = TargetLevelName;
			}else
			{
			Level l = editor.levelHook.AddComponent<Level>();
			l.name = newLevelName;
			currentLevel = l;
			currentLevel.name = newLevelName;
			editor.LevelName = newLevelName;
			editor.currentLevel = currentLevel;
			}
			}
			editor.startEdit();
		closeLoadingScreen();
	}

	void startTest()
	{
		GameLogic.current.startRound();
		closeLoadingScreen();
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
		
	}

	void updateEditMode () {

	}

	public void updateCursor () {
		dummy.type = selectedPrefab;
		bool valid = editor.checkPositionValid (cursorLocation,dummy,editor.currentLevel);
		cursor.setCursor(valid);
		if ((int) selectedPrefab >= 1) cursor.setCursor (valid, cursorData.previewMesh, cursorData);
	}
}