using System.Collections.Generic;
using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;
using UnityEngine.Events;

public abstract class Openable : MonoBehaviour
{
    public bool open;

    public UnityEvent openEvent;
    public UnityEvent closeEvent;

    private readonly Queue<Act> actions = new Queue<Act>();

    private bool finished;


    public bool Finished
    {
        get => finished;
        set
        {
            finished = value;
            if (finished) Process();
        }
    }

    public virtual void Awake()
    {
        Finished = true;
    }

    public void Open()
    {
        actions.Enqueue(Act.Open);
        Process();
    }

    public void Close()
    {
        actions.Enqueue(Act.Close);
        Process();
    }


    private void Process()
    {
        Debug.Log(Finished);
        if (Finished)
            if (actions.Count > 0)
            {
                var todo = actions.Dequeue();
                if (todo == Act.Open && !open)
                {
                    Finished = false;
                    open = true;
                    OnOpen();
                    MainCaller.Do(() => { Invoke("FinishOpen", 1f); });
                }
                else if (todo == Act.Close && open)
                {
                    Finished = false;
                    open = false;

                    OnClose();
                    MainCaller.Do(() => { Invoke("FinishClose", 1f); });
                }
            }
    }

    private void FinishOpen()
    {
        openEvent.Invoke();
        Finished = true;
    }

    private void FinishClose()
    {
        closeEvent.Invoke();
        Finished = true;
    }

    public abstract void OnOpen();
    public abstract void OnClose();

    private enum Act
    {
        Open,
        Close
    }
}