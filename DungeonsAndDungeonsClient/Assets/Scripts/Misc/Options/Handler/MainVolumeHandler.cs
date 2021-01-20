using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainVolumeHandler : OptionHandler
{
    public override void OnValueChanged(object val)
    {
        AudioListener.volume = (float) val;
        Debug.Log("Test wir haben: "+val);
    }
}
