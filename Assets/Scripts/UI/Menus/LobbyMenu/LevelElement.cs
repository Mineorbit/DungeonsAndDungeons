using System.Collections;
using System.Collections.Generic;
using com.mineorbit.dungeonsanddungeonscommon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelElement : MonoBehaviour
{
    public RectMask2D mask;
    public Button openButton;

    public TextMeshProUGUI nameTextField;
    public TextMeshProUGUI infoTextField;

    public NetLevel.LevelMetaData selectedLevelMetaData;
    private float bot = 86;

    private GameObject[] colorBars;
    private bool finished;
    private Vector2 lastObjectPosition;


    private Vector2 lastPosition;

    private LevelList list;
    private bool open;

    private UIAnimation openingAnimation;
    private float right = 159;
    private bool target;
    private Vector3 targetPosition;
    private Queue<Task> tasks;
    
    

    private void Awake()
    {
        colorBars = new GameObject[4];
        for (var i = 0; i < 4; i++) colorBars[i] = transform.Find("ColorSelect").Find(i.ToString()).gameObject;
    }

    private void Start()
    {
        finished = true;
        targetPosition = transform.position;
        openButton.onClick.AddListener(Click);


        UpdateScreen();
        tasks = new Queue<Task>();
        list = transform.parent.GetComponent<LevelList>();
    }

    private void Update()
    {
        UpdatePosition();
        UpdateAnimation();
    }

    public void UpdateScreen()
    {
        right = Screen.width * 0.5f * 0.65f;
        bot = Screen.height * 0.5f * 0.75f;

        Set(0);
    }

    public void Open()
    {
        list.SetSelectedLevel(selectedLevelMetaData);
        tasks.Enqueue(Task.Open);
    }

    public void Close()
    {
        if (list.GetSelectedLevel() == selectedLevelMetaData) list.SetSelectedLevel(null);
        tasks.Enqueue(Task.Close);
    }

    public void UpdateElement(NetLevel.LevelMetaData data)
    {
        if (data != null)
        {
        selectedLevelMetaData = data;
        nameTextField.SetText(data.FullName);
        infoTextField.SetText(data.Description);
        UpdateScreen();
        UpdateAvailColors();
        }
    }

    private void UpdateAvailColors()
    {
        colorBars[0].SetActive(selectedLevelMetaData.AvailBlue);
        colorBars[1].SetActive(selectedLevelMetaData.AvailYellow);
        colorBars[2].SetActive(selectedLevelMetaData.AvailRed);
        colorBars[3].SetActive(selectedLevelMetaData.AvailGreen);
    }

    private IEnumerator OpenAnim()
    {
        finished = false;
        open = true;
        for (var ft = 0f; ft <= 1; ft += 4 * Time.deltaTime)
        {
            Set(ft);
            yield return new WaitForSeconds(.01f);
        }

        Set(1);
        finished = true;
    }

    private IEnumerator CloseAnim()
    {
        finished = false;
        for (var ft = 1f; ft >= 0; ft -= 4 * Time.deltaTime)
        {
            Set(ft);
            yield return new WaitForSeconds(.01f);
        }

        Set(0);

        open = false;
        finished = true;
    }

    private void UpdateAnimation()
    {
        if (tasks.Count > 0)
            if (finished)
            {
                var t = tasks.Dequeue();
                if (t == Task.Open)
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

    public void Click()
    {
        if (!target)
        {
            if (list.listType == LevelList.ListType.Net)
            {
                // NetworkManager.instance.SendLevelSelection(d);
            }

            Open();
        }
        else
        {
            Close();
        }

        target = !target;
    }

    private void Set(float t)
    {
        if (t < 0 || t > 1) return;
        var r = (1 - t) * right;
        var b = (1 - t) * bot;
        mask.padding = new Vector4(0, b, r, 0);
        Canvas.ForceUpdateCanvases();
    }

    private void UpdatePosition()
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

    private enum Task
    {
        Open,
        Close
    }
}