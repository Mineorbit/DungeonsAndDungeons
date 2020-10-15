using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLogic : Logic
{

    public override void Init()
    {
        sceneIndex = 2;
        created = Instantiator.InstantiateAssets("test");
    }

    public override void Start()
    {
        if (running) return;
        base.Start();
        SpawnAll();
        SpawnPlayer(new Vector3(0, 0, 0));
    }
    
    public override void Stop()
    {
        if (!running) return;
        DespawnPlayer();
        DespawnAll();
    }
    public override void DeInit()
    {
        
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
