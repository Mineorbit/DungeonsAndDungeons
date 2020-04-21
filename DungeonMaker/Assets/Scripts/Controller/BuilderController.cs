using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuilderController : MonoBehaviour {
	public static BuilderController instance;

	public Transform cursor;
	ViewController viewController;
	LevelEditor levelEditor;


	public enum BuildMode {Add,Remove,Set};
	public static BuildMode currentBuildMode;
	// Start is called before the first frame update
	void Start () {
		instance = this;
		cursor = GameObject.Find ("Cursor").transform;
		viewController = GetComponent<ViewController> ();
		GameObject gManager = GameObject.Find ("GameManager");
		levelEditor = gManager.GetComponent<LevelEditor> ();
		gManager.GetComponent<GameManager>().cursor = cursor.GetComponent<Cursor>();
		startAdd();
	}

	// Update is called once per frame
	void Update () {
		cursor.position = viewController.target;
		handleMouseInput ();

	}
	public void startAdd()
	{
		currentBuildMode = BuildMode.Add;
	}
	public void startRemove()
	{
		currentBuildMode = BuildMode.Remove;

	}

	void handleMouseInput () {
		if(currentBuildMode == BuildMode.Add)
		{
			if (Input.GetMouseButtonDown (0)) {
			double y = Input.mousePosition.y / Screen.height;
			bool noUIcontrolsInUse = EventSystem.current.IsPointerOverGameObject ();
			if (!noUIcontrolsInUse) {
				levelEditor.add ();
			}
		}
		}else if(currentBuildMode == BuildMode.Remove)
		{

			if (Input.GetMouseButtonDown (0)) {
			double y = Input.mousePosition.y / Screen.height;
			bool noUIcontrolsInUse = EventSystem.current.IsPointerOverGameObject ();
			if (!noUIcontrolsInUse) {
				levelEditor.remove();
			}
			}
		}
	}

}