using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.mineorbit.dungeonsanddungeonscommon;
public class EditLogic : Logic
{
    GameObject builder;
    public override void Init()
    {
        sceneIndex = 3;

    }
    
    public override void Start()
    {
        if (running) return;
        base.Start();
        Debug.Log("Starting Edit");

        PlayerController.doSim = false;
    }
    public override void Stop()
    {
        if (!running) return;
        base.Stop();
        Debug.Log("Stopping Edit");

    }
    public override void DeInit()
    {
        
    }
    

}
