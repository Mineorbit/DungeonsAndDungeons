using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LevelObject
{
   public override void OnTestRoundStart()
    {
        gameObject.AddComponent<EnemyController>();
        gameObject.AddComponent<NavMeshAgent>();
    }
}
