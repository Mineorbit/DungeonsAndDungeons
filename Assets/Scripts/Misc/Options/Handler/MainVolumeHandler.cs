using UnityEngine;

public class MainVolumeHandler : OptionHandler
{
    public override void OnValueChanged(object val)
    {
        AudioListener.volume = (float) val;
    }
}