using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public string name;
    public virtual void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else instance = this;
    }
    public virtual void Reset()
    {

    }
    public virtual void performAction()
    {
    }
    public virtual void performAction(GameManager.GameAction g)
    {
        Debug.Log($"[Manager] Performing {g}");
    }
}
