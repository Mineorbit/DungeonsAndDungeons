using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
	GameManager gameManager;
	public GameObject MainMenu;
    	public GameObject Menu;
	public GameObject BuilderUI;
	public GameObject BuilderMenu;

	
	public GameObject PlayerUI;
	public GameObject PlayerMenu;
		
	CanvasGroup MainMenuSlider;
	public float FadeTime;
	public bool menuActive = false;

	//MainMenuMode
	
	//EditMode
	Dictionary<PreviewData,Texture> prefabTextures;



	bool renderTexturesIn = false;
    // Start is called before the first frame update
    void Start()
    {
	setup();
    }
	void MenuLogic()
	{
	if(Input.GetKeyDown(KeyCode.Escape)&&!menuActive)
	{
	OpenMenu();
	}else        
	if(Input.GetKeyDown(KeyCode.Escape)&&menuActive)
	{
	CloseMenu();
	} 
	}
	
	void setup()
	{
	
	GameObject gManager = GameObject.Find("GameManager");
	if(gManager!=null)	
	gameManager = gManager.GetComponent<GameManager>();

	if(gameManager.currentState==GameManager.State.mainmenu)
	{
	MainMenu = this.transform.Find("MainMenu").gameObject;
	setupMainMenu();	
	}
	
	if(gameManager.currentState!=GameManager.State.mainmenu)
	{	
	Menu = this.transform.Find("Menu").gameObject;
	MainMenuSlider = Menu.GetComponent<CanvasGroup>();
	setupMenu();
	}

	if(gameManager.currentState==GameManager.State.edit)
	{
	BuilderUI =  this.transform.Find("BuilderUI").gameObject;
	setupBuilderUI();
	}
	if(gameManager.currentState==GameManager.State.test)
	{
	setupTestEndMenu();

	}


	}
	
	void setupMainMenu()
	{
	Button play = MainMenu.transform.Find("Play").GetComponent<Button>();
	Button edit = MainMenu.transform.Find("Edit").GetComponent<Button>();
	play.onClick.AddListener(gameManager.startPlayMode);
	edit.onClick.AddListener(gameManager.startEditMode);
	
	}
	
	void setupBuilderUI()
	{
		Button test =  BuilderUI.transform.Find("BottomBar").Find("SubMenu").Find("Test").GetComponent<Button>();
		test.onClick.AddListener(backToEdit);
	}
	void backToEdit()
	{
	gameManager.backToEdit = true;
	gameManager.startTestMode();
	}
	void setupBuilderMenu()
	{
	}
	void setupPlayerUI()
	{
	}
	void setupPlayerMenu()
	{
	}
	void setupTestEndMenu()
	{
	Object prefabElement = Resources.Load("UI/EndTest");
	GameObject button	= (GameObject) Instantiate(prefabElement,new Vector3(0,0,0),Quaternion.identity,this.transform) as GameObject;
	RectTransform rectTransform = button.GetComponent<RectTransform>();
	rectTransform.offsetMin = new Vector2(0, 0);
    rectTransform.offsetMax = new Vector2(0, 0);

	Button testButton = button.GetComponent<Button>();
	testButton.onClick.AddListener(gameManager.startEditMode);


	}
	void setupMenu()
	{
	Transform rMenu = Menu.transform.Find("Panel");
	Button exit = rMenu.Find("Close").GetComponent<Button>();
	Button mainMenu = rMenu.Find("MainMenu").GetComponent<Button>();
	Button options = rMenu.Find("Option").GetComponent<Button>();
	Button desktop = rMenu.Find("Exit").GetComponent<Button>();
	exit.onClick.AddListener(CloseMenu);
	mainMenu.onClick.AddListener(gameManager.startMainMenuMode);
	desktop.onClick.AddListener(Application.Quit);
	Menu.SetActive(false);
	}
    // Update is called once per frame
    void Update()
    {
	       
	if(gameManager.currentState!=GameManager.State.mainmenu)
	MenuLogic();

	if(gameManager.currentState==GameManager.State.edit)
	{
		if(!renderTexturesIn&&gameManager.renderTexturesSet&&BuilderUI!=null)
		{
		updateItemList();
		}
	}

    }

	void updateItemList()
	{
	
		renderTexturesIn = true;
		prefabTextures = gameManager.renderTextures;
		setupLevelItemList();
	}
	void setupLevelItemList()
	{
		GameObject content = BuilderUI.transform.Find("BottomBar").Find("Content").gameObject;
		Object prefabElement = Resources.Load("UI/PrefabElement");
		foreach(KeyValuePair<PreviewData,Texture> d in prefabTextures)
		{
		GameObject element = (GameObject)Instantiate(prefabElement,new 	Vector3(0,0,0),Quaternion.identity,content.transform);
		//Set Texture
		element.GetComponent<RawImage>().texture = d.Value;

		//Add MouseOverEvent
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener( (eventData) => {gameManager.selectedPrefab = d.Key.thisElement;
		gameManager.cursorData = d.Key;
		 gameManager.dummy =  gameManager.getEditor().setClass();
		 gameManager.updateCursor();} 
		 //Change color of Selectionbox
		 );
		element.GetComponent<EventTrigger>().triggers.Add(entry);

		}
	}
	IEnumerator closeMenu()
	{

	for (float ft = 1f; ft >= 0; ft -= 0.1f) 
    	{
	MainMenuSlider.alpha = ft;
        yield return null;
	}
	MainMenuSlider.alpha = 0;

        yield return null;
	}
	
	IEnumerator openMenu()
	{

	for (float ft = 0f; ft <= 1; ft += 0.1f) 
    	{
	MainMenuSlider.alpha = ft;
        yield return null;
	}
	MainMenuSlider.alpha = 1;

        yield return null;
	}

	public void CloseMenu()
	{
	menuActive = false;
	Menu.SetActive(false);
	StartCoroutine("closeMenu");
	}
	public void OpenMenu()
	{
	menuActive = true;
	Menu.SetActive(true);
	StartCoroutine("openMenu");
	}
	public void StopGame()
	{
	Application.Quit();
	}

	void openEditMenu()
	{
	BuilderUI.SetActive(true);
	}

}
