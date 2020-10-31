using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Openable : MonoBehaviour
{
    enum Act { Open, Close };

    Queue<Act> actions;

    bool finished;

    public bool open;

    public bool Finished
    {
        get { return finished; }
        set { finished = value; if(finished) Process(); }
    }

    public void Awake()
    {
        actions = new Queue<Act>();
        Finished = true;
    }

    public void Open()
    {
        actions.Enqueue(Act.Open);
        Process();
    }
    public void Close()
    {

        Debug.Log("Closing");
        actions.Enqueue(Act.Close);
        Process();
    }

    void Process()
    {
        if(Finished)
        {
            if(actions.Count>0)
            {
                Finished = false;
                Act todo = actions.Dequeue();
                if (todo == Openable.Act.Open && !open)
                {
                    open = true;
                    OnOpen();
                }
                else if ( todo == Openable.Act.Close && open)
                {
                    open = false;
                    OnClose();
                }
            }
        }
    }

    public abstract void OnOpen();
    public abstract void OnClose();
}
