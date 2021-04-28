using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.mineorbit.dungeonsanddungeonscommon;

public class UIVolumeHandler : OptionHandler
{
    public override void OnValueChanged(object val)
    {
        AudioProfile.UiVolume = (float)val;
    }
}
