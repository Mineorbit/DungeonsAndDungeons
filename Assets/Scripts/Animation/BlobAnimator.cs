using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class BlobAnimator : MonoBehaviour
    {
        Animator animator;
        public float speed;

        public Vector3 target;

        public UnityEvent attackEvent;
        public UnityEvent endAttackEvent;

        void Start()
        {
            animator = gameObject.GetComponent<Animator>();
            attackEvent = new UnityEvent();
            endAttackEvent = new UnityEvent();
        }

        void Update()
        {
            if (animator != null)
                animator.SetFloat("Speed", speed);


            float angle = 180 + (180 / Mathf.PI) * Mathf.Atan2(target.x, target.z);

            transform.eulerAngles = new Vector3(0, angle, 0);
        }

        public void Attack()
        {
            animator.SetTrigger("Strike");
            attackEvent.Invoke();
        }
        public void Hit()
        {
            animator.SetTrigger("Hit");
        }

        public void EndAttack()
        {
            endAttackEvent.Invoke();
        }
    }
}
