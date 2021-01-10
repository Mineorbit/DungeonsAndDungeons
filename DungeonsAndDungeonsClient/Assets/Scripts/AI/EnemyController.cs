
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    Enemy me;
    public Player seenPlayer;
    public Enemy seenAlly;
    public int health;
    public int damage;


    float viewDistance = 15;
    float attackDistance = 4;


    float distToTarget = float.MaxValue;

    public enum EnemyState {Idle = 1, Track, Attack, PrepareStrike ,Strike, Dead};
    public EnemyState enemyState;
    Vector3 lastPositionOfPlayer;

    BlobAnimator animator;

    NavMeshAgent navMeshAgent;
    System.Random rand;

    void Start()
    {
        me = GetComponent<Enemy>();

        rand = new System.Random();
        health = 100;

        damage = 2;

        navMeshAgent = GetComponent<NavMeshAgent>();

        animator = GetComponentInChildren<BlobAnimator>();

        animator.endAttackEvent.AddListener(FinishStrike);

        enemyState = EnemyState.Idle;
    }

    void FixedUpdate()
    {
        UpdateDistance();
        UpdateState();

        UpdateTarget();
      
    }

    void UpdateTarget()
    {
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
        seenAlly = CheckClosestAlley();
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

    public IEnumerator strikeTimer;

    void UpdateState()
    {
        if(enemyState != EnemyState.Strike || enemyState!= EnemyState.PrepareStrike)
        {

        if (attackDistance <= distToTarget && distToTarget <= viewDistance)
        {
            if (seenPlayer != null)
                {
                    enemyState = EnemyState.Track;
                    if(strikeTimer != null)
                    { 
                        StopCoroutine(strikeTimer);
                        strikeTimer = null;
                    }
                }
        }
        else if (distToTarget <= attackDistance)
        {
            if (seenPlayer != null)
                { 
                    enemyState = EnemyState.Attack;
                    if(seenAlly == null)
                    {
                        TryStrike();
                    }else
                    if(seenAlly.GetState() != EnemyState.Strike || seenAlly.GetState() != EnemyState.PrepareStrike)
                    {
                        TryStrike();
                    }
                }
            }
        else
        {
            enemyState = EnemyState.Idle;
                if (strikeTimer != null)
                    StopCoroutine(strikeTimer);
        }

        }
    }

    public void TryStrike()
    {
        if (strikeTimer == null)
        {
            enemyState = EnemyState.PrepareStrike;
            strikeTimer = StrikeTimer((1 +  5 * (float) rand.NextDouble()));
            StartCoroutine(strikeTimer);
        }
    }
    IEnumerator StrikeTimer(float strikeTime)
    {
        yield return new WaitForSeconds(strikeTime);
        Strike();
    }

    void Strike()
    {
        if(seenAlly!=null)
        {
        if (seenAlly.GetState() != EnemyState.Strike || seenAlly.GetState() != EnemyState.PrepareStrike)
            {
                strikeTimer = null;
                enemyState = EnemyState.Strike;
                animator.Attack();
            }
            else
            {
            enemyState = EnemyState.Attack;
            }
        }else
        {
            strikeTimer = null;
            enemyState = EnemyState.Strike;
            animator.Attack();
        }
    }

    public void  FinishStrike()
    {
        enemyState = EnemyState.Attack;
    }

    void OnTriggerEnter(Collider collision)
    {
        GameObject col = collision.gameObject;
        if (col.tag == "Item")
        {
            //int damage = col.GetComponent<EnemyController>().damage;
            int damage = 20;
            if (damage > 0)
            {
                Hit(damage);
            }
        }
    }

    bool hitCooldown = false;

    IEnumerator HitTimer(float time)
    {
        yield return new WaitForSeconds(time);
        hitCooldown = false;
    }


    void StartHitCooldown()
    {
        hitCooldown = true;
        StartCoroutine(HitTimer(2));
    }

    public virtual void Hit(int damage)
    {
        if (!hitCooldown)
        {
            StartHitCooldown();
            health = health - damage;
            if (health == 0)
            {
                Kill();
            }
        }

    }

    public void Kill()
    {
        enemyState = EnemyState.Dead;
        Level.currentLevel.RemoveObject(GetComponent<Enemy>());
    }

    Enemy CheckClosestAlley()
    {
        float minDist = float.MaxValue;
        Enemy minEnemy = null;
        foreach (Enemy e in Level.GetAllEnemies())
        {
            if (e != null  && e!=me && e.GetState() != EnemyState.Dead)
            {
               
                    Vector3 dir = e.transform.position - transform.position;
                    float dist = dir.magnitude;

                    if (dist <= attackDistance && dist < minDist)
                    {
                        minEnemy = e;
                        minDist = dist;
                    }
            }
        }
        return minEnemy;
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
                if(p.IsAlive())
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
        }
        return minPlayer;
    }
}
