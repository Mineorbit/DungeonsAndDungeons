using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelObjectDataSelectorBox : MonoBehaviour
{
    public ScrollRect scrollRect;

    public UnityEngine.Object selectorPrefab;
    public LevelObjectData[] dataObjects;

    int numberOfSelectorsVisible = 5;
    LevelObjectDataSelector[] selectors;

    public float t = 0;

    enum Direction {Left, Right};
    Queue<Direction> directionQueue;
    void Start()
    {
        directionQueue = new Queue<Direction>();

        selectorPrefab = Resources.Load("pref/level/UI/LevelObjectSelector");
        scrollRect = GetComponent<ScrollRect>();

        SetupList();
        scrollRect.normalizedPosition = new Vector2(0.5f,0.5f);

    }
    Vector2 pos;
    void Update()
    {
        if (!moving)
        {
        pos = new Vector2(0.5f, 0.5f);
        SetListFromT();
        }

        GetInput();
        StartMoving();

        scrollRect.normalizedPosition = pos;
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
                if(t>0)
                StartCoroutine(goLeft(0.25f));
            }
        }
    }
    float eps = 0.01f;
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

        pos = new Vector2(0.5f, 0.5f);
        SetListFromT();
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
    }

    void SetListFromT()
    {
        for(int i = 0;i < numberOfSelectorsVisible+2;i++)
        {
            if((int)t + i < dataObjects.Length)
            selectors[i].SetData(dataObjects[(int)t+i]);
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
