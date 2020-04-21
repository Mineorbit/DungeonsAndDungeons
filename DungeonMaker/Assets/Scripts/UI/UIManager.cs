using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;
public class UIManager : MonoBehaviour {
	GameManager gameManager;
	public static UIManager current;

	//MainMenuMode

	public GameObject MainMenu;
	public GameObject Menu;

	public GameObject BuilderUI;
	public GameObject BuilderMenu;

	public GameObject PlayerUI;
	public GameObject PlayerMenu;
	public GameObject WinMenu;


	public GameObject OptionsUI;

	CanvasGroup MenuSlider;

	CanvasGroup MainMenuSlider;

	CanvasGroup PlaySlider;
	CanvasGroup WinSlider;

	CanvasGroup EditSlider;

	CanvasGroup	bottomBuildSlider;

	public Bar hpBar;
	public Bar mpBar;

	Button delete;

	public float FadeTime;
	public bool menuActive = false;
	bool		optionOpen = false;
	enum MenuState { Main, Play, Edit, Option, Credit, Pause, None }
	MenuState currentMenu = MenuState.None;

	//EditMode
	Dictionary<PreviewData, Texture> prefabTextures;

	bool renderTexturesIn = false;
	string[] dataLevelName;
	// Start is called before the first frame update
	public void Start () {
		current = this;
		setup ();
	}

