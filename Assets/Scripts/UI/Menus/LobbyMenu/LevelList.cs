﻿using System;
using System.Collections.Generic;
using com.mineorbit.dungeonsanddungeonscommon;
using NetLevel;
using UnityEngine;
using UnityEngine.Events;

public class LevelList : MonoBehaviour
{
    public enum ListType
    {
        Net,
        Local
    }

    //Lol
    public static HashSet<LevelList> levelLists;

    public LevelElement[] levelElements;

    public InstantionTarget levelElementPrefab;
    public ListType listType;


    private GameObject[] elements;
    private Vector2 lastObjectPosition;
    private Vector2 lastPosition;

    private NetLevel.LevelMetaData selected;
    private Vector3 targetPosition;

    public UnityEvent<LevelMetaData> levelSelectedEvent = new UnityEvent<LevelMetaData>();
    
    public CanvasGroup canvasGroup;
    
    private void Awake()
    {
        if (levelLists == null) levelLists = new HashSet<LevelList>();

        canvasGroup = GetComponent<CanvasGroup>();

    }

    private void Start()
    {
        levelLists.Add(this);

       // RefreshList();
    }

    private void OnDestroy()
    {
        levelLists.Remove(this);
    }

    public void OnEnable()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnDisable()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void Update()
    {
       // if (GameManager.GetState() == GameManager.MainMenu)
       //     RefreshList();
    }

    public void SetSelectedLevel(LevelMetaData levelMetaData, bool fireEvent = true)
    {
        selected = levelMetaData;
        
        if(fireEvent)
        levelSelectedEvent.Invoke(selected);
    }

    public LevelMetaData GetSelectedLevel()
    {
        return selected;
    }


    public void CloseOthersFrom(LevelElement e)
    {
        foreach (var x in levelElements)
            if (x != e)
                x.Close();
    }

    public static void UpdateDisplay()
    {
        if (levelLists != null)
            foreach (var l in levelLists)
            {
                Debug.Log("Updating list "+l);
                l.UpdateList(l.listType==ListType.Local?LevelDataManager.instance.localLevels:LevelDataManager.instance.networkLevels);
            }
    }


    public void UpdateList(LevelMetaData[] localLevels)
    {
        if (localLevels == null)
        {
            localLevels = new LevelMetaData[0];
            Debug.Log("Local Level List null updating Level List");
        }
        if (elements != null)
            foreach (var g in elements)
                Destroy(g);
        elements = new GameObject[localLevels.Length];
        levelElements = new LevelElement[localLevels.Length];
        for (var i = 0; i < elements.Length; i++)
        {
            elements[i] = levelElementPrefab.Create(GetPositionOfElement(i, elements.Length), transform);
            levelElements[i] = elements[i].GetComponent<LevelElement>();
            levelElements[i].UpdateElement(localLevels[i]);
        }
    }

    private int GetClosestDiv(int N)
    {
        var f = Mathf.Ceil(Mathf.Sqrt(N));
        var i = 0;
        while (N - i > 0)
        {
            if (N % ((int) f + i) == 0)
                return (int) f + i;
            if (N % ((int) f - i) == 0) return (int) f - i;
            i++;
        }

        return (int) Mathf.Ceil(Mathf.Sqrt(N));
    }

    private Vector2 GetPositionOfElement(int f, int n)
    {
        var cnt = GetClosestDiv(n);
        var boxHeight = Screen.height * 0.75f * 0.5f;
        var boxWidth = Screen.width * 0.65f * 0.5f;

        var tableWidth = cnt;
        var tableHeight = (int) Mathf.Floor(n / (float) cnt);

        var widthScale = (float) tableWidth / 2 - 1;
        var heightScale = (float) tableHeight / 2 - 1;
        var height = (int) Mathf.Floor(f / (float) cnt);
        var width = f % cnt;
        var placeOffset = new Vector2(-width * boxWidth, height * boxHeight);
        var stockOffset = new Vector2(widthScale * boxWidth, -heightScale * boxHeight);
        var start = new Vector2(Screen.width / 2, -Screen.height / 2);
        return start + placeOffset + stockOffset;
    }
}