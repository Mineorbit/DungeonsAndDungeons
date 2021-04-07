using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager instance;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else instance = this;
    }

    public void startCoroutine(IEnumerator ie)
    {
        StartCoroutine(ie);
    }
}
