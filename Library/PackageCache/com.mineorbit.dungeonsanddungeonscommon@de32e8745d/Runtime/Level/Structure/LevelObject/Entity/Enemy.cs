using UnityEngine;
using UnityEngine.AI;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class Enemy : Entity
    {

        public float viewDistance = 15;
        public float attackDistance = 4;

        // Enemy Properties

        public bool forgetPlayer;

        public bool seeThroughWalls;
        public int damage = 5;


        public float currentDamage;


        public FSM<EnemyState, EnemyAction> FSM;

        public override void Update()
        {
            base.Update();
        }


        public EnemyController GetController()
        {
            return (EnemyController) controller;
        }
        
        public virtual void OnEnable()
        {
            controller = GetComponent<EnemyController>();
            var n = GetComponent<NavMeshAgent>();
            if (n != null) n.enabled = true;
            controller.enabled = true;
        }

        public virtual void OnDisable()
        {
            controller = GetComponent<EnemyController>();
            var n = GetComponent<NavMeshAgent>();
            if (n != null) n.enabled = false;
            controller.enabled = false;
        }


        public void TryDamage(GameObject g, float damage)
        {
            var c = g.GetComponentInParent<Entity>(true);
            if (c != null)
            {
                c.Hit(this, (int) damage);
            }
        }


        public override void OnInit()
        {
            base.OnInit();

            GetController().seenPlayer = null;
            GetController().seenAlly = null;
            GetController().lastSeenPlayer = null;
        }


        public EnemyState getState()
        {
            return FSM.state;
        }

        public void SetState(EnemyState state)
        {
            FSM.state = state;
        }

        public class EnemyState : CustomEnum
        {
            public static EnemyState Idle = new EnemyState("Idle", 1);
            public static EnemyState PrepareStrike = new EnemyState("PrepareStrike", 2);
            public static EnemyState Attack = new EnemyState("Attack", 3);

            public EnemyState(string val, int card) : base(val, card)
            {
                Value = val;
                cardinal = card;
            }
        }


        public class EnemyAction : CustomEnum
        {
            public static EnemyAction Engage = new EnemyAction("Engage", 1);
            public static EnemyAction Disengage = new EnemyAction("Disengage", 2);
            public static EnemyAction Attack = new EnemyAction("Attack", 3);

            public EnemyAction(string val, int card) : base(val, card)
            {
                Value = val;
                cardinal = card;
            }
        }
    }
}