using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : LevelObject
{
List<Player> playersInGoal;
bool won = false;

public void Start()
{
}

public override void onRoundStart()
{
won = false;
playersInGoal = new List<Player>();
}
public void Update()
{
    if(playersInGoal.Count == GameLogic.current.playerCount&&!won)
    {
        won =  true;
        doAction();
    }
}

public override void doAction()
{
    Debug.Log("Hurra!");
    TestLogic.current.Win();
}
void OnTriggerEnter(Collider other)
{
    if(other.gameObject.GetComponent<Player>()!=null)
    {
        Player p = other.gameObject.GetComponent<Player>();
        playersInGoal.Add(p);
    }
}
void OnTriggerExit(Collider other)
{
    if(other.gameObject.GetComponent<Player>()!=null)
    {
        Player p = other.gameObject.GetComponent<Player>();
        playersInGoal.Remove(p);
    }
}


}
