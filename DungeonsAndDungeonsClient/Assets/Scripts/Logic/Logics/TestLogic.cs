using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLogic : Logic
{

    int player = 0;
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

        SpawnPlayers();
    }
    //This is ugly need better way
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Swap();
        }
    }
    public void Swap()
    {
            Debug.Log("Test");
            player = (player + 1) % 4;
            PlayerManager.playerManager.SetCurrentPlayer(player);
    }
    void SpawnPlayers()
    {
        for(int i = 0;i<4;i++)
        {
            PlayerManager.playerManager.DespawnPlayer(i);
            //HIER CHECK FÜR SPAWN PLACE LOGIC
            PlayerManager.playerManager.SpawnPlayer(i,new Vector3((float)i*5, 0, 0));
        }

        PlayerManager.playerManager.SetCurrentPlayer(player);
    }
    void DespawnPlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            PlayerManager.playerManager.DespawnPlayer(i);
        }
    }

    public override void Stop()
    {
        if (!running) return;
        DespawnPlayers();
        DespawnAll();
    }
    public override void DeInit()
    {
        
    }
    
    
}
