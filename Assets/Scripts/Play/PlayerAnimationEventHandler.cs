using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerAnimationEventHandler : MonoBehaviour
{
    public UnityEvent attackEvent;
    public UnityEvent attackFinishedEvent;

    void Awake()
    {
        attackEvent = new UnityEvent();
        attackFinishedEvent = new UnityEvent();
    }

    public void Attack()
    {
        attackEvent.Invoke();
    }

    public void FinishAttack()
    {
        attackFinishedEvent.Invoke();
    }
}
