using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    public UnityEvent attackEvent;
    public UnityEvent attackFinishedEvent;

    private void Awake()
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