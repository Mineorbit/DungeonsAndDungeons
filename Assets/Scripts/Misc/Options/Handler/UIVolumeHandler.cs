using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVolumeHandler : OptionHandler
{
    public static float volume;
    public override void OnValueChanged(object val)
    {
        volume = (float)val;
    }
}
