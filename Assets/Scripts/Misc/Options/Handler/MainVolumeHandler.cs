using UnityEngine;
using com.mineorbit.dungeonsanddungeonscommon;

public class MainVolumeHandler : OptionHandler
{
    public override void OnValueChanged(object val)
    {
        AudioListener.volume = (float) val;
    }
}