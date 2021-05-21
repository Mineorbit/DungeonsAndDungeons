using System;
using System.Collections;
using System.Collections.Generic;
using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class LevelObjectDataSelectorBox : MonoBehaviour
{
    public ScrollRect scrollRect;

    public Object selectorPrefab;
    public LevelObjectData[] dataObjects;

    public float t = -1;
    private bool changed;
    private Queue<Direction> directionQueue;
    private readonly float eps = 0.000001f;

    private Scrollbar marker;

    private bool moving;
    private int n;

    private readonly int numberOfSelectorsVisible = 5;
    private Vector2 pos;

    private int selected;
    private LevelObjectDataSelector[] selectors;

    private void Start()
    {
        directionQueue = new Queue<Direction>();

        selectorPrefab = Resources.Load("pref/level/UI/LevelObjectSelector");
        scrollRect = GetComponent<ScrollRect>();

        dataObjects = LevelObjectData.GetAllBuildable();
        marker = transform.GetComponentInChildren<Scrollbar>();


        n = Math.Min(numberOfSelectorsVisible + 2, dataObjects.Length);

        SetupList();
        t = -1;
        SetListFromT();
        scrollRect.normalizedPosition = new Vector2(0.5f, 0.5f);
        Select(1);
    }

    private void Update()
    {
        GetInput();
        if (!moving && changed)
        {
            pos = new Vector2(0.5f, 0.5f);
            SetListFromT();
        }

        if (changed)
        {
            Select(selected);
            changed = false;
        }

        scrollRect.normalizedPosition = pos;
        StartMoving();
    }

    private void StartMoving()
    {
        if (!moving && directionQueue.Count > 0)
        {
            var d = directionQueue.Dequeue();
            if (d == Direction.Right)
            {
                if (t < dataObjects.Length - numberOfSelectorsVisible - 1)
                {
                    moving = true;
                    StartCoroutine(goRight(0.25f));
                }
            }
            else if (d == Direction.Left)
            {
                if (t > -1)
                {
                    moving = true;
                    StartCoroutine(goLeft(0.25f));
                }
            }
        }
    }

    private IEnumerator goRight(float time)
    {
        var oldT = t;
        for (float x = 0; time - x > eps; x += Time.deltaTime)
        {
            t += Time.deltaTime;

            pos = new Vector2(0.5f, 0.5f) + x / time * new Vector2(0.5f, 0.5f);

            yield return null;
        }

        t = oldT + 1;
        moving = false;
        changed = true;
    }

    private IEnumerator goLeft(float time)
    {
        moving = true;
        var oldT = t;
        for (float x = 0; time - x > eps; x += Time.deltaTime)
        {
            t += Time.deltaTime;

            pos = new Vector2(0.5f, 0.5f) - x / time * new Vector2(0.5f, 0.5f);

            yield return null;
        }

        t = oldT - 1;
        moving = false;
        changed = true;
    }


    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            directionQueue.Enqueue(Direction.Left);
        else if (Input.GetKeyDown(KeyCode.E)) directionQueue.Enqueue(Direction.Right);
        for (var i = 1; i <= numberOfSelectorsVisible; i++)
            if (Input.GetKeyDown(i.ToString()))
                Select(i);
    }


    private void SetListFromT()
    {
        for (var i = 0; i < n; i++)
        {
            var p = (int) t + i;
            if (0 <= p && p < dataObjects.Length)
                selectors[i].SetData(dataObjects[(int) t + i]);
        }

        changed = true;
    }

    private void Select(int i)
    {
        if (i >= 1 && i <= numberOfSelectorsVisible)
        {
            selected = i;
            marker.value = (float) (i - 1) / (numberOfSelectorsVisible - 1);
            try
            {
                selectors[i].Select();
            }
            catch (Exception e)
            {
            }
        }
    }


    private void SetupList()
    {
        var hook = transform.Find("Viewport").Find("Content");

        Debug.Log("Anzahl: " + dataObjects.Length);
        selectors = new LevelObjectDataSelector[n];

        for (var i = (int) t; i < (int) t + n; i++)
        {
            var selObject = Instantiate(selectorPrefab, hook) as GameObject;
            selectors[i] = selObject.GetComponent<LevelObjectDataSelector>();
            selectors[i].SetData(dataObjects[i]);
        }
    }

    private enum Direction
    {
        Left,
        Right
    }
}