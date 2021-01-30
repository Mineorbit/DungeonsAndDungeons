using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderController : MonoBehaviour
{
    public Option option;
    public Slider slider;
    public float f;
    void Start()
    {
        slider = GetComponent<Slider>();
        f = (float)option.Value;
        slider.value = f;
        slider.onValueChanged.AddListener(ValueChange);
    }

    void ValueChange(float t)
    {
        if(slider.value != (float) option.Value)
        {
            option.Value = slider.value;
        }
        OptionManager.Save();
    }
}
