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
        Debug.Log("Opening " + this);
        actions.Enqueue(Act.Open);
        Process();
    }
    public void Close()
    {
        Debug.Log("Closing "+this);
        actions.Enqueue(Act.Close);
        Process();
    }

    private void Update()
    {
        Process();
    }

    void Process()
    {

           if (Finished)
           {
               if (actions.Count > 0)
                {
                    Finished = false;
                    Act todo = actions.Dequeue();
                    Debug.Log("We got "+todo);
                    if (todo == Openable.Act.Open && !open)
                   {

                       open = true;
                       Debug.Log(this + " open calling " + openEvent);
                       openEvent.Invoke();
                       OnOpen();
                   }
                   else 
                   if (todo == Openable.Act.Close && open)
                   {
                       Finished = false;

                       open = false;
                       Debug.Log(this + " close calling " + closeEvent);
                       closeEvent.Invoke();

                       OnClose();

                   }
               }
           }
    }

    public abstract void OnOpen();
    public abstract void OnClose();
}
