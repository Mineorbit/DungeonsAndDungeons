using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    bool onPlayer = true;
    public Transform target;

    public Quaternion rotationHand;
    public Vector3 offsetHand;

    public void Start()
    {

        animateDefault();
        attachItem();
    }

    public void Update()
    {
       attachItem();
    }
    public void tryAttachItem()
    {
       if(Input.GetKeyDown(KeyCode.E))
        {
            attachItem();
        } 
    }
    void attachItem()
    {
        this.transform.parent = target;
        transform.localPosition = offsetHand;
        transform.localRotation = rotationHand;
    }

    public virtual void animateDefault()
    {
    }
    public virtual void animateAction()
    {

    }
}