	public void openWin()
	{
		StartCoroutine("openWinMenu");
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


		if(GameManager.current.currentState == GameManager.State.play||GameManager.current.currentState==GameManager.State.test)
		{
			WinMenu = this.transform.Find("Win").gameObject;
			WinSlider = WinMenu.GetComponent<CanvasGroup>();
			setupWinMenu();
		}

		if (gameManager.currentState == GameManager.State.mainmenu) {
			
			MainMenu = this.transform.Find ("MainMenu").gameObject;
			MainMenuSlider = MainMenu.GetComponent<CanvasGroup>();
			setupMainMenu ();
			PlayerMenu = this.transform.Find ("PlayerMenu").gameObject;
			PlaySlider = PlayerMenu.GetComponent<CanvasGroup>();
			BuilderMenu = this.transform.Find ("BuilderMenu").gameObject;
			EditSlider = BuilderMenu.GetComponent<CanvasGroup>();
			OptionsUI = this.transform.Find("Options").gameObject;
			setupOptions();
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
		if (gameManager.currentState == GameManager.State.play||gameManager.currentState == GameManager.State.test) {
			PlayerUI = this.transform.Find ("PlayerUI").gameObject;
			setupPlayerUI ();
		}
		if (gameManager.currentState == GameManager.State.test) {
			setupTestEndMenu ();
		}

		startMainMenu();
	}

	void setupOptions()
	{
		OptionsUI.GetComponent<Options>().Setup();
	}

	void setupMainMenu () {
		Button play = MainMenu.transform.Find ("Play").GetComponent<Button> ();
		Button edit = MainMenu.transform.Find ("Edit").GetComponent<Button> ();
		Button option = MainMenu.transform.Find("Option").GetComponent<Button>();
		play.onClick.AddListener (startPlayerMenu);
		//play.onClick.AddListener(gameManager.startPlayMode);
		edit.onClick.AddListener (startBuilderMenu);
		option.onClick.AddListener(startOption);
	}

	//Open Menu
	public void startOption()
	{
		if(!optionOpen)
		{
		optionOpen = true;
		StartCoroutine("openOptions");
		}
	}
	public void closeOption()
	{
		StartCoroutine("closeOptions");
		optionOpen = false;
	}


	void setupPlayerUI () { 
		hpBar = PlayerUI.transform.Find("HP").GetComponent<Bar>();
		mpBar = PlayerUI.transform.Find("MP").GetComponent<Bar>();

	}

	void setupBuilderUI () {
		Button test = BuilderUI.transform.Find ("BottomBar").Find ("SubMenu").Find ("Test").GetComponent<Button> ();
		test.onClick.AddListener (backToEdit);
		Button save = BuilderUI.transform.Find ("BottomBar").Find ("SubMenu").Find ("Save").GetComponent<Button> ();
		save.onClick.AddListener (storeAction);
		bottomBuildSlider = BuilderUI.transform.Find("BottomBar").GetComponent<CanvasGroup>();
		delete = BuilderUI.transform.Find("Delete").GetComponent<Button>();
		delete.onClick.AddListener(enterDeleteMode);
	}
	void enterDeleteMode()
	{
		delete.onClick.RemoveAllListeners();
		delete.GetComponentInChildren<TMP_Text>().text = "Exit Remove";
		delete.onClick.AddListener(exitDeleteMode);

		fadeOutBottomBar();
		BuilderController.instance.startRemove();
	}
	void exitDeleteMode()
	{

		delete.onClick.RemoveAllListeners();
		delete.GetComponentInChildren<TMP_Text>().text = "Remove Objects";
		delete.onClick.AddListener(enterDeleteMode);
		fadeInBottomBar();
		BuilderController.instance.startAdd();

	}
	void fadeOutBottomBar()
	{
		StartCoroutine("closeBottomBar");
	}
	void fadeInBottomBar()
	{
		StartCoroutine("openBottomBar");
	}

	IEnumerator closeBottomBar () {
		for (float ft = 1f; ft >= 0; ft -= 0.1f) {
			bottomBuildSlider.alpha = ft;
			yield return null;
		}
		bottomBuildSlider.alpha = 0;
		bottomBuildSlider.gameObject.SetActive(false);
		yield return null;
	}

	IEnumerator openBottomBar () {

		bottomBuildSlider.gameObject.SetActive(true);
		for (float ft = 0f; ft <= 1; ft += 0.1f) {
			bottomBuildSlider.alpha = ft;
			yield return null;
		}
		bottomBuildSlider.alpha = 1;

		yield return null;
	}

	void storeAction () {
		if(gameManager.currentLevel.Valid())
		{
		gameManager.getEditor ().save ();
		}else
		{
			Debug.Log("level hat Mängel");
		}
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
		getLocalLevels();
		changeEditItemList();
		setupBuilderMenuButton();
	 }
	void setupBuilderMenuButton()
	{
		Button newLevel = BuilderMenu.transform.Find("NewLevel").GetComponent<Button>();
		newLevel.onClick.AddListener(createNewLevel);
	}
	

	 void changeEditItemList()
	 {
		Transform contentHandle =   BuilderMenu.transform.Find("LevelList").Find("Viewport").Find("Content");
		clearChildren(contentHandle);
		LevelData[] data = gameManager.localLevels;

		Object levelItemprefab = Resources.Load("UI/LevelItem");
		dataLevelName = new string[data.Length];
		int i = 0;
		foreach(LevelData d in data)
		{
			int j = i;
			GameObject item = Instantiate(levelItemprefab,contentHandle) as GameObject;
			item.transform.Find("Name").GetComponent<TMP_Text>().text = d.name;
			dataLevelName[i] = d.name;
			item.transform.Find("Select").GetComponent<Button>().onClick.AddListener(()=>{loadExistingLevel(j);});
			i++;
		}
	 }

	 void loadExistingLevel(int i)
	 {
		gameManager.newLevel = false;
		gameManager.TargetLevelName = dataLevelName[i];
		gameManager.startEditMode();
	 }

	void createNewLevel()
	{
		Debug.Log("New Level");
		gameManager.newLevel  = true;
		gameManager.newLevelName = BuilderMenu.transform.Find("NewLevelName").Find("Text Area").Find("Text").GetComponent<TMP_Text>().text;
		gameManager.startEditMode();
	}
	void clearChildren(Transform t)
	{
		foreach(Transform data in t)
		{
			Destroy(data);
		}
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

	void getLocalLevels(){
		DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath+"/map");
		LevelLoader loader = new LevelLoader();
		
		FileInfo[] files = dir.GetFiles("*.lev");
		
		gameManager.localLevels = new LevelData[files.Length];
		int i = 0;
		foreach (FileInfo file in files)
 		{
			Debug.Log(file);
			LevelData data = loader.load("/map/"+file.Name);
			gameManager.localLevels[i] = data;
			i++;
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

	IEnumerator closeOptions () {

		for (float ft = 0.85f; ft >= 0; ft -= 0.1f) {
			OptionsUI.transform.localScale = new Vector3(ft,ft,1);
			yield return null;
		}
		OptionsUI.transform.localScale = new Vector3(0,0,1);
		OptionsUI.SetActive(false);
		yield return null;
	}

	IEnumerator openOptions () {

		OptionsUI.SetActive(true);
		for (float ft = 0f; ft <= 0.85f; ft += 0.1f) {
			OptionsUI.transform.localScale = new Vector3(ft,ft,1);
			yield return null;
		}
		OptionsUI.transform.localScale = new Vector3(0.85f,0.85f,1);

		yield return null;
	}


	IEnumerator closeWinMenu () {
		for (float ft = 1f; ft >= 0; ft -= 0.1f) {
			WinSlider.alpha = ft;
			yield return null;
		}
		WinSlider.alpha = 0;
		WinMenu.SetActive(false);
		yield return null;
	}

	IEnumerator openWinMenu () {

		WinMenu.SetActive(true);
		for (float ft = 0f; ft <= 1; ft += 0.1f) {
			WinSlider.alpha = ft;
			yield return null;
		}
		WinSlider.alpha = 1;

		yield return null;
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
	void setupWinMenu() {
		Button mainMenu = WinMenu.transform.Find("MainMenu").GetComponent<Button>();
		Button lobby = WinMenu.transform.Find("Lobby").GetComponent<Button>();
		lobby.onClick.AddListener(null);
		mainMenu.onClick.AddListener(GameManager.current.startMainMenuMode);
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
					gameManager.dummy.setType(gameManager.selectedPrefab);
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
		if(GameManager.current.CanOpenMenu)
		{
		menuActive = true;
		StartCoroutine ("openMenu");
		}
	}
	public void StopGame () {
		Application.Quit ();
	}

	
}