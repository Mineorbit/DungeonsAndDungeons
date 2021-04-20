using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.mineorbit.dungeonsanddungeonscommon;

public class TestLogic : Logic
{

    int player = 0;
    public override void Init()
    {
        sceneIndex = 2;
    }
    void CreatePlayers()
    {
        for(int i = 0;i<4;i++)
        {
            PlayerManager.playerManager.Add(i,"Rot", true);
        }
    }

    public override void Start()
    {
        if (running) return;
        base.Start();
        SpawnAll();
        CreatePlayers();
        Debug.Log("Players spawned");
        LevelManager.StartRound();
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

        if (LevelManager.currentLevel.spawn[i] != null) return (LevelManager.currentLevel.spawn[i].transform.position+new Vector3(0,1.5f,0));
        return new Vector3(i*4,0.25f,0);
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
        //LevelManager.StopRound();
        if (!running) return;
        RemovePlayers();
        DespawnAll();
    }

    public override void DeInit()
    {
        
    }
    
    
}
