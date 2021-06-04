using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Switcher : MonoBehaviour
{
    public UnityEngine.Object A;
    public UnityEngine.Object B;

    public Slider slider;


    public UnityEngine.Object selected;
    
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
                ((MonoBehaviour) A).enabled = false;
                ((MonoBehaviour) B).enabled = true;
        }
        else
        {
            value = false;
            selected = A;
                ((MonoBehaviour) A).enabled = true;
                ((MonoBehaviour) B).enabled = false;
        }
        valueChanged.Invoke(value);
    }
}
