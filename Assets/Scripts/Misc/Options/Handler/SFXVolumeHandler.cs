using com.mineorbit.dungeonsanddungeonscommon;

public class SFXVolumeHandler : OptionHandler
{
    public override void OnValueChanged(object val)
    {
        AudioProfile.SFXVolume = (float) val;
    }
}