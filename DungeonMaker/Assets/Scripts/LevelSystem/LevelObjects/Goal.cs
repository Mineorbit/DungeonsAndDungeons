using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : LevelObject
{
List<Player> playersInGoal;
bool won = false;

public void Start()
{
    instance = this.gameObject;
    setInpoints();
}

public override void onRoundStart()
{
won = false;
playersInGoal = new List<Player>();
}
 
public override void updateTest()
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
