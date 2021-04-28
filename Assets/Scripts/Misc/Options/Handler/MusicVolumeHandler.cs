using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.mineorbit.dungeonsanddungeonscommon;

public class MusicVolumeHandler : OptionHandler
{
    public override void OnValueChanged(object val)
    {
        AudioProfile.MusicVolume = (float)val;
    }
}
