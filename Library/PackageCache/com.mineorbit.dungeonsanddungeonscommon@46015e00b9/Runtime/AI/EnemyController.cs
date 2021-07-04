using UnityEngine;
using UnityEngine.AI;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class EnemyController : EntityController
    {
        public static float forgetTime = 5f;

        public Enemy me;


        public Player seenPlayer;
        public Player lastSeenPlayer;
        public Enemy seenAlly;
        

        //Queue<Vector3> targetPoints = new Queue<Vector3>();

        private Vector3 currentTarget = new Vector3(0, 0, 0);


        private float distToTarget = float.MaxValue;


        private Vector3 lastPositionOfPlayer;


        private NavMeshAgent navMeshAgent;

        private TimerManager.Timer visibilityTimer;

        public override void Start()
        {
            base.Start();

            me = GetComponent<Enemy>();


            navMeshAgent = GetComponent<NavMeshAgent>();

            currentTarget = transform.position;

            SetTrackingAbility(true);
        }

        public override void Update()
        {
            UpdateLocomotion();

            currentSpeed = navMeshAgent.velocity.magnitude;

            base.Update();
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            UpdateVariables();
            UpdateState();
        }


        public void SetTrackingAbility(bool ability, bool reset = false)
        {
            if (navMeshAgent != null)
            {
                navMeshAgent.isStopped = !ability;
                if (reset) navMeshAgent.ResetPath();
            }
        }


        public Vector3 GetDirection()
        {
            return navMeshAgent.velocity;
        }

        public void GoTo(Vector3 target)
        {
            currentTarget = target;
        }
        /*
        public void GoTo(Vector3 target)
        {
            targetPoints.Enqueue(target);
        }

        TimerManager.Timer walkTimer;


        public void UpdateLocomotion()
        {
            if ((currentTarget - transform.position).magnitude < 0.05f || !TimerManager.isRunning(walkTimer))
            {
                if (targetPoints.Count > 0)
                {
                    currentTarget = targetPoints.Dequeue();
                    navMeshAgent.SetDestination(currentTarget);
                    walkTimer = TimerManager.StartTimer(5f,()=> { });
                }

            }
        }*/

        public void UpdateLocomotion()
        {
            if(navMeshAgent.isActiveAndEnabled && navMeshAgent.isOnNavMesh)
                navMeshAgent.SetDestination(currentTarget);
        }


        private Player CheckVisiblePlayer()
        {
            var minDist = float.MaxValue;
            Player minPlayer = null;
            for (var i = 0; i < 4; i++)
            {
                var p = PlayerManager.playerManager.players[i];
                if (p != null)
                    if (p.IsAlive())
                    {
                        var dir = p.transform.position - transform.position;
                        var dist = dir.magnitude;

                        if (Vector3.Dot(transform.forward, dir) > 0 && dist <= me.viewDistance && dist < minDist)
                        {
                            minPlayer = p;
                            minDist = dist;
                        }
                    }
            }

            var walls = me.seeThroughWalls;
            if (walls ? true : CheckLineOfSight(minPlayer))
                return minPlayer;
            return null;
        }

        public bool CheckLineOfSight(Entity target)
        {
            if (target == null) return false;

            var layerMask = 0;

            layerMask = ~layerMask;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit,
                Mathf.Infinity, layerMask))
            {
                if (hit.collider.gameObject.GetComponentInParent<Player>()) return true;

                return false;
            }

            return false;
        }

        private void UpdateSeenPlayer()
        {
            var currentViewedPlayer = CheckVisiblePlayer();
            if (currentViewedPlayer == null)
            {
                // start timer for 2 seconds then drop player
                if (lastSeenPlayer != null)
                    if (me.forgetPlayer)
                        visibilityTimer = TimerManager.StartTimer(forgetTime, () => { ForgetPlayer(); });
            }
            else
            {
                TimerManager.StopTimer(visibilityTimer);

                lastSeenPlayer = currentViewedPlayer;
                seenPlayer = currentViewedPlayer;
            }
        }


        private void ForgetPlayer()
        {
            seenPlayer = null;
        }

        private void UpdateVariables()
        {
            UpdateSeenPlayer();
            UpdateDistance();
        }


        private void UpdateDistance()
        {
            seenAlly = CheckClosestAlley();
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


        private void UpdateState()
        {
            me.FSM.ExecuteState();
        }


        private Enemy CheckClosestAlley()
        {
            Enemy minEnemy = null;
            /*
            foreach (GameObject g in Level.GetAllEnemies())
            {
                Enemy e = g.GetComponent<Enemy>();
                if (e != null && e != me && e.GetState() != Enemy.EnemyState.Dead)
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
            */
            return minEnemy;
        }
    }
}