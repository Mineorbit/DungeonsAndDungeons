using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject owner;
    public virtual void OnAttach()
    {
        owner = GetComponentInParent<Player>().gameObject;
    }
    public virtual void OnDettach()
    {

    }

    public virtual void Use()
    {

    }

    public virtual void  StopUse()
    {

    }
}
