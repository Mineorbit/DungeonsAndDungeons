using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static GameLogic current;
    public int playerCount=1;
    public Player[] players;

    Vector3 spawnPoint;
    UnityEngine.Object player;
    void Start()
    {
        current = this;
        players = new Player[4];
        player = Resources.Load("Main/Player/Player");
    }
    public void Win()
    {
        
    }
    
    void FixedUpdate()
    {
        
    }

    public void setupPlayer(int localId)
    {
        GameObject p = (GameObject) Instantiate(player) as  GameObject;
        Player pp = p.GetComponent<Player>();
        p.name = "Player"+localId;
        pp.id=localId;
        Vector3 offset = new Vector3(Mathf.Sin((Mathf.PI/2)*localId),0,-Mathf.Cos((Mathf.PI/2)*localId));
        offset *=2;
        p.transform.position = spawnPoint+offset;
        players[localId] = pp;
    }
    public void  checkAllPlayersReady()
    {
        int c = 0;

        for(int i = 0;i<playerCount;i++)
        {
            if(players[i]!=null)c++;
        }

        if(c==playerCount)
        {
            ServerSend.GameReady();
        }
    }
}
