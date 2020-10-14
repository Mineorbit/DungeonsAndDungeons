using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public Player[] players;

    long levelId = 0;
    public static GameLogic current;
    public void Start()
    {
        if (current != null) Destroy(this);
        current = this;
        players = new Player[4];
    }
    public void prepareRound()
    {

    }
    public static void ClearRound()
    {
        if (GameLogic.current != null)
        {
            Destroy(GameLogic.current);
        }
    }
    public static void StartRound(Transform t)
    {
        t.gameObject.AddComponent<GameLogic>();
    }
    public void AddPlayer(int localId, Client c)
    {
        GameObject g = ServerManager.instance.playerTarget.Create(new Vector3(0,0,0));
        g.name = "Player"+c.name;
        Player p = g.GetComponent<Player>();
        p.name = c.name;
        p.localId = localId;
        p.client = c;
        players[localId] = p;
    }

    
    public void RemovePlayer(int localId)
    {
        if(players[localId]!=null)
        Destroy(players[localId].gameObject);
    }
}
