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
    Level currentLevel;
    void Start()
    {
        current = this;
        players = new Player[4];
        player = Resources.Load("Main/Player/Player");
    }
    public virtual void loadLevel()
    {

    }
    public void Win()
    {
        
    }
    
    void FixedUpdate()
    {
        
    }
    Vector3 getSpawnPoint()
    {
        if(currentLevel==null)
        {
            return new Vector3(20,20,20);
        }else
        {
            return currentLevel.spawn.transform.position+new Vector3(0,5,0);
        }
    }
    public void setupPlayer(int localId)
    {
        GameObject p = (GameObject) Instantiate(player) as  GameObject;
        Player pp = p.GetComponent<Player>();
        p.name = "Player"+localId;
        pp.id=localId;
        Vector3 offset = new Vector3(Mathf.Sin((Mathf.PI/4)*(float)localId),0,-Mathf.Cos((Mathf.PI/4)*(float)localId));
        offset *=3;

        spawnPoint = getSpawnPoint();
       
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
