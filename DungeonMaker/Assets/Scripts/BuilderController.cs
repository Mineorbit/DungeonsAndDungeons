using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuilderController : MonoBehaviour
{
    public Transform cursor;
    ViewController viewController;
	LevelEditor levelEditor;
    // Start is called before the first frame update
    void Start()
    {
	cursor = GameObject.Find("Cursor").transform;
        viewController = GetComponent<ViewController>();
	GameObject gManager = GameObject.Find("GameManager");
	levelEditor = gManager.GetComponent<LevelEditor>();
    }

    // Update is called once per frame
    void Update()
    {
        cursor.position = viewController.target;
	handleMouseInput();

    }
	
	void handleMouseInput()
	{
	
		if(Input.GetMouseButtonDown(0))
		{
		double y = Input.mousePosition.y/Screen.height;
		bool noUIcontrolsInUse = EventSystem.current.IsPointerOverGameObject();
		if(!noUIcontrolsInUse)
		{
		levelEditor.add();
		}	
		}
	}

}
