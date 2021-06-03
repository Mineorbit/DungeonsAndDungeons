using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Switcher : MonoBehaviour
{
    public GameObject A;
    public GameObject B;

    public Slider slider;


    public GameObject selected;
    
    public bool value;

    public UnityEvent<bool> valueChanged = new UnityEvent<bool>();
    
    public bool initialSelection;

    public void Start()
    {
        value = initialSelection;
        slider.value = 2;
        slider.onValueChanged.AddListener((x)=>Change(x));
        slider.value = initialSelection ? 1 : 0;
    }

    void Change(float x)
    {
        if (x > 0)
        {
            value = true;
            selected = B;
            A.SetActive(false);
            B.SetActive(true);
        }
        else
        {
            value = false;
            selected = A;
            A.SetActive(true);
            B.SetActive(false);
        }
        valueChanged.Invoke(value);
    }
}
