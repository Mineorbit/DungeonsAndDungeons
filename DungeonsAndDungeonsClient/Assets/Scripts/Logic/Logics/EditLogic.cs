using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditLogic : Logic
{
    GameObject[] created;
    GameObject builder;
    public override void Init()
    {
        Debug.Log("Init Build");
        created = Instantiator.InstantiateAssets();
        builder = created[0];
    }
    
    public override void Start()
    {
        if (running) return;
        base.Start();
        SpawnBuilder(new Vector3(0,0,0));
    }
    public override void Stop()
    {
        if (!running) return;
        DespawnBuilder();
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
