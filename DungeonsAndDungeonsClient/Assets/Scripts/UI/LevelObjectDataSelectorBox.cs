using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class LevelObjectDataSelectorBox : MonoBehaviour
{
    public ScrollRect scrollRect;

    public UnityEngine.Object selectorPrefab;
    public LevelObjectData[] dataObjects;

    int numberOfSelectorsVisible = 5;
    LevelObjectDataSelector[] selectors;

    public float t = -1;

    int selected = 0;

    enum Direction {Left, Right};
    Queue<Direction> directionQueue;

    Scrollbar marker;
    void Start()
    {
        directionQueue = new Queue<Direction>();

        selectorPrefab = Resources.Load("pref/level/UI/LevelObjectSelector");
        scrollRect = GetComponent<ScrollRect>();

        marker = transform.GetComponentInChildren<Scrollbar>();

        SetupList();
        t = -1;
        SetListFromT();
        scrollRect.normalizedPosition = new Vector2(0.5f,0.5f);
        Select(1);
    }
    Vector2 pos;
    bool changed = false;
    void Update()
    {
        

        GetInput();
        if (!moving)
        {
            pos = new Vector2(0.5f, 0.5f);
            SetListFromT();
            if(changed)
            {
                Select(selected);
                changed = false;
            }
        }
        scrollRect.normalizedPosition = pos;
        StartMoving();
    }

    bool moving = false;

    void StartMoving()
    {
        if(!moving&&directionQueue.Count>0)
        {
            Direction d = directionQueue.Dequeue();
            if(d == Direction.Right)
            {
                if (t < dataObjects.Length-numberOfSelectorsVisible-1)
                StartCoroutine(goRight(0.25f));
            }else
            if (d == Direction.Left)
            {
                if (t > -1)
                StartCoroutine(goLeft(0.25f));
            }
        }
    }
    float eps = 0.000001f;
    IEnumerator goRight(float time)
    {
        moving = true;
        float oldT = t;
        for (float x = 0; (time - x) > eps; x += Time.deltaTime)
        {
            t += Time.deltaTime;

            pos = new Vector2(0.5f, 0.5f) + (x/time)* new Vector2(0.5f, 0.5f);
            
            yield return null;
        }
        t = oldT + 1;
        moving = false;
    }
    IEnumerator goLeft(float time)
    {
        moving = true;
        float oldT = t;
        for (float x = 0; (time-x) > eps; x += Time.deltaTime)
        {
            t += Time.deltaTime;

            pos = new Vector2(0.5f, 0.5f) - (x / time) * new Vector2(0.5f, 0.5f);
            
            yield return null;
        }

        t = oldT - 1;
        moving = false;
    }



    void GetInput()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            directionQueue.Enqueue(Direction.Left);
        }else
        if (Input.GetKeyDown(KeyCode.E))
        {
            directionQueue.Enqueue(Direction.Right );
        }
        for(int i = 1;i <= numberOfSelectorsVisible;i++)
        {
            if(Input.GetKeyDown(i.ToString()))
            {
                Select(i);
            }
        }
    }


    void SetListFromT()
    {
        for(int i = 0;i < numberOfSelectorsVisible+2;i++)
        {
            int p = (int)t + i;
            if (0 <= p && p < dataObjects.Length)
            selectors[i].SetData(dataObjects[(int)t+i]);
        }
        changed = true;
    }

    void Select(int i)
    {
        if(i>=1&&i<=numberOfSelectorsVisible)
        {
            selected = i;
            marker.value = (float)(i - 1) / (numberOfSelectorsVisible-1);
            try
            {
                selectors[i].Select();
            }catch (Exception e)
            {

            }
        }
    }





    void SetupList()
    {
        int n = numberOfSelectorsVisible + 2;
        Transform hook = transform.Find("Viewport").Find("Content");
        dataObjects = Resources.LoadAll<LevelObjectData>("pref/level/data");
        selectors = new LevelObjectDataSelector[n];
        
        for (int i = (int) t;i<(int) t+n;i++)
        {
            GameObject selObject = Instantiate(selectorPrefab, hook) as GameObject;
            selectors[i] = selObject.GetComponent<LevelObjectDataSelector>();
            selectors[i].SetData(dataObjects[i]);
        }
    }
}
