using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : LevelObject
{
List<Player> playersInGoal;

public void Start()
{
playersInGoal = new List<Player>();
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
