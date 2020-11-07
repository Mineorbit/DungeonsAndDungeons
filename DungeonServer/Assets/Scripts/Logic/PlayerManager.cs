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
    
    public void RemovePlayer(int localId)
    {
        if (PlayerManager.playerManager.players[localId] != null)
            Destroy(PlayerManager.playerManager.players[localId].gameObject);
    }
    public void AddPlayer(Player p,int localId)
    {

        if (players[localId] == null)
            players[localId] = p;
    }
}
