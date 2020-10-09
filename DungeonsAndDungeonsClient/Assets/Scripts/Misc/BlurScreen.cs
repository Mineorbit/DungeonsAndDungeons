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

    void Start()
    {
        if (blurScreen != null) Destroy(this);
        blurScreen = this;

        open = false;
        t = 1;
        screen = transform.Find("Screen").gameObject;

        screen.SetActive(false);
    }

    void Update()
    {
    if(Camera.main != null)
        { 
        transform.LookAt(Camera.main.transform.position);
        transform.position = LerpPos(t);
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
        if (open||!finished) return;
        StartCoroutine("OpenAnim");
        screen.SetActive(true);
    }
    public void Close()
    {
        if (!open || !finished) return;
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
