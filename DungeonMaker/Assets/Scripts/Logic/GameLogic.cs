using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static GameLogic current;
    public static GameObject[] players;
	public int hp;
	public int mp;
    public int maxhp;
	public int maxmp;
    void Start()
    {
        current = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startRound()
    {
        players = GameManager.current.players;
    }
    public void spawnPlayers(GameObject[] players)
	{
		GameManager.current.currentLevel.spawn.doAction(players);
	}
    public void spawnPlayer(int p,Vector3 location)
	{
		players[p].transform.position = location + new Vector3(0,5,0);
	}
    public void stopRound()
    {

    }
}
