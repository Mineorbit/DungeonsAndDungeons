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
        CreatePlayers();
    }
    void CreatePlayers()
    {
        for(int i = 0;i<4;i++)
        {
            PlayerManager.playerManager.Add(i,"Rot");
        }
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
            player = (player + 1) % 4;
            PlayerManager.playerManager.SetCurrentPlayer(player);
    }
    Vector3 GetSpawnLocation(int i)
    {

        if (Level.currentLevel.spawn[i] != null) return (Level.currentLevel.spawn[i].transform.position+new Vector3(0,1.5f,0));
        return new Vector3(i*4,0.25f,0);
    }

    void SpawnPlayers()
    {
        for(int i = 0;i<4;i++)
        {
            PlayerManager.playerManager.DespawnPlayer(i);
            //HIER CHECK FÜR SPAWN PLACE LOGIC
            Vector3 location = GetSpawnLocation(i);
            Debug.Log("Spawning player at: " + location);
            PlayerManager.playerManager.SpawnPlayer(i,location);
        }

        PlayerManager.playerManager.SetCurrentPlayer(player);
    }

    void RemovePlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            PlayerManager.playerManager.Remove(i);
        }
    }

    public override void Stop()
    {
        if (!running) return;
        RemovePlayers();
        DespawnAll();
    }
    public override void DeInit()
    {
        
    }
    
    
}
