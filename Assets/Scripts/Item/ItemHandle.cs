using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandle : MonoBehaviour
{
    public enum HandleType { LeftHand, RightHand};
    public Item slot;
    public HandleType handleType;


    public void Attach(Item item)
    {
        item.transform.position = transform.position;
        item.transform.parent = transform;
        slot = item;
        slot.OnAttach();
    }
    public void Dettach()
    {
        slot.OnDettach();
    }

    public void Use()
    {
        slot.Use();
    }

    public void StopUse()
    {
        slot.StopUse();
    }
}
