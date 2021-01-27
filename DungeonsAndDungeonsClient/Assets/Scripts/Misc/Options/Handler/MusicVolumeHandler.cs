using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVolumeHandler : OptionHandler
{
    public static float volume;
    public override void OnValueChanged(object val)
    {
        volume = (float)val;
    }
}
