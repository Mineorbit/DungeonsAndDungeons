using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditLogic : Logic
{
    GameObject builder;
    public override void Init()
    {
        builder = GameObject.Find("Builder");
    }
    public override void Start()
    {
        if (running) return;
        base.Start();
        SpawnBuilder(new Vector3(0,0,0));
    }
    public override void Stop()
    {

    }
    void SpawnBuilder(Vector3 location)
    {
        builder.transform.position = location;
        builder.SetActive();
        //Noch HUD Aktivieren
    }
}
