using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;

public class MusicVolumeHandler : OptionHandler
{
    public override void OnValueChanged(object val)
    {
        Debug.Log("Setting Music volume to "+val);
        AudioProfile.MusicVolume = (float) val;
    }
}