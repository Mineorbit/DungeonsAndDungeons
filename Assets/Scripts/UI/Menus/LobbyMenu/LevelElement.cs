using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using com.mineorbit.dungeonsanddungeonscommon;

public class LevelElement : MonoBehaviour
{
    enum Task { Open, Close };
    Queue<Task> tasks;
    bool finished;

    UIAnimation openingAnimation;
    float bot = 86;
    float right = 159;
    public RectMask2D mask;
    public Button openButton;
    bool open = false;


    Vector2 lastPosition;
    Vector2 lastObjectPosition;
    Vector3 targetPosition;

    public TextMeshProUGUI nameTextField;
    public TextMeshProUGUI infoTextField;

    LevelList list;

    public LevelMetaData d;

    GameObject[] colorBars;

    void Awake()
    {
        colorBars = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            colorBars[i] = transform.Find("ColorSelect").Find(i.ToString()).gameObject;
        }
    }
    void Start()
    {
        finished = true;
        targetPosition = transform.position;
       openButton.onClick.AddListener(Click);


        UpdateScreen();
        tasks = new Queue<Task>();
        list = transform.parent.GetComponent<LevelList>();
        
    }

    public void UpdateScreen()
    {
        right = (float)Screen.width * 0.5f * 0.65f;
        bot = (float)Screen.height * 0.5f * 0.75f;

        Set(0);
    }
    public void Open()
    {
        list.SetSelectedLevel(d);
        tasks.Enqueue(Task.Open);
    }
    public void Close()
    {
        if(list.GetSelectedLevel() == d)
        {
            list.SetSelectedLevel(null);
        }
        tasks.Enqueue(Task.Close);
    }
    public void UpdateElement(LevelMetaData data)
    {
        d = data;
        nameTextField.SetText(data.name);
        UpdateScreen();
        UpdateAvailColors();
    }
    void UpdateAvailColors()
    {
        colorBars[0].SetActive(d.availBlue);
        colorBars[1].SetActive(d.availYellow);
        colorBars[2].SetActive(d.availRed);
        colorBars[3].SetActive(d.availGreen);
    }
    IEnumerator OpenAnim()
    {

        finished = false;
        open = true;
        for (float ft = 0f; ft <= 1; ft += 4*Time.deltaTime)
        {
            Set(ft);
            yield return new WaitForSeconds(.01f);
        }

        Set(1);
        finished = true;
    }
    IEnumerator CloseAnim()
    {
        finished = false;
        for (float ft = 1f; ft >= 0; ft -= 4*Time.deltaTime)
        {
            Set(ft);
            yield return new WaitForSeconds(.01f);
        }
        Set(0);

        open = false;
        finished = true;
    }
    void UpdateAnimation()
    {
        if(tasks.Count>0)
        {
            if (finished)
            {
                Task t = tasks.Dequeue();
                if(t == Task.Open)
                {
                    if (!open)
                    {

                        list.CloseOthersFrom(this);
                        StartCoroutine("OpenAnim");
                    }
                }
                else
                {
                    if (open)
                    StartCoroutine("CloseAnim");
                }
            }
        }
    }
    bool target = false;
    public void Click() 
    {
        if(!target)
        {
            if(list.listType == LevelList.ListType.Net)
            {
               // NetworkManager.instance.SendLevelSelection(d);
            }
            Open();
        }else
        {
            Close();
        }
        target = !target;
    }
    void Set(float t)
    {
        if (t<0 || t> 1) return;
        float r =  (1-t)* right;
        float b = (1-t) * bot;
        mask.padding = new Vector4(0,b,r,0);
        Canvas.ForceUpdateCanvases();
    }
    void Update()
    {
        UpdatePosition();
        UpdateAnimation();
    }
    void UpdatePosition()
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
