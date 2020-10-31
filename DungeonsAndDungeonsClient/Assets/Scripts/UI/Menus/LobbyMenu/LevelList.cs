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

    //Lol
    public static HashSet<LevelList> levelLists;


    public enum ListType { Net, Local };
    public ListType listType;
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
    public static void UpdateDisplay()
    {
        foreach(LevelList l in levelLists)
        l.UpdateList(l.currentList);
    }

    void Awake()
    {

        if (levelLists == null)
        {
            levelLists = new HashSet<LevelList>();
        }
    }

    void Start()
    {
        levelLists.Add(this);

        RefreshList();
    }
    void RefreshList()
    {
        switch(listType)
        {
            case ListType.Net:
                if (LevelManager.levelManager.availableNetworkLevels != currentList)
                {
                    Debug.Log("List changed");
                    UpdateList(LevelManager.levelManager.availableNetworkLevels);
                }
                break;
            case ListType.Local:
                if (LevelManager.levelManager.availableLocalLevels != currentList)
                {
                    Debug.Log("List changed");
                    UpdateList(LevelManager.levelManager.availableLocalLevels);
                }
                break;
        }
    }
    void Update()
    {
        RefreshList();
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

            elements[i] = levelElementPrefab.Create(GetPositionOfElement(i,elements.Length), transform);
            levelElements[i] = elements[i].GetComponent<LevelElement>();
            levelElements[i].UpdateElement(localLevels[i]);
        }
        currentList = localLevels;
    }

    int GetClosestDiv(int N)
    {
        float f = Mathf.Ceil(Mathf.Sqrt((float) N));
        int i = 0;
        while(N-i > 0)
        {
            if(N % ((int)f  + i) == 0 )
            {
                return (int) f + i;
            }else
            if (N % ((int)f - i) == 0)
            {
                    return (int)f - i;
            }
            i++;
        }
        return (int) Mathf.Ceil(Mathf.Sqrt((float)N));
    }
    Vector2 GetPositionOfElement(int f,int n)
    {
        int cnt = GetClosestDiv(n);
        float boxHeight = Screen.height * 0.75f * 0.5f;
        float boxWidth = Screen.width * 0.65f * 0.5f;

        int tableWidth = cnt;
        int tableHeight = (int)Mathf.Floor((float)n/ (float) cnt);

        float widthScale = (float)tableWidth / 2 - 1;
        float heightScale = (float)tableHeight / 2 - 1; 
        int height = (int) Mathf.Floor(f/((float) cnt));
        int width =   f % cnt;
        Vector2 placeOffset = new Vector2(-width*boxWidth,height*boxHeight);
        Vector2 stockOffset = new Vector2(widthScale*boxWidth,-heightScale*boxHeight);
        Vector2 start = new Vector2(Screen.width/2, -Screen.height/2);
        return start+placeOffset+stockOffset;
    }  
   
}
