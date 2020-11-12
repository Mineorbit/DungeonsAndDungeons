using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{

    long levelId = 0;
    public static GameLogic current;
    
    
    public void Awake()
    {
        if (current != null) Destroy(this);
        current = this;
    }

    public static void CheckForWin(bool[] insidePlayers)
    {
        bool win = true;
        bool anyone = false;
        for(int i = 0;i<4;i++)
        {
            if (PlayerManager.playerManager.players[i] != null)
            {
                anyone = true;
                win = win && insidePlayers[i];
            }
        }
        if(win&&anyone)
        ServerManager.instance.performAction(ServerManager.GameAction.WinGame);
    }

    //Called on Victory
    public static void EndRound()
    {
        //Despawn Players
        for(int i = 0;i<4;i++)
        {
            current.DespawnPlayer(i);
        }

        if (GameLogic.current != null)
        {
            Destroy(GameLogic.current);
        }
    }

    public static void PrepareRound(Transform t)
    {
        t.gameObject.AddComponent<GameLogic>();
        //Hier war mal ein player spawn ist aber falsch per se
    }

    public static void ClearRound()
    {
        Level.Clear();

        for (int i = 0; i < 4; i++)
        {
            ServerManager.instance.RemoveClient(i);
        }
        EndRound();
    }

    public void StartRound()
    {
        //Reset Player Data if exists
        for (int i = 0; i < 4; i++)
        {
            if (PlayerManager.playerManager.players[i] != null)
                PlayerManager.playerManager.players[i].Reset();
        }
            //Set Level As Selected
            LevelData.LevelMetaData levelMetaData = LevelManager.GetTopLevel();
        if (levelMetaData == null)
        { ServerManager.instance.performAction(ServerManager.GameAction.EndGame); }
        else
        { 

            LevelManager.Load(levelMetaData);

            //Send LevelData

            //and Spawn Players in Positions
            for (int i = 0;i<4;i++)
            {
            Level.currentLevel.SendChunkAt(Level.currentLevel.spawn[i].transform.position, i);
            SpawnPlayer(i);
            }
        }

    }
    public void DespawnPlayer(int localId)
    {
        if (Level.currentLevel.spawn[localId] == null || PlayerManager.playerManager.players[localId] == null) return;
        PlayerManager.playerManager.players[localId].gameObject.SetActive(false);
        SetPlayerPosition(localId, new Vector3(0,0,0), true);
    }
    public void SpawnPlayer(int localId)
    {
        if (Level.currentLevel.spawn[localId] == null || PlayerManager.playerManager.players[localId] == null) return;
        Vector3 spawnLocation = Level.currentLevel.spawn[localId].transform.position;
        Debug.Log(localId+" : "+spawnLocation);
        PlayerManager.playerManager.players[localId].gameObject.SetActive(true);
        SetPlayerPosition(localId,spawnLocation,true);
    }
    public void SetPlayerPosition(int localId,Vector3 pos, bool allowMove)
    {
        if (PlayerManager.playerManager.players[localId] == null) return;

        PlayerSpawnPacket packet = new PlayerSpawnPacket(localId, pos, allowMove);

        PlayerManager.playerManager.players[localId].setPositionData(pos,new Quaternion(0,0,0,0));

        Server.SendPacketToAll(packet);
    }

    public static void SpawnPlayersInLobby()
    {
        for(int i = 0;i<4;i++)
        {
            GameLogic.current.SetPlayerPosition(i,GetLobbyPosition(i),false);
        }
    }

    public static Vector3 GetLobbyPosition(int localId)
    {
        return new Vector3(32 + localId * 8, 0, 0);
    }
    public void AddPlayer(int localId, Client c)
    {
        Vector3 spawnPosition = GetLobbyPosition(localId);
        GameObject g = ServerManager.instance.playerTarget.Create(spawnPosition);
        g.transform.position = spawnPosition;
        g.name = "Player|"+c.name+"|"+localId;
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
