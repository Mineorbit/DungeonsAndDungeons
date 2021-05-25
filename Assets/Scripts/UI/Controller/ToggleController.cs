using UnityEngine;
using UnityEngine.UI;
using com.mineorbit.dungeonsanddungeonscommon;

public class ToggleController : MonoBehaviour
{
    public Option option;
    public Toggle toggle;

    private void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = (bool) option.Value;
        toggle.onValueChanged.AddListener(delegate { ToggleValueChanged(toggle); });
    }


    private void ToggleValueChanged(Toggle change)
    {
        if (toggle.isOn != (bool) option.Value) option.Value = toggle.isOn;
        OptionManager.Save();
    }
}