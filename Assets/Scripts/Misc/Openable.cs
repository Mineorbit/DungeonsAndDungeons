using com.mineorbit.dungeonsanddungeonscommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Openable : MonoBehaviour
{
    enum Act { Open, Close };

    Queue<Act> actions = new Queue<Act>();

    bool finished;

    public bool open;

    public UnityEvent openEvent;
    public UnityEvent closeEvent;


    public bool Finished
    {
        get { return finished; }
        set { finished = value; if(finished) Process(); }
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


    void Process()
    {
        Debug.Log(Finished);
           if (Finished)
           {
               if (actions.Count > 0)
                {
                    Act todo = actions.Dequeue();
                    if (todo == Openable.Act.Open && !open)
                    {
                        Finished = false;
                        open = true;
                        OnOpen();
                        MainCaller.Do(() => { Invoke("FinishOpen", 1f); });
                    }
                    else 
                    if (todo == Openable.Act.Close && open)
                    {
                        Finished = false;
                        open = false;
                        
                        OnClose();
                        MainCaller.Do(() =>{Invoke("FinishClose", 1f);});
                    }
               }
           }
    }

    void FinishOpen()
    {
        openEvent.Invoke();
        Finished = true;
    }

    void FinishClose()
    {
        closeEvent.Invoke();
        Finished = true;
    }
    public abstract void OnOpen();
    public abstract void OnClose();
}
