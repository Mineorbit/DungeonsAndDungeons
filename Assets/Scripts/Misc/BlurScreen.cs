using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurScreen : Openable
{
    public static BlurScreen blurScreen;
    GameObject screen;
    public float maxDist = 50;
    public float minDist = 1;
    public float t;


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
        t = 1;
        screen = transform.Find("Screen").gameObject;
        screen.SetActive(false);
    }


    Vector3 LerpPos(float t)
    {
        Vector3 camPos = Camera.main.transform.position;
        Vector3 camFor = Camera.main.transform.forward;
        return (1 - t) * (camPos + camFor * minDist) + t * (camPos + camFor * maxDist);
    }

    public override void OnOpen()
    {
        screen.SetActive(true);
        StartCoroutine("OpenAnim");
    }
    public override void OnClose()
    {
        StartCoroutine("CloseAnim");
    }

    public bool isOpen()
    {
        return open;
    }
    public void Update()
    {
        screen.transform.position = LerpPos(t);
        screen.transform.LookAt(Camera.main.transform);
    }

    IEnumerator OpenAnim()
    {
        for (float ft = 1f; ft >= 0; ft -=  Time.deltaTime)
        {
            t = ft;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Finished = true;
    }
    IEnumerator CloseAnim()
    {
        for (float ft = 0f; ft <= 1; ft +=  Time.deltaTime)
        {
            t = ft;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        screen.SetActive(false);
        Finished = true;
    }
}
