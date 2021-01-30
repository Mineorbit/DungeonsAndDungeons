using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LevelObject
{
    EnemyController enemyController;
    public Transform t;
   public override void OnTestRoundStart()
   {
        enemyController = gameObject.AddComponent<EnemyController>();
        gameObject.AddComponent<NavMeshAgent>();
        Destroy(GetComponent<BoxCollider>());
        t = transform.Find("enemy_model_holder");
        t.localEulerAngles = new Vector3(0,0,0);
   }
   public EnemyController.EnemyState GetState()
   {
   return enemyController.enemyState;
   }
}
