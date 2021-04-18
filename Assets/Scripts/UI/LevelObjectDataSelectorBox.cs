using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using com.mineorbit.dungeonsanddungeonscommon;

public class LevelObjectDataSelectorBox : MonoBehaviour
{
    public ScrollRect scrollRect;

    public UnityEngine.Object selectorPrefab;
    public LevelObjectData[] dataObjects;

    int numberOfSelectorsVisible = 5;
    LevelObjectDataSelector[] selectors;

    public float t = -1;

    int selected = 0;
    int n;

    enum Direction {Left, Right};
    Queue<Direction> directionQueue;

    Scrollbar marker;
    void Start()
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
        scrollRect.normalizedPosition = new Vector2(0.5f,0.5f);
        Select(1);
    }
    Vector2 pos;
    bool changed = false;
    void Update()
    {
        

        GetInput();
        if (!moving&&changed)
        {
            pos = new Vector2(0.5f, 0.5f);
            SetListFromT();
        }
        if(changed)
        {
            Select(selected);
            changed = false;
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
                {
                    moving = true;
                    StartCoroutine(goRight(0.25f));
                }
            }else
            if (d == Direction.Left)
            {
                if (t > -1)
                {
                    moving = true;
                    StartCoroutine(goLeft(0.25f));
                }
            }
        }
    }
    float eps = 0.000001f;
    IEnumerator goRight(float time)
    {
        float oldT = t;
        for (float x = 0; (time - x) > eps; x += Time.deltaTime)
        {
            t += Time.deltaTime;

            pos = new Vector2(0.5f, 0.5f) + (x/time)* new Vector2(0.5f, 0.5f);
            
            yield return null;
        }
        t = oldT + 1;
        moving = false;
        changed = true;
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
        changed = true;
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
        for(int i = 0;i < n;i++)
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
        Transform hook = transform.Find("Viewport").Find("Content");

        Debug.Log("Anzahl: "+dataObjects.Length);
        selectors = new LevelObjectDataSelector[n];
        
        for (int i = (int) t;i<(int) t+n;i++)
        {
            GameObject selObject = Instantiate(selectorPrefab, hook) as GameObject;
            selectors[i] = selObject.GetComponent<LevelObjectDataSelector>();
            selectors[i].SetData(dataObjects[i]);
        }
    }
}
