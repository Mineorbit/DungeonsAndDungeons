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
   
}
