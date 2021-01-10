using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LevelObject
{
    EnemyController enemyController;
   public override void OnTestRoundStart()
   {
        enemyController = gameObject.AddComponent<EnemyController>();
        Destroy(GetComponent<SphereCollider>());
   }
   public EnemyController.EnemyState GetState()
   {
   return enemyController.enemyState;
   }
}
