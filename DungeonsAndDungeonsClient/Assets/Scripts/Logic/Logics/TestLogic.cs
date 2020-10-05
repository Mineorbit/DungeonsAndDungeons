using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLogic : Logic
{

    GameObject[] created;
    public override void Init()
    {
        Debug.Log("Init Test");
        created = Instantiator.InstantiateAssets("test");
    }

    public override void Start()
    {
        if (running) return;
        base.Start();
        SpawnPlayer(new Vector3(0, 0, 0));
    }
    public override void Stop()
    {
        if (!running) return;
        DespawnPlayer();
    }
    void DespawnPlayer()
    {
        PlayerController.Despawn();
    }
    void SpawnPlayer(Vector3 location)
    {
        //Move to other class Player eventually
        PlayerController.Setup();
        PlayerCameraController.Setup();
        PlayerController.Spawn(new Vector3(0,0,0));
        //Noch HUD Aktivieren
    }
}
