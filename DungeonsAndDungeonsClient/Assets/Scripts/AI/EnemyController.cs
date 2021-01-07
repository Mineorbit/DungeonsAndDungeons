using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    Player seenPlayer;
    float viewDistance = 15;
    float attackDistance = 4;
    public float distToTarget = float.MaxValue;
    public enum EnemyState { Idle = 1, Track, Attack };
    public EnemyState enemyState;

    Vector3 lastPositionOfPlayer;
    NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyState = EnemyState.Idle;
    }

    void FixedUpdate()
    {
        UpdateDistance();
        UpdateState();

        if (enemyState == EnemyState.Track)
        {
            navMeshAgent.SetDestination(lastPositionOfPlayer);
        }
        else if (enemyState == EnemyState.Attack)
        {
            navMeshAgent.SetDestination(transform.position);
            transform.LookAt(lastPositionOfPlayer);
        }
    }

    void UpdateDistance()
    {

        seenPlayer = CheckVisiblePlayer();
        if (seenPlayer != null)
        {
            distToTarget = (seenPlayer.transform.position - transform.position).magnitude;
            lastPositionOfPlayer = seenPlayer.transform.position;
        }
        else
        {
            distToTarget = float.MaxValue;
        }
    }

    void UpdateState()
    {
        if (attackDistance <= distToTarget && distToTarget <= viewDistance)
        {
            if (seenPlayer != null)
                enemyState = EnemyState.Track;
        }
        else if (distToTarget <= attackDistance)
        {
            if (seenPlayer != null)
                enemyState = EnemyState.Attack;
        }
        else
        {
            enemyState = EnemyState.Idle;
        }
    }

    Player CheckVisiblePlayer()
    {
        float minDist = float.MaxValue;
        Player minPlayer = null;
        for (int i = 0; i < 4; i++)
        {
            Player p = PlayerManager.playerManager.players[i];
            if (p != null)
            {
                Vector3 dir = p.transform.position - transform.position;
                float dist = dir.magnitude;
                if (Vector3.Dot(transform.forward, dir) > 0 && dist <= viewDistance && dist < minDist)
                {
                    minPlayer = p;
                    minDist = dist;
                }
            }
        }
        return minPlayer;
    }
}
