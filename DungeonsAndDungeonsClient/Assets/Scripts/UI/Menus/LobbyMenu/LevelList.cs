using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelList : MonoBehaviour
{
    Vector2 lastPosition;
    Vector2 lastObjectPosition;
    Vector3 targetPosition;

    GameObject[] elements;
    LevelElement[] levelElements; 
    public InstantionTarget levelElementPrefab;

    void Start()
    {
        transform.position = new Vector3(Screen.width/1.3f,-Screen.height/5,0);
        targetPosition = transform.position;
        UpdateList(LevelManager.levelManager.availableLocalLevels);
    }

    public void UpdateList(LevelData.LevelMetaData[] localLevels)
    {
        Debug.Log("Updating List");
        if(elements!=null)
        foreach(GameObject g in elements)
        {
            Destroy(g);
        }
        elements = new GameObject[localLevels.Length];
        levelElements = new LevelElement[localLevels.Length];
        for (int i = 0;i < elements.Length;i++)
        {

            elements[i] = levelElementPrefab.Create(GetPositionOfElement(((float) i)), transform);
            levelElements[i] = elements[i].GetComponent<LevelElement>();
            levelElements[i].UpdateElement(localLevels[i]);
        }

    }

    Vector2 GetPositionOfElement(float f)
    {
        float cnt = 8f;
        float r = Mathf.Floor(f / cnt);
        Vector2 offset = 150* (r + 1) * new Vector2(Mathf.Sin(2 * f * Mathf.PI / ( cnt)),Mathf.Cos(2*f*Mathf.PI/(cnt)));
        return offset;
    }
    void Update()
    {
        updatePosition();
    }
    void updatePosition()
    {
        Vector2 offset;
        if (Input.GetMouseButtonDown(0))
        {
            lastPosition = Input.mousePosition;
            lastObjectPosition = transform.position;
        }
        if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            offset = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - lastPosition;
            targetPosition = new Vector3(lastObjectPosition.x + offset.x, lastObjectPosition.y + offset.y, 0);
        }
        transform.position = (transform.position + targetPosition) / 2;
    }
}
