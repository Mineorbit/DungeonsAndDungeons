using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class BlobAnimator : MonoBehaviour
{
    Animator animator;
    public float speed;
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
        if(animator != null)
        animator.SetFloat("Speed",speed);
    }

    public void Attack()
    {
        animator.SetTrigger("Strike");
        attackEvent.Invoke();
    }

    public void EndAttack()
    {
        endAttackEvent.Invoke();
    }
}
