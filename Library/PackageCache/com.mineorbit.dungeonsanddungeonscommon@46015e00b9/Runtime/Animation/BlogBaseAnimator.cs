using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class BlogBaseAnimator : EntityBaseAnimator
    {

        public Transform target;

        public UnityEvent attackEvent;
        public UnityEvent endAttackEvent;
        private Vector3 targetInterpolation;

        [FormerlySerializedAs("enemyController")] public new EnemyController entityController;
        

        private void Start()
        {
            targetInterpolation = transform.parent.forward;
            attackEvent = new UnityEvent();
            endAttackEvent = new UnityEvent();
        }

        public override void Update()
        {
            base.Update();

            var targetDirection = new Vector3(0, 0, 0);

            if (target == null)
                targetDirection = transform.parent.forward;
            else
                targetDirection = target.transform.position - transform.parent.position;

            LookInDirection(targetDirection);
        }


        public void LookInDirection(Vector3 dir)
        {
            targetInterpolation = (targetInterpolation + dir) / 2;

            var angle = 180 / Mathf.PI * Mathf.Atan2(targetInterpolation.x, targetInterpolation.z);

            transform.eulerAngles = new Vector3(0, angle, 0);
        }

        /*
        public void Attack()
        {
            animator.SetTrigger("Strike");
            attackEvent.Invoke();
        }
        
        public void EndAttack()
        {
            endAttackEvent.Invoke();
        }
        */
    }
}