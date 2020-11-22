using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager playerManager;
    public Player[] players;
    void Start()
    {
        if (playerManager != null) Destroy(this);
        playerManager = this;
        players = new Player[4];
    }
    public static bool Exists(int localId)
    {
        return PlayerManager.playerManager.players[localId] != null;
    }
    public static void RemovePlayer(int localId)
    {
        if (PlayerManager.Exists(localId))
            Destroy(PlayerManager.playerManager.players[localId].gameObject);
    }

    public static void AddPlayer(int localId, Client c)
    {

        if (!Exists(localId))
        { 
        Vector3 spawnPosition = GetLobbyPosition(localId);
        GameObject g = ServerManager.instance.playerTarget.Create(spawnPosition);
        g.transform.position = spawnPosition;
        g.name = "Player|" + c.name + "|" + localId;
        Player p = g.GetComponent<Player>();
        p.name = c.name;
        p.localId = localId;
        p.client = c;
        p.Setup();

        playerManager.players[localId] = p;
        }
    }


    public static void DespawnPlayer(int localId)
    {
        if (Level.currentLevel.spawn[localId] == null || PlayerManager.playerManager.players[localId] == null) return;
        PlayerManager.playerManager.players[localId].gameObject.SetActive(false);
        SetPlayerPosition(localId, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), false);
    }
    public static void SpawnPlayer(int localId)
    {
        if (Level.currentLevel.spawn[localId] == null || PlayerManager.playerManager.players[localId] == null) return;
        Vector3 spawnLocation = Level.currentLevel.spawn[localId].transform.position;
        Debug.Log(localId + " : " + spawnLocation);
        PlayerManager.playerManager.players[localId].gameObject.SetActive(true);
        SetPlayerPosition(localId, spawnLocation,new Quaternion(0,0,0,0), true);
    }
    public static void SetPlayerPosition(int localId, Vector3 pos,Quaternion rot, bool allowMove)
    {
        if (PlayerManager.playerManager.players[localId] == null) return;

        Debug.Log($"Setting Player {localId} Position to: {pos} ");
        PlayerSpawnPacket packet = new PlayerSpawnPacket(localId, pos, rot, allowMove);

        PlayerManager.playerManager.players[localId].setPositionData(pos, new Quaternion(0, 0, 0, 0));

        Server.SendPacketToAll(packet);
    }

    public static void SpawnPlayersInLobby()
    {
        for (int i = 0; i < 4; i++)
        {
            SpawnPlayerInLobby(i);
        }
    }

    public static void SpawnPlayerInLobby(int i)
    {
    PlayerManager.SetPlayerPosition(i, GetLobbyPosition(i), new Quaternion(0, 0, 0, 0), false);
    }

    public static Vector3 GetLobbyPosition(int localId)
    {
        return new Vector3(32 + localId * 8, 0, 0);
    }
}
