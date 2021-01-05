using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LevelObject
{
    int health;

   public override void OnTestRoundStart()
    {
        gameObject.AddComponent<EnemyController>();
        gameObject.AddComponent<NavMeshAgent>();
        Init();
    }
    void Init()
    {
        health = 100;
    }
}
