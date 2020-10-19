using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelList : MonoBehaviour
{
    Vector2 lastPosition;
    Vector2 lastObjectPosition;
    Vector3 targetPosition;

    GameObject[] elements;
    public LevelElement[] levelElements; 
    public InstantionTarget levelElementPrefab;

    LevelData.LevelMetaData selected;

    LevelData.LevelMetaData[] currentList;

    public void SetSelectedLevel(LevelData.LevelMetaData levelMetaData)
    {
        selected = levelMetaData;
    }
    public LevelData.LevelMetaData GetSelectedLevel()
    {
        return selected;
    }

    public void CloseOthersFrom(LevelElement e)
    {
        foreach(LevelElement x in levelElements)
        {
            if(x!=e)
            {
                x.Close();
            }
        }
    }

    void Start()
    {
        UpdateList(LevelManager.levelManager.availableLocalLevels);
    }
    void Update()
    {
        if(LevelManager.levelManager.availableLocalLevels!=currentList)
        {
            UpdateList(LevelManager.levelManager.availableLocalLevels);
        }
    }

    public void UpdateList(LevelData.LevelMetaData[] localLevels)
    {
        if(elements!=null)
        foreach(GameObject g in elements)
        {
            Destroy(g);
        }
        elements = new GameObject[localLevels.Length];
        levelElements = new LevelElement[localLevels.Length];
        for (int i = 0;i < elements.Length;i++)
        {

            elements[i] = levelElementPrefab.Create(GetPositionOfElement(i,localLevels.Length), transform);
            levelElements[i] = elements[i].GetComponent<LevelElement>();
            levelElements[i].UpdateElement(localLevels[i]);
        }
        currentList = localLevels;
    }

    Vector2 GetPositionOfElement(int f,int n)
    {
        Vector2 start = new Vector2(-Screen.width/2,-Screen.height);
        float cnt = 10f;
        float h = Mathf.Floor(Screen.height * 0.375f *  Mathf.Floor((float) f/cnt));
        float w = Mathf.Floor(Screen.width * 0.65f * 5 * ( 1 - ((float) f / cnt - Mathf.Floor((float) f / cnt)))) - 1;
        Vector2 offset = new Vector2(w,h);
        return offset+start;
    }  
   
}
