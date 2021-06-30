using System;
using System.Collections;
using System.Collections.Generic;
using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    public Light keyLight;


    public float buildIntensity = 1000;
    public float buildRange = 100;

    public float defaultIntensity = 60;
    public float defaultRange = 10;
    
    private float targetIntensity;
    private float targetRange;
    public void Update()
    {
        if (Level.instantiateType == Level.InstantiateType.Edit)
        {
            targetIntensity = buildIntensity;
            targetRange = buildRange;
        }
        else
        {
            targetIntensity = defaultIntensity;
            targetRange = defaultRange;
        }

        keyLight.intensity = (targetIntensity + keyLight.intensity) / 2;
        keyLight.range = (targetRange + keyLight.range) / 2;
    }
}
