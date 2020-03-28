using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	//GameLogic
	public enum SceneIndex { Starting = 0, MainMenu = 1, Edit = 2, Play = 3, Option = 4 }
	SceneIndex currentScene = SceneIndex.Starting;
	public enum State { start, mainmenu, loadplay, loadedit, play, edit, option }
	public enum PlayState { test, play }
	public State currentState = State.start;

	public bool loading = true;

	//EditState

	LevelEditor editor;
	public Cursor cursor;
	public PreviewData cursorData;
	public enum Selectable { cursor = 0, wall = 1, floor = 2 }

	public Selectable selectedPrefab = Selectable.cursor;
	Object[] examplePrefabs;
	public GameObject[] instantiatedPlacePrefabs;
	Mesh[] cursorMeshes;
	public bool clearLevel = true;

	public int previewTextureResolution = 512;
	public RenderTexture[] renderTextures;
	public bool renderTexturesSet = false;
	Vector3 prefabPlace = new Vector3 (0, -40, 0);

	Vector3 prefabOffset = new Vector3 (0, -20, 0);

	//PlaySate/Networkdata
	PlayState currentPlayState;
	// Start is called before the first frame update
	void Start () {
		editor = this.GetComponent<LevelEditor> ();
		startMainMenuMode ();
	}

	public void startMainMenuMode () {
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
		postSceneLoadAction ();

		loading = false;
	}

	void clear () {
		if (currentState == State.edit) {
			stopEditMode ();
		}
		if (currentState == State.play) {
			stopPlayMode ();
		}
		if (currentScene != SceneIndex.Starting) {
			SceneManager.UnloadSceneAsync ((int) currentScene);
		}
	}

	public void startPlayMode () {
		clear ();
		currentState = State.play;
		StartCoroutine (load (SceneIndex.Play));

	}

	public void startEditMode () {
		clear ();
		currentState = State.edit;
		StartCoroutine (load (SceneIndex.Edit));
		GenerateExamplePrefabs ();
		setupMeshes ();
	}

	void stopEditMode () {
		foreach (Transform prefab in transform) {
			Destroy (prefab.gameObject);
		}
		if (clearLevel) {
			editor.clear ();
		}
	}
	void stopPlayMode () {
		if (clearLevel) {
			editor.clear ();
		}

	}
	void postSceneLoadAction () {
		if (currentState == State.edit) {
			GameObject cur = GameObject.Find ("Cursor");
			cursor = cur.GetComponent<Cursor> ();

			editor.startEdit ();
			//Load  LevelData if existing level is edited
		}
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
		renderTextures = new RenderTexture[examplePrefabs.Length];
		Vector3 location = new Vector3 (0, 0, 0);
		instantiatedPlacePrefabs = new GameObject[examplePrefabs.Length];
		for (int i = 0; i < examplePrefabs.Length; i++) {
			location = prefabPlace + (i * prefabOffset);
			instantiatedPlacePrefabs[i] = (GameObject) Instantiate (examplePrefabs[i], location, Quaternion.identity, this.transform);
			instantiatedPlacePrefabs[i].GetComponent<PreviewData> ().apply ();
			GameObject cameraRig = (GameObject) Instantiate (rotCamera, location, Quaternion.identity, this.transform);

			Camera cam = cameraRig.transform.Find ("Camera").gameObject.GetComponent<Camera> ();
			RenderTexture rt = new RenderTexture (previewTextureResolution, previewTextureResolution, 16, RenderTextureFormat.ARGB32);
			rt.Create ();
			renderTextures[i] = rt;
			cam.targetTexture = renderTextures[i];
			renderTexturesSet = true;
		}
	}

	void Update () {
		updateEditMode ();
	}

	void updateEditMode () {

	}

	public void updateCursor () {
		bool valid = editor.checkPositionValid ();

		if ((int) selectedPrefab >= 1) cursor.setCursor (valid, cursorMeshes[((int) selectedPrefab) - 1], cursorData);
	}
}