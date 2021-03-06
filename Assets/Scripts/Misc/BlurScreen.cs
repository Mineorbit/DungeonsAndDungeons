﻿using System.Collections;
using UnityEngine;

public class BlurScreen : Openable
{
    public static BlurScreen blurScreen;
    public float maxDist = 50;
    public float minDist = 1;
    public float t;
    private GameObject screen;

    private void Reset()
    {
        Setup();
    }


    private void Start()
    {
        if (blurScreen != null) Destroy(this);
        blurScreen = this;

        Setup();
    }

    public void Update()
    {
        screen.transform.position = LerpPos(t);
        screen.transform.LookAt(Camera.main.transform);
    }

    private void Setup()
    {
        t = 1;
        screen = transform.Find("Screen").gameObject;
        screen.SetActive(false);
    }


    private Vector3 LerpPos(float t)
    {
        var camPos = Camera.main.transform.position;
        var camFor = Camera.main.transform.forward;
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

    private IEnumerator OpenAnim()
    {
        for (var ft = 1f; ft >= 0; ft -= Time.deltaTime)
        {
            t = ft;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        Finished = true;
    }

    private IEnumerator CloseAnim()
    {
        for (var ft = 0f; ft <= 1; ft += Time.deltaTime)
        {
            t = ft;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        screen.SetActive(false);
        Finished = true;
    }
}