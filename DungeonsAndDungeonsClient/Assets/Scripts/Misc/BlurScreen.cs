using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurScreen : MonoBehaviour
{
    public static BlurScreen blurScreen;
    float maxDist = 1000;
    float minDist = 1;
    public float t;
    bool open = false;
    void Start()
    {
        if (blurScreen != null) Destroy(this);
        blurScreen = this;
        open = false;
        t = 1;
    }

    void Update()
    {
    if(Camera.main != null)
    transform.LookAt(Camera.main.transform.position);

        transform.position = LerpPos(t);
    }

    Vector3 LerpPos(float t)
    {
        Vector3 camPos = Camera.main.transform.position;
        Vector3 camFor = Camera.main.transform.forward;
        return (1 - t) * (camPos + camFor * minDist) + t * (camPos + camFor * maxDist);
    }

    IEnumerator Open()
    {
        return null;
    }
    IEnumerator Close()
    {

        return null;
    }
}
