using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	GameManager gameManager;

	//MainMenuMode

	public GameObject MainMenu;
	public GameObject Menu;

	public GameObject BuilderUI;
	public GameObject BuilderMenu;

	public GameObject PlayerUI;
	public GameObject PlayerMenu;

	CanvasGroup MenuSlider;

	CanvasGroup MainMenuSlider;

	CanvasGroup PlaySlider;

	CanvasGroup EditSlider;

	public float FadeTime;
	public bool menuActive = false;
	enum MenuState { Main, Play, Edit, Option, Credit, Pause, None }
	MenuState currentMenu = MenuState.None;

	//EditMode
	Dictionary<PreviewData, Texture> prefabTextures;

	bool renderTexturesIn = false;
	// Start is called before the first frame update
	void Start () {
		setup ();
	}

	void MenuLogic () {
		if (Input.GetKeyDown (KeyCode.Escape) && !menuActive) {
			OpenMenu ();
		} else
		if (Input.GetKeyDown (KeyCode.Escape) && menuActive) {
			CloseMenu ();
		}
	}

	void setup () {
		GameObject gManager = GameObject.Find ("GameManager");
		if (gManager != null)
			gameManager = gManager.GetComponent<GameManager> ();

		if (gameManager.currentState == GameManager.State.mainmenu) {
			
			MainMenu = this.transform.Find ("MainMenu").gameObject;
			MainMenuSlider = MainMenu.GetComponent<CanvasGroup>();
			setupMainMenu ();
			PlayerMenu = this.transform.Find ("PlayerMenu").gameObject;
			PlaySlider = PlayerMenu.GetComponent<CanvasGroup>();
			BuilderMenu = this.transform.Find ("BuilderMenu").gameObject;
			EditSlider = BuilderMenu.GetComponent<CanvasGroup>();
			setupPlayerMenu();
			setupBuilderMenu();
		}

		if (gameManager.currentState != GameManager.State.mainmenu) {
			Menu = this.transform.Find ("Menu").gameObject;
			MenuSlider = Menu.GetComponent<CanvasGroup> ();
			setupMenu ();
		}

		if (gameManager.currentState == GameManager.State.edit) {
			BuilderUI = this.transform.Find ("BuilderUI").gameObject;
			setupBuilderUI ();
		}
		if (gameManager.currentState == GameManager.State.test) {
			setupTestEndMenu ();
		}

		startMainMenu();
	}

	void setupMainMenu () {
		Button play = MainMenu.transform.Find ("Play").GetComponent<Button> ();
		Button edit = MainMenu.transform.Find ("Edit").GetComponent<Button> ();
		play.onClick.AddListener (startPlayerMenu);
		edit.onClick.AddListener (startBuilderMenu);
	}

	void setupPlayerUI () { }

	void setupBuilderUI () {
		Button test = BuilderUI.transform.Find ("BottomBar").Find ("SubMenu").Find ("Test").GetComponent<Button> ();
		test.onClick.AddListener (backToEdit);
		Button save = BuilderUI.transform.Find ("BottomBar").Find ("SubMenu").Find ("Save").GetComponent<Button> ();
		save.onClick.AddListener (storeTestAction);
	}
	void storeTestAction () {
		gameManager.getEditor ().save ();
	}
	void backToEdit () {
		gameManager.backToEdit = true;
		gameManager.startTestMode ();
	}
	void setupHotbar(Transform bar)
	{
		GameObject o = bar.transform.Find("Back").gameObject;
		Button backButton = o.GetComponent<Button>();
		backButton.onClick.AddListener(startMainMenu);
	}
	void setupBuilderMenu () {
		setupHotbar(BuilderMenu.transform.Find("Hotbar"));
	 }

	void closeCurrentMenu () {
		if (currentMenu == MenuState.Play) {
			closePlayerMenu ();
		} else
		if (currentMenu == MenuState.Edit) {
			closeBuilderMenu ();
		} else
		if (currentMenu == MenuState.Main) {
			closeMainMenu ();
		}
	}

	void setupPlayerMenu () { 
		setupHotbar(PlayerMenu.transform.Find("Hotbar"));
	}

	void startMainMenu () { 
		closeCurrentMenu();
		currentMenu = MenuState.Main;
		StartCoroutine("openMainMenu");
	}

	void startPlayerMenu () { 
		closeCurrentMenu();
		currentMenu = MenuState.Play;
		StartCoroutine("openPlayMenu");
	}
	void startBuilderMenu () { 
		closeCurrentMenu();
		currentMenu = MenuState.Edit;
		StartCoroutine("openEditMenu");
	}

	void closePlayerMenu () {
		StartCoroutine("closePlayMenu");
	}
	void closeBuilderMenu () {
		StartCoroutine("closeEditMenu");
	}
	void closeMainMenu () {
		StartCoroutine("closetheMainMenu");
	}


	IEnumerator closetheMainMenu () {

		for (float ft = 1f; ft >= 0; ft -= 0.1f) {
			MainMenuSlider.alpha = ft;
			yield return null;
		}
		MainMenuSlider.alpha = 0;
		MainMenu.SetActive(false);
		yield return null;
	}

	IEnumerator openMainMenu () {

		MainMenu.SetActive(true);
		for (float ft = 0f; ft <= 1; ft += 0.1f) {
			MainMenuSlider.alpha = ft;
			yield return null;
		}
		MainMenuSlider.alpha = 1;

		yield return null;
	}

	IEnumerator closePlayMenu () {

		for (float ft = 1f; ft >= 0; ft -= 0.1f) {
			PlaySlider.alpha = ft;
			yield return null;
		}
		PlaySlider.alpha = 0;
		PlayerMenu.SetActive(false);
		yield return null;
	}

	IEnumerator openPlayMenu () {

		PlayerMenu.SetActive(true);
		for (float ft = 0f; ft <= 1; ft += 0.1f) {
			PlaySlider.alpha = ft;
			yield return null;
		}
		PlaySlider.alpha = 1;

		yield return null;
	}


	IEnumerator closeEditMenu () {

		for (float ft = 1f; ft >= 0; ft -= 0.1f) {
			EditSlider.alpha = ft;
			yield return null;
		}
		EditSlider.alpha = 0;

		BuilderMenu.SetActive(false);
		yield return null;
	}

	IEnumerator openEditMenu () {

		BuilderMenu.SetActive(true);
		for (float ft = 0f; ft <= 1; ft += 0.1f) {
			EditSlider.alpha = ft;
			yield return null;
		}
		EditSlider.alpha = 1;

		yield return null;
	}


	void setupTestEndMenu () {
		Object prefabElement = Resources.Load ("UI/EndTest");
		GameObject button = (GameObject) Instantiate (prefabElement, new Vector3 (0, 0, 0), Quaternion.identity, this.transform) as GameObject;
		RectTransform rectTransform = button.GetComponent<RectTransform> ();
		rectTransform.offsetMin = new Vector2 (0, 0);
		rectTransform.offsetMax = new Vector2 (0, 0);

		Button testButton = button.GetComponent<Button> ();
		testButton.onClick.AddListener (gameManager.startEditMode);

	}
	void setupMenu () {
		Transform rMenu = Menu.transform.Find ("Panel");
		Button exit = rMenu.Find ("Close").GetComponent<Button> ();
		Button mainMenu = rMenu.Find ("MainMenu").GetComponent<Button> ();
		Button options = rMenu.Find ("Option").GetComponent<Button> ();
		Button desktop = rMenu.Find ("Exit").GetComponent<Button> ();
		exit.onClick.AddListener (CloseMenu);
		mainMenu.onClick.AddListener (gameManager.startMainMenuMode);
		desktop.onClick.AddListener (Application.Quit);
		Menu.SetActive (false);
	}
	// Update is called once per frame
	void Update () {

		if (gameManager.currentState != GameManager.State.mainmenu)
			MenuLogic ();

		if (gameManager.currentState == GameManager.State.edit) {
			if (!renderTexturesIn && gameManager.renderTexturesSet && BuilderUI != null) {
				updateItemList ();
			}
		}

	}

	void updateItemList () {

		renderTexturesIn = true;
		prefabTextures = gameManager.renderTextures;
		setupLevelItemList ();
	}
	void setupLevelItemList () {
		GameObject content = BuilderUI.transform.Find ("BottomBar").Find ("Content").gameObject;
		Object prefabElement = Resources.Load ("UI/PrefabElement");
		foreach (KeyValuePair<PreviewData, Texture> d in prefabTextures) {
			GameObject element = (GameObject) Instantiate (prefabElement, new Vector3 (0, 0, 0), Quaternion.identity, content.transform);
			//Set Texture
			element.GetComponent<RawImage> ().texture = d.Value;

			//Add MouseOverEvent
			EventTrigger.Entry entry = new EventTrigger.Entry ();
			entry.eventID = EventTriggerType.PointerClick;
			entry.callback.AddListener ((eventData) => {
					gameManager.selectedPrefab = d.Key.thisElement;
					gameManager.cursorData = d.Key;
					gameManager.dummy = gameManager.getEditor ().setClass ();
					gameManager.updateCursor ();
				}
				//Change color of Selectionbox
			);
			element.GetComponent<EventTrigger> ().triggers.Add (entry);

		}
	}
	IEnumerator closeMenu () {

		for (float ft = 1f; ft >= 0; ft -= 0.1f) {
			MenuSlider.alpha = ft;
			yield return null;
		}
		MenuSlider.alpha = 0;

		Menu.SetActive(false);
		yield return null;
	}

	IEnumerator openMenu () {
		Menu.SetActive(true);
		for (float ft = 0f; ft <= 1; ft += 0.1f) {
			MenuSlider.alpha = ft;
			yield return null;
		}
		MenuSlider.alpha = 1;

		yield return null;
	}

	public void CloseMenu () {
		menuActive = false;
		StartCoroutine ("closeMenu");
	}
	public void OpenMenu () {
		menuActive = true;
		StartCoroutine ("openMenu");
	}
	public void StopGame () {
		Application.Quit ();
	}

	
}