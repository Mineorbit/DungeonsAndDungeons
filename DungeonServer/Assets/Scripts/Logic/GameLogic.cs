using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{

    long levelId = 0;
    public static GameLogic current;
    
    
    public void Start()
    {
        if (current != null) Destroy(this);
        current = this;
        Setup();
    }
    

    public void Setup()
    {

    }

    public static void ClearRound()
    {
        if (GameLogic.current != null)
        {
            Destroy(GameLogic.current);
        }
    }
    public static void PrepareRound(Transform t)
    {
        t.gameObject.AddComponent<GameLogic>();
    }

    public void StartRound()
    {
        //Set Level As Selected

        LevelManager.LoadOnline(OnlineLevelOrganizer.onlineLevelOrganizer.GetTopLevel());

        //Send LevelData

        //and Spawn Players in Positions
        for (int i = 0;i<4;i++)
        {

            Level.currentLevel.SendChunkAt(Level.currentLevel.spawn[i].transform.position, i);
            SpawnPlayer(i);
        }



    }

    public void SpawnPlayer(int localId)
    {
        if (Level.currentLevel.spawn[localId] == null || PlayerManager.playerManager.players[localId] == null) return;
        Vector3 spawnLocation = Level.currentLevel.spawn[localId].transform.position;
        PlayerSpawnPacket packet = new PlayerSpawnPacket(localId,spawnLocation);

        PlayerManager.playerManager.players[localId].transform.position = spawnLocation;

        Server.SendPacketToAll(packet);

    }

    public void AddPlayer(int localId, Client c)
    {
        GameObject g = ServerManager.instance.playerTarget.Create(new Vector3(32+localId*8,0,0));
        g.name = "Player"+c.name;
        Player p = g.GetComponent<Player>();
        p.name = c.name;
        p.localId = localId;
        p.client = c;
        p.Setup();
    }
    public void RemovePlayer(int localId)
    {
        PlayerManager.playerManager.RemovePlayer(localId);
    }
    
}
