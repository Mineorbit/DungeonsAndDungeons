using UnityEngine;
using UnityEngine.UI;
using com.mineorbit.dungeonsanddungeonscommon;

public class SliderController : MonoBehaviour
{
    public Option option;
    public Slider slider;
    public float f;

    private void Start()
    {
        slider = GetComponent<Slider>();
        f = (float) option.Value;
        slider.value = f;
        slider.onValueChanged.AddListener(ValueChange);
    }

    private void ValueChange(float t)
    {
        if (slider.value != (float) option.Value) option.Value = slider.value;
        OptionManager.Save();
    }
}