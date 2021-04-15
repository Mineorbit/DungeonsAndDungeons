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

        builder = created[0];
    }
    
    public override void Start()
    {
        if (running) return;
        base.Start();
        Debug.Log("Starting Edit");
        SpawnAll();
        SpawnBuilder(new Vector3(0,0,0));
        PlayerController.doSim = false;
    }
    public override void Stop()
    {
        if (!running) return;
        base.Stop();
        Debug.Log("Stopping Edit");

        DespawnAll();
    }
    public override void DeInit()
    {
        
    }
    void DespawnBuilder()
    {
        builder.SetActive(false);
    }
    void SpawnBuilder(Vector3 location)
    {
        builder.transform.position = location;
        builder.SetActive(true);
        //Noch HUD Aktivieren
    }

}
