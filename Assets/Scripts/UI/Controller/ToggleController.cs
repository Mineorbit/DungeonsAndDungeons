using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    public Option option;
    public Toggle toggle;
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = (bool)option.Value;
        toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(toggle);
        });

    }

   

    void ToggleValueChanged(Toggle change)
    {
        if (toggle.isOn != (bool) option.Value)
        {
            option.Value = toggle.isOn;
        }
        OptionManager.Save();
    }
}
