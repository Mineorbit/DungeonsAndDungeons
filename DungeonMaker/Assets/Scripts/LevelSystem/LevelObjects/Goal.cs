using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : LevelObject
{
List<Player> playersInGoal;

public void Start()
{

}
void OnTriggerEnter(Collider other)
{
Debug.Log("Test");
}
void OnTriggerExit(Collider other)
{

}


}
