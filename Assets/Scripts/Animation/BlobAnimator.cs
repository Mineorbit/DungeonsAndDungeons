using UnityEngine;
using UnityEngine.Events;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class BlobAnimator : MonoBehaviour
    {
        public float speed;

        public Vector3 target;

        public UnityEvent attackEvent;
        public UnityEvent endAttackEvent;
        private Animator animator;

        private void Start()
        {
            animator = gameObject.GetComponent<Animator>();
            attackEvent = new UnityEvent();
            endAttackEvent = new UnityEvent();
        }

        private void Update()
        {
            if (animator != null)
                animator.SetFloat("Speed", speed);


            var angle = 180 + 180 / Mathf.PI * Mathf.Atan2(target.x, target.z);

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