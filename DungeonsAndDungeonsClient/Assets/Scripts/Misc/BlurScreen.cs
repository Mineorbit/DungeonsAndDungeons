using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurScreen : MonoBehaviour
{
    public static BlurScreen blurScreen;
    GameObject screen;

    float maxDist = 50;
    float minDist = 1;
    public float t;
    bool open = false;
    bool finished = true;
    enum Task { Open, Close };
    Queue<Task> todo;
    void Start()
    {
        if (blurScreen != null) Destroy(this);
        blurScreen = this;

        Setup();

    }

    void Reset()
    {
        Setup();
    }
    void Setup()
    {
        open = false;
        t = 1;
        screen = transform.Find("Screen").gameObject;
        todo = new Queue<Task>();
        screen.SetActive(false);
    }

    void Update()
    {
    if(Camera.main != null)
        { 
        transform.LookAt(Camera.main.transform.position);
        transform.position = LerpPos(t);
        }
        if(todo.Count>0)
        if(finished)
        {
                Task t = todo.Dequeue();
                if(t == Task.Close)
                {
                    Close();
                }
                if (t == Task.Open)
                {
                    Open();
                }
            }


    }

    Vector3 LerpPos(float t)
    {
        Vector3 camPos = Camera.main.transform.position;
        Vector3 camFor = Camera.main.transform.forward;
        return (1 - t) * (camPos + camFor * minDist) + t * (camPos + camFor * maxDist);
    }

    public void Open()
    {
        if (open || !finished) { todo.Enqueue(Task.Open); return; }
        StartCoroutine("OpenAnim");
        screen.SetActive(true);
    }
    public void Close()
    {
        if (!open || !finished) { todo.Enqueue(Task.Close); return; }
        StartCoroutine("CloseAnim");
    }
    public bool isOpen()
    {
        return open;
    }
    IEnumerator OpenAnim()
    {

        finished = false;
        open = false;
        for (float ft = 1f; ft >= 0; ft -= 2* Time.deltaTime)
        {
            t = ft;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        open = true;
        finished = true;
    }
    IEnumerator CloseAnim()
    {
        open = true;
        finished = false;
        for (float ft = 0f; ft <= 1; ft += 2 * Time.deltaTime)
        {
            t = ft;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        open = false;
        screen.SetActive(false);
        finished = true;
    }
}
